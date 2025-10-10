using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Polaris prefab
    public Transform spawnPoint;   // A single fixed spawn point
    private GameObject currentEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        // Prevent duplicate spawns
        if (currentEnemy != null)
        {
            Debug.Log("[EnemyManager] Enemy already exists, not spawning another.");
            return;
        }

        // Spawn Polaris on the left side of the screen
        if (spawnPoint != null)
        {
            currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("[EnemyManager] Spawned Polaris on the left side.");
        }
        else
        {
            // Fallback if no spawn point is assigned
            Vector3 leftSidePosition = new Vector3(-7f, 0f, 0f); // Adjust X to move further left
            currentEnemy = Instantiate(enemyPrefab, leftSidePosition, Quaternion.identity);
            Debug.Log("[EnemyManager] Spawned Polaris at default left position.");
        }
    }
}
