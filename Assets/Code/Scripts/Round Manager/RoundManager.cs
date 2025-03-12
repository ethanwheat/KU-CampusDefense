using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject fanPrefab;
    [SerializeField] private GameObject coachPrefab;
    [SerializeField] private GameObject cheerleaderPrefab;

    [Header("Prefab Speeds")]
    [SerializeField] private float fanSpeed = 15f;
    [SerializeField] private float coachSpeed = 10f;
    [SerializeField] private float cheerleaderSpeed = 25f;

    [Header("Prefab Healths")]
    [SerializeField] private float fanHealth = 100;
    [SerializeField] private float coachHealth = 200;
    [SerializeField] private float cheerleaderHealth = 50;

    [Header("Round Initialization")]
    [SerializeField] private BoxCollider[] spawnAreas;  // Where enemies will spawn
    [SerializeField] private PathNode[] startNodes;

    [Header("Round Configuration")]
    [SerializeField] private RoundData currentRound;

    [Header("Round Data")]
    [SerializeField] private int coins;
    [SerializeField] private int currentWave = 0;

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

        StartCoroutine(StartRound());
    }

    // Start round.
    IEnumerator StartRound()
    {
        while (currentWave < currentRound.waves.Length)
        {
            yield return StartCoroutine(SpawnEnemies(currentRound.waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(3f);
        }

        Debug.Log("All waves complete!");
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        yield return SpawnEnemyType(fanPrefab, wave.fans, wave.spawnInterval, fanSpeed, fanHealth);
        yield return SpawnEnemyType(coachPrefab, wave.coaches, wave.spawnInterval, coachSpeed, coachHealth);
        yield return SpawnEnemyType(cheerleaderPrefab, wave.cheerleaders, wave.spawnInterval, cheerleaderSpeed, cheerleaderHealth);
    }

    IEnumerator SpawnEnemyType(GameObject prefab, int count, float interval, float speed, float health)
    {
        for (int i = 0; i < count; i++)
        {

            int spawnIndex = Random.Range(0, spawnAreas.Length);
            Vector3 spawnPosition = GetRandomPointInBounds(spawnAreas[spawnIndex].bounds);

            // Set the parent of the new enemy.
            GameObject parent = GameObject.Find("Enemies") ?? new GameObject("Enemies");
            GameObject enemy = Instantiate(prefab, parent.transform);
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
            if (enemyScript != null)
            {
                enemyScript.SetSpeed(speed);
                enemyScript.SetHealth(health);
                enemyScript.currentNode = startNodes[spawnIndex];
            }

            yield return new WaitForSeconds(interval);
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

}

