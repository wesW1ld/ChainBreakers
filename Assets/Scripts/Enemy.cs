using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Polaris";
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 20;
    public int damageReceived = 10;
    private bool defending = false;

    private int NumChoices = 3;
    private float[] ProbabiltyMatrix;//attack, defense, special weights

    private void Start()
    {
        currentHP = maxHP;
        ProbabiltyMatrix = new float[] { .5f, .2f, .3f };
        float sum = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            sum += ProbabiltyMatrix[i];
        }
        if(sum != 1)
        {
            Debug.Log($"Sum is equal to {sum}, not 1");
        }
    }

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
        int choice = 0;
        for (int i = 0; i < NumChoices; i++)
        {
            if (probabilities[i] > actionNum)
            {
                choice = i;
                i = NumChoices;
            }
        }
    
        switch(choice)
        {
            case 0:
                Debug.Log("Attacking");
                PlayerManager.instance.TakeDamage(100f);
                break;
            case 1:
                Debug.Log("Defending");
                defending = true;
                break;
            default:
                Debug.Log("Not implemented yet");
                break;
        }
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
}

