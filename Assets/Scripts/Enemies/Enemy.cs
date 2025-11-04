using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    protected int NumChoices = 3;
    protected float[] ProbabiltyMatrix;//attack, defense, special weights   protected:child can access

    private void Start()
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
                Attack();
                break;
            case 1:
                Debug.Log("Defending");
                defending = true;
                textUI.text = "Defending";
                break;
            default:
                SpecialAttack(choice);
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
        textUI.text = "Undefined";
    }

    public virtual void Attack()
    {
        PlayerManager.instance.TakeDamage(100f);
        textUI.text = "Attacking for 100";
    }
}

