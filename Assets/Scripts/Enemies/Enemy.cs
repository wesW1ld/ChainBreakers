using System;
using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Polaris";
    public int maxHP = 100;
    public int currentHP;
    public float attackPower = 25f;
    public int damageReceived = 10;
    private bool defending = false;
    public int[] Count;
    public float missChance = 0f;
    public float damageMult = 1f;
    public float enemyMissChance = 0f;
    protected bool dazed = false;

    private int choice;

    protected int NumChoices = 3;

    [SerializeField]
    protected float[] ProbabiltyMatrix;//attack, defense, special weights   protected:child can access
    protected float[] tProbabiltyMatrix; //for temp storage

    public virtual void Start()
    {
        currentHP = maxHP;
        ProbabiltyMatrix = new float[] { .7f, .3f, .0f };
        tProbabiltyMatrix = (float[])ProbabiltyMatrix.Clone();
        MakePMatrix();
        float sum = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            sum += ProbabiltyMatrix[i];
        }
        if (sum != 1)
        {
            Debug.Log($"Sum is equal to {sum}, not 1");
        }

        //action ui
        ui = Instantiate(EnemyManager.Instance.textPrefab, UIManager.instance.UICanvas.transform);
        textUI = ui.GetComponent<TextMeshProUGUI>();
        textUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1);

        //hp ui
        uiHP = Instantiate(EnemyManager.Instance.textPrefab, UIManager.instance.UICanvas.transform);
        textUIHP = uiHP.GetComponent<TextMeshProUGUI>();
        textUIHP.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
        textUIHP.text = $"{currentHP} / {maxHP}";

        //status ui
        uiSta = Instantiate(EnemyManager.Instance.textPrefab, UIManager.instance.UICanvas.transform);
        textUISta = uiSta.GetComponent<TextMeshProUGUI>();
        textUISta.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * -2);
        textUISta.text = "no status";

        StartCoroutine(PickChoice());
    }
    
    protected TextMeshProUGUI textUI;
    protected TextMeshProUGUI textUIHP;
    protected TextMeshProUGUI textUISta;
    GameObject ui;
    GameObject uiHP;
    GameObject uiSta;

    public void TakeDamage(int amount)
    {
        if(Random.value < enemyMissChance)
        {
            Debug.Log("Player Missed");
        }
        else if(defending)
        {
            currentHP -= amount / 2;
            defending = false;
        }
        else
        {
            currentHP -= amount;
        }

        textUIHP.text = $"{currentHP} / {maxHP}";
        

        // Update score using your existing singleton ScoreManager
        if (scoreManager.Instance != null)
        {
            scoreManager.Instance.ChangeScore(amount);
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found!");
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void EnemyAction()
    {
        if(Random.value >= missChance)
        {
            switch (choice)
            {
                case 0:
                    //Debug.Log("Attacking");
                    Attack();
                    Debug.Log($"attacking with mult {damageMult} and miss chance of {missChance}");
                    break;
                case 1:
                    //Debug.Log("Defending");
                    defending = true;
                    break;
                default:
                    SpecialAttack(choice);
                    break;
            }
        }
        else
        {
            Debug.Log("Missed");
        }
        
        //reset status stuff
        ProbabiltyMatrix = (float[])tProbabiltyMatrix.Clone(); //restore temp if different
        missChance = 0f;
        damageMult = 1f;
        enemyMissChance = 0f;
        dazed = false;

        UpdatePMatrix();
        tProbabiltyMatrix = (float[])ProbabiltyMatrix.Clone(); //make same
        ApplyStatus();
        StartCoroutine(PickChoice());
    }

    private void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");

        // Give bonus score when Polaris is defeated
        if (scoreManager.Instance != null)
        {
            scoreManager.Instance.ChangeScore(50);
        }

        Destroy(ui);
        Destroy(uiHP);
        Destroy(uiSta);
        EnemyManager.Instance.EnemyDestroyed(gameObject);
        Destroy(gameObject);
    }

    public virtual void MakePMatrix()
    {
        float sum = 0;
        for (int i = 0; i < ProbabiltyMatrix.Length; i++)
        {
            sum += ProbabiltyMatrix[i];
        }
        if(sum != 1f)
        {
            Debug.LogError("Probability Matrix doesn't add up to 1");
        }
    }

    public virtual void SpecialAttack(int i)
    {
        Debug.Log("Not implemented yet/No special");
    }

    public virtual void Attack()
    {
        if(dazed)
        {
            if(Random.Range(0, 2) == 1)
            {
                TakeDamage((int)(25f * damageMult));
                Debug.Log("hit self");
                return;
            }
        }
        PlayerManager.instance.TakeDamage((int)(attackPower * damageMult));
    }

    IEnumerator PickChoice()
    {
        yield return new WaitForSeconds(0f);

        //turns {.5, .2, .3} into {.5, .7, 1} for ranges of options for random choice
        NumChoices = ProbabiltyMatrix.Length;
        float[] probabilities = new float[NumChoices];
        float prev = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            float num = prev + ProbabiltyMatrix[i];
            probabilities[i] = num;
            prev += ProbabiltyMatrix[i];
        }

        // Debug.Log($"PMatrix: {ProbabiltyMatrix[0]} {ProbabiltyMatrix[1]} {ProbabiltyMatrix[2]}");
        // Debug.Log($"probabilities: {probabilities[0]} {probabilities[1]} {probabilities[2]}");

        float actionNum = Random.value;
        choice = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            if (probabilities[i] > actionNum)
            {
                choice = i;
                i = NumChoices;
            }
        }

        switch (choice)
        {
            case 0:
                AttackPreview();
                break;
            case 1:
                textUI.text = "Defending";
                break;
            default:
                SpecialAttackPreview(choice);
                break;
        }
    }

    public virtual void AttackPreview()
    {
        textUI.text = $"Attacking for {attackPower}";
    }
    
    public virtual void SpecialAttackPreview(int i)
    {
        textUI.text = "Undefined";
    }

    public virtual void UpdatePMatrix() //updates P matrix based on the cards the player plays
    {
        //player card types: attack, defend, status, special
        //if player attacks alot, defend more
        //if player defends alot, special more, attack if no special
        //if player status alot, attack more
        //if player specials, alot defend more?
        //gonna start by checking if one card type was played much more often than other
        //maybe see if the difference between most and 2nd most played is greater than a quarter of the total
        //same for least
        //also update these based on health for boss too

        //fill Count array will number of each type from types enum in Card.cs
        //index relates to type, ei Attack = 0
        Card[] CardStack = PlayList.instance.SeeStack();
        int size = CardStack.Length;
        Count = new int[4];
        foreach(Card card in CardStack)
        {
            Count[(int)card.cardType] += 1;
        }

        //get array of sorted indexes for comparison
        int[] sortedIndexes = Enumerable
            .Range(0, Count.Length)
            .OrderByDescending(i => Count[i])
            .ToArray();

        //get a quarter of the sum of types used
        int qsum = 0;
        for(int i = 0; i < 4; i++)
        {
            qsum += Count[i];
        }
        qsum /= 4;

        //if one is used alot more than others, else if one is used alot less than others
        if(Count[sortedIndexes[0]] > qsum + Count[sortedIndexes[1]])
        {
            UpdateBasedOnUsed((Card.CardType)sortedIndexes[0], true);
        }
        else if(Count[sortedIndexes[2]] > qsum + Count[sortedIndexes[3]])
        {
            UpdateBasedOnUsed((Card.CardType)sortedIndexes[3], false);
        }
    }

    private void UpdateBasedOnUsed(Card.CardType type, bool most)
    {
        Card.CardType add;
        Card.CardType remove;

        //if theres alot of attacking, take away from attacking and add to defending
        if(type == Card.CardType.Attack)
        {
            if(most)
            {
                //Debug.Log("attack is most played");
                remove = Card.CardType.Attack;
                add = Card.CardType.Defend;
            }
            else//not alot of attacking, swap
            {
                //Debug.Log("attack is least played");
                remove = Card.CardType.Defend;
                add = Card.CardType.Attack;
            }
            if(ProbabiltyMatrix[(int)remove] > .2f)
            {
                ProbabiltyMatrix[(int)add] += .2f;
                ProbabiltyMatrix[(int)remove] -= .2f;
            }
        }
        else if(type == Card.CardType.Defend || type == Card.CardType.Status) //if theres alot of defending or status, special or attack instead of defending
        {
            if(most)
            {
                //Debug.Log("defend or status is most played");
                remove = Card.CardType.Defend;
                if(ProbabiltyMatrix[2] != 0) //if the enemy has a special attack
                {
                    add = Card.CardType.Special - 1; //enemy has no status move
                }
                else
                {
                    add = Card.CardType.Attack;
                }
            }
            else //if not alot of defending, attack
            {
                //Debug.Log("defend or status is least played");
                remove = Card.CardType.Defend;
                add = Card.CardType.Attack;
            }
            if(ProbabiltyMatrix[(int)remove] > .2f)
            {
                ProbabiltyMatrix[(int)add] += .2f;
                ProbabiltyMatrix[(int)remove] -= .2f;
            }
        }
    }

    //status stuff
    private struct Status
    {
        public Card.StatusEffect effect;
        public int timeLeft;

        public Status(Card.StatusEffect status, int time)
        {
            effect = status;
            timeLeft = time;
        }
    }
    private List<Status> statusEffects = new List<Status>();

    public void GiveEnemyStatus(Card.StatusEffect status, int time)
    {
        int index = statusEffects.FindIndex(s => s.effect == status);
        if (index != -1)
        {
            // Get the struct, modify it, then write it back
            Status s = statusEffects[index];
            if(s.timeLeft < time){s.timeLeft = time;}
            statusEffects[index] = s;  //assign back
        }
        else
        {
            statusEffects.Add(new Status(status, time));
        }
        UpdateStaText();
    }

    private void UpdateStaText()
    {
        textUISta.text = "";
        foreach(Status s in statusEffects)
        {
            textUISta.text += s.effect.ToString();
            textUISta.text +=s.timeLeft;
        }
        if(statusEffects.Count == 0)
        {
            textUISta.text = "no status";
        }
    }

    //         Dazed, chance to hit self
    //         Enraged, always attack
    //         Emboldened, more damage
    //         Shocked, miss a turn
    //         Rage, do more damage, % chance to miss
    //         Cloaked, cant be hit
    //         Blur, % chance to not be hit
    //         Binded, % chance to do more damage, otherwise miss
    private void ApplyStatus()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {

            Status status = statusEffects[i];
            switch(status.effect)
            {
                case Card.StatusEffect.Dazed:
                    dazed = true;
                    break;
                case Card.StatusEffect.Enraged:
                    choice = 0;
                    Enrage();
                    if(missChance < .5f){missChance = .5f;}
                    break;
                case Card.StatusEffect.Emboldened:
                    damageMult += .2f;
                    break;
                case Card.StatusEffect.Shocked:
                    missChance = 1f;
                    break;
                case Card.StatusEffect.Rage:
                    damageMult += .5f;
                    if(missChance < .5f){missChance = .5f;}
                    break;
                case Card.StatusEffect.Cloaked:
                    enemyMissChance = 1f;
                    break;
                case Card.StatusEffect.Blur:
                    if(enemyMissChance < .3f){enemyMissChance = .3f;}
                    break;
                case Card.StatusEffect.Binded:
                    damageMult += .3f;
                    if(missChance < .6f){missChance = .6f;}
                    break;
                default:
                    Debug.Log("dont use might or poise or regenerative");
                    break;
            }
            if(status.timeLeft < 2)
            {
                statusEffects.Remove(status);
                UpdateStaText();
            }
        }

        for(int j = 0; j < statusEffects.Count; j++)
        {
            Status se = statusEffects[j];
            se.timeLeft--;
            statusEffects[j] = se;
        }
        UpdateStaText();
    
    }

    public virtual void Enrage()
    {
        ProbabiltyMatrix = new float[] {1f, 0f, 0f};
    }
}

