using System;
using System.Collections;
using System.Collections.Generic;
using ChainBreakers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Polaris";
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 20;
    public int damageReceived = 10;
    private bool defending = false;
    public int[] Count;
    public float missChance = 0f;
    public float damageMult = 1f;
    public float enemyMissChance = 0f;

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

        var ui = Instantiate(EnemyManager.Instance.textPrefab, FindObjectOfType<Canvas>().transform);
        textUI = ui.GetComponent<TextMeshProUGUI>();
        textUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);

        var uiHP = Instantiate(EnemyManager.Instance.textPrefab, FindObjectOfType<Canvas>().transform);
        textUIHP = uiHP.GetComponent<TextMeshProUGUI>();
        textUIHP.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3);
        textUIHP.text = $"{currentHP} / {maxHP}";

        StartCoroutine(PickChoice());
    }
    
    protected TextMeshProUGUI textUI;
    protected TextMeshProUGUI textUIHP;

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
        PlayerManager.instance.TakeDamage((int)(100f * damageMult));
    }

    IEnumerator PickChoice()
    {
        yield return new WaitForSeconds(1f);

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
        textUI.text = "Attacking for 100";
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
        statusEffects.Add(new Status(status, time));
    }

    //         Dazed, always defend
    //         Enraged, always attack
    //         Emboldened, always specials (attacks if no special), with chance to miss
    //         Shocked, miss a turn
    //         Rage, do more damage, % chance to miss
    //         Cloaked, cant be hit
    //         Blur, % chance to not be hit
    //         Binded, % chance to do more damage, otherwise miss
    //         Regenerative, heal % at end of turn
    private void ApplyStatus()
    {
        foreach(Status status in statusEffects)
        {
            switch(status.effect)
            {
                case Card.StatusEffect.Dazed:
                    //make Pmatrix always defend, save and restore outside after
                    if(NumChoices == 4)
                    {
                        ProbabiltyMatrix = new float[] {0f, 1f, 0f, 0f};
                    }
                    else
                    {
                        ProbabiltyMatrix = new float[] {0f, 1f, 0f};
                    }
                    break;
                case Card.StatusEffect.Enraged:
                    if(NumChoices == 4)
                    {
                        ProbabiltyMatrix = new float[] {1f, 0f, 0f, 0f};
                    }
                    else
                    {
                        ProbabiltyMatrix = new float[] {1f, 0f, 0f};
                    }
                    break;
                case Card.StatusEffect.Emboldened:
                    if(NumChoices == 4)
                    {
                        ProbabiltyMatrix = new float[] {0f, 0f, .8f, .2f};
                    }
                    else
                    {
                        ProbabiltyMatrix = new float[] {0f, 0f, 1f};
                    }
                    missChance = .35f;
                    break;
                case Card.StatusEffect.Shocked:
                    missChance = 1f;
                    break;
                case Card.StatusEffect.Rage:
                    damageMult = 1.5f;
                    missChance = .5f;
                    break;
                case Card.StatusEffect.Cloaked:
                    enemyMissChance = 1f;
                    break;
                case Card.StatusEffect.Blur:
                    enemyMissChance = .3f;
                    break;
                case Card.StatusEffect.Binded:
                    //% chance to do more damage, otherwise miss
                    if(Random.value < .3f)
                    {
                        damageMult = 1.75f;
                    }
                    else
                    {
                        missChance = 1f;
                    }
                    break;
                case Card.StatusEffect.Regenerative:
                    currentHP += (int)(maxHP * .2);//heal 20%
                    if(currentHP > maxHP)
                    {
                        currentHP = maxHP;
                    }
                    textUIHP.text = $"{currentHP} / {maxHP}";
                    break;
                default:
                    Debug.Log("dont use might or poise");
                    break;
            }
            if(status.timeLeft < 2)
            {
                statusEffects.Remove(status);
            }
        }

        for(int i = 0; i < statusEffects.Count; i++)
        {
            Status se = statusEffects[i];
            se.timeLeft--;
            statusEffects[i] = se;
        }
    }
}

