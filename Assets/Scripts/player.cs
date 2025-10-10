using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int attackDamage = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            // Simulate attacking Polaris
            Enemy enemy = FindObjectOfType<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Player attacked Polaris!");
            }
        }
    }
}
