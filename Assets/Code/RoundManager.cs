using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public Button startRoundButton;  // Assign this in the Unity Inspector
    public GameObject enemyPrefab;   // Assign enemy prefab (to spawn)
    public BoxCollider[] spawnAreas;  // Where enemies will spawn
    public int enemiesPerRound = 20;  // How many enemies spawn per round

    private int currentRound = 0;

    void Start()
    {
        // Attach the button's click event
        startRoundButton.onClick.AddListener(StartRound);
    }

    void StartRound()
    {
        currentRound++;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {

            
            BoxCollider randomSpawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

            Vector3 randomPosition = GetRandomPointInBounds(randomSpawnArea.bounds);

            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            //Enemy enemyScript = enemy.GetComponent<Enemy>();
            //if (enemyScript != null)
            //{
                //enemyScript.spawnPoint = randomSpawnArea.transform; // Store spawn area
            //}

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

