using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;   // Assign enemy prefab (to spawn)

    [Header("Round Initialization")]
    [SerializeField] private BoxCollider[] spawnAreas;  // Where enemies will spawn
    [SerializeField] private PathNode[] startNodes;
    [SerializeField] private int enemiesPerRound = 20;  // How many enemies spawn per round

    [Header("Round Data")]
    [SerializeField] private int coins;
    [SerializeField] private int currentRound = 0;

    private RoundSceneUIController roundSceneUIController;

    void Start()
    {
        // Set roundSceneUIController.
        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "RoundSceneUI")
            {
                roundSceneUIController = currentGameObject.GetComponent<RoundSceneUIController>();
                break;
            }
        }

        StartRound();
    }

    // Start round.
    public void StartRound()
    {
        currentRound++;
        StartCoroutine(SpawnEnemies());
    }

    // Get coin amount.
    public int getCoinAmount()
    {
        return coins;
    }

    // Add coins.
    public void addCoins(int amount)
    {
        coins += amount;
        roundSceneUIController.updateCoinUI();
    }

    // Subtract coins.
    public void subtractCoins(int amount)
    {
        int futureAmount = coins - amount;

        if (futureAmount >= 0)
        {
            coins = futureAmount;
            roundSceneUIController.updateCoinUI();
        }
    }

    // Spawn enemies.
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {

            int spawnIndex = Random.Range(0, spawnAreas.Length);
            BoxCollider randomSpawnArea = spawnAreas[spawnIndex];

            Vector3 spawnPosition = GetRandomPointInBounds(randomSpawnArea.bounds);

            // Set the parent of the new enemy.
            GameObject parent = GameObject.Find("Enemies");

            if (!parent)
            {
                parent = new GameObject("Enemies");
            }

            Transform parentTransform = parent.transform;

            GameObject enemy = Instantiate(enemyPrefab, parentTransform);
            Transform enemyTransform = enemy.transform;
            enemyTransform.position = spawnPosition;
            enemyTransform.rotation = Quaternion.identity;

            EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
            if (enemyScript != null)
            {
                enemyScript.currentNode = startNodes[spawnIndex];
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // Get random point in bounds.
    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.center.y;
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}

