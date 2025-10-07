using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySystem : MonoBehaviour
{
    [Header("Stats")]
    public float attackDamage = 1f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float moveSpeed = 2f;

    [Header("Target")]
    private Transform player;
    private bool canAttack = true;

    private void Start()
    {
        // Find player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
    }

    private void Update()
    {
        if (player == null) return;

        // Move toward player
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move closer
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Try to attack
            if (canAttack)
                StartCoroutine(AttackPlayer());
        }

        // Face player (optional)
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0; // prevent tilting
        if (lookDir != Vector3.zero)
            transform.forward = lookDir;
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;

        // Call playerManager's TakeDamage
        if (playerManager.Instance != null)
        {
            playerManager.Instance.TakeDamage(attackDamage);
            Debug.Log($"Enemy attacked player for {attackDamage} damage!");
        }

        // Wait before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

