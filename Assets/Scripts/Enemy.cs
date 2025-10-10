using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Polaris";
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 20;
    public int damageReceived = 10;

    private bool inDefenseMode = false;

    private void Start()
    {
        currentHP = maxHP;
        Debug.Log($"{enemyName} has entered battle with {currentHP} HP.");
    }

    public void TakeDamage(int amount)
    {
        // Check if enemy is in defense mode, halve the damage
        if (inDefenseMode)
        {
            amount /= 2;
        }

        currentHP -= amount;
        Debug.Log($"{enemyName} took {amount} damage! Current HP: {currentHP}/{maxHP}");

        // Update score using your existing singleton ScoreManager
        if (scoreManager.Instance != null)
        {
            scoreManager.Instance.ChangeScore(amount);
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found!");
        }

        // Enter defense mode if HP is below half
        if (!inDefenseMode && currentHP <= maxHP / 2 && currentHP > 0)
        {
            EnterDefenseMode();
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void EnterDefenseMode()
    {
        inDefenseMode = true;
        Debug.Log("ENEMY HAS GONE INTO DEFENSE MODE!");
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
