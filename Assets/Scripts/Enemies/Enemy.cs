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

    private int choice;

    protected int NumChoices = 3;
    protected float[] ProbabiltyMatrix;//attack, defense, special weights   protected:child can access

    public virtual void Start()
    {
        currentHP = maxHP;
        ProbabiltyMatrix = new float[] { .7f, .3f, .0f };
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

        StartCoroutine(PickChoice());
    }
    
    protected TextMeshProUGUI textUI;

    public void TakeDamage(int amount)
    {
        if(defending)
        {
            currentHP -= amount / 2;
            defending = false;
        }
        else
        {
            currentHP -= amount;
        }
        

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
        switch (choice)
        {
            case 0:
                Debug.Log("Attacking");
                Attack();
                break;
            case 1:
                Debug.Log("Defending");
                defending = true;
                break;
            default:
                SpecialAttack(choice);
                break;
        }
        
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
        PlayerManager.instance.TakeDamage(100f);
    }

    IEnumerator PickChoice()
    {
        yield return new WaitForSeconds(1f);

        //turns {.5, .2, .3} into {.5, .7, 1} for ranges of options for random choice
        float[] probabilities = new float[NumChoices];
        float prev = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            probabilities[i] = prev + ProbabiltyMatrix[i];
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
        int[] Count = new int[4];
        for(int i = 0; i < size; i++)
        {
            int j = 0;
            foreach(Card.CardType type in CardStack[i].cardTypes)
            {
                Count[j] += 1;
                j++;
            }
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
        else if(Count[sortedIndexes[3]] > qsum + Count[sortedIndexes[4]])
        {
            UpdateBasedOnUsed((Card.CardType)sortedIndexes[4], false);
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
                remove = Card.CardType.Attack;
                add = Card.CardType.Defend;
            }
            else//not alot of attacking, swap
            {
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
                remove = Card.CardType.Defend;
                if(ProbabiltyMatrix[3] != 0) //if the enemy has a special attack
                {
                    add = Card.CardType.Special;
                }
                else
                {
                    add = Card.CardType.Attack;
                }
            }
            else //if not alot of defending, attack
            {
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
}

