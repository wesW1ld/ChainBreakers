using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public int maxEnemies = 5;      // Maximum number of enemies allowed
    public Vector3 spawnPosition = new Vector3(-0.4546244f, -0.1548468f, 0.04473706f); // Starting spawn position
    public float spacing = 0.5f;    // How far apart each enemy is placed (along X axis)

    private List<GameObject> enemies = new List<GameObject>(); // Track all active enemies
    private static EnemyManager instance;

    // Singleton pattern
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("EnemyManager is NULL.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Debug.LogWarning("[EnemyManager] Duplicate instance detected, destroying this one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        SpawnAllEnemies();
    }

    // Spawns all enemies (up to maxEnemies), each slightly to the left of the previous one
    public void SpawnAllEnemies()
    {
        if (enemies.Count > 0)
        {
            Debug.Log("[EnemyManager] Enemies already spawned.");
            return;
        }

        for (int i = 0; i < maxEnemies; i++)
        {
            // Offset each enemy along the X axis by "spacing"
            Vector3 offsetPosition = new Vector3(
                spawnPosition.x - (i * spacing),
                spawnPosition.y,
                spawnPosition.z
            );

            SpawnEnemy(offsetPosition);
        }
    }

    // Spawns a single enemy at the given position
    public void SpawnEnemy(Vector3 position)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemies.Add(newEnemy);
        Debug.Log($"[EnemyManager] Spawned enemy #{enemies.Count} at {position}.");
    }

    // Deals damage to a specific enemy by index
    public void DealDamage(int dmg, int enemyIndex)
    {
        if (enemyIndex < 0 || enemyIndex >= enemies.Count)
        {
            Debug.LogWarning("[EnemyManager] Invalid enemy index.");
            return;
        }

        GameObject targetEnemy = enemies[enemyIndex];
        if (targetEnemy != null)
        {
            Enemy enemyScript = targetEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(dmg);
                Debug.Log($"[EnemyManager] Dealt {dmg} damage to enemy #{enemyIndex}.");
            }
            else
            {
                Debug.LogWarning("[EnemyManager] Enemy script missing TakeDamage().");
            }
        }
        else
        {
            Debug.LogWarning("[EnemyManager] Enemy reference is null.");
        }
    }

    public void DealDamage(int dmg)
    {
        int enemyIndex = 0;
        bool enemyFound = false;
        while (!enemyFound && (enemyIndex < enemies.Count))
        {
            if (enemies[enemyIndex] == null)
            {
                enemyIndex++;
            }
            else
            {
                enemyFound = true;
            }
        }

        if ((enemyIndex >= enemies.Count))
        {
            StartCoroutine(GoToEnd());
        }
        else
        {
            enemies[enemyIndex].GetComponent<Enemy>().TakeDamage(dmg);
        }
    }

    // Called when an enemy dies
    public void EnemyDestroyed(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log("[EnemyManager] Enemy removed from list.");
        }
    }

    // Destroy all enemies
    public void ClearAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        enemies.Clear();
        Debug.Log("[EnemyManager] All enemies cleared.");
    }

    IEnumerator GoToEnd()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
