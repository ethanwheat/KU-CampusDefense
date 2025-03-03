using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public GameObject enemyPrefab;   // Assign enemy prefab (to spawn)
    public BoxCollider[] spawnAreas;  // Where enemies will spawn
    public PathNode[] startNodes;
    public int enemiesPerRound = 20;  // How many enemies spawn per round

    private int currentRound = 0;

    public void StartRound()
    {
        currentRound++;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {

            int spawnIndex = Random.Range(0, spawnAreas.Length);
            BoxCollider randomSpawnArea = spawnAreas[spawnIndex];

            Vector3 spawnPosition = GetRandomPointInBounds(randomSpawnArea.bounds);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
            if (enemyScript != null)
            {
                enemyScript.currentNode = startNodes[spawnIndex];
            }
            
            // Set the parent of the new enemy.
            GameObject parent = GameObject.Find("Enemies");

            if (!parent)
            {
                parent = new GameObject("Enemies");
            }

            Transform parentTransform = parent.transform;

            GameObject enemy = Instantiate(enemyPrefab, parentTransform);
            Transform enemyTransform = enemy.transform;
            enemyTransform.position = randomPosition;
            enemyTransform.rotation = Quaternion.identity;

            //Enemy enemyScript = enemy.GetComponent<Enemy>();
            //if (enemyScript != null)
            //{
            //enemyScript.spawnPoint = randomSpawnArea.transform; // Store spawn area

            yield return new WaitForSeconds(1f);
        }
    }
    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.center.y;
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}

