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

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

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

    //public void AttackPlayer(Player player)
    //{
    //    player.TakeDamage(attackPower);
    //}

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

