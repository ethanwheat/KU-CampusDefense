using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] private GameDataController gameData;
    [SerializeField] private float coinMultiplier = 1;
    [SerializeField] private float dollarMultiplier = 1;

    [Header("Round Data")]
    [SerializeField] private int coins;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int numWaves = 0;
    [SerializeField] private int remainingEnemies;
    private bool gameOver = false;

    [Header("Allen Fieldhouse")]
    [SerializeField] private float fieldhouseHealth = 1000f;
    [SerializeField] private HealthBar fieldhouseHealthBar; // Reference to UI HealthBar
    private float maxFieldhouseHealth;

    [Header("Sounds")]
    [SerializeField] private AudioClip allenFieldHouseDamageSoundEffect;

    [Header("UI Controllers")]
    [SerializeField] private RoundSceneUIController roundSceneUIController;
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("Transforms")]
    [SerializeField] private Transform defensesParent;

    void Start()
    {
        currentRound = gameData.getSelectedRound();
        fieldhouseHealth = currentRound.fieldhouseHealth;
        maxFieldhouseHealth = fieldhouseHealth;

        numWaves = currentRound.waves.Length;

        foreach (Wave wave in currentRound.waves)
        {
            remainingEnemies += wave.fans + wave.cheerleaders + wave.coaches;
        }

        getBonuses();

        StartCoroutine(StartRound());
    }

    // Start round.
    IEnumerator StartRound()
    {
        roundSceneUIController.updateWaveUI(currentWave + 1, numWaves);
        yield return new WaitForSeconds(1f);

        while (currentWave < numWaves)
        {
            WeatherController.Instance.ResetWeatherForNewRound();
            WeatherController.Instance.TryActivateWeather();
            roundSceneUIController.updateWaveUI(currentWave + 1, numWaves);
            roundSceneUIController.showWavePopupPanel((currentWave + 1).ToString());
            yield return StartCoroutine(SpawnEnemies(currentRound.waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(10f);
        }

        Debug.Log("All waves complete!");
    }

    public void EndRound()
    {
        StopAllCoroutines();
        SoundManager.instance.stopMusic(.5f);
        roundSceneUIController.closeExistingUI();
        StartCoroutine(EndRoundCoroutine());
    }

    private IEnumerator EndRoundCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(.5f));
        SceneManager.LoadScene("Building Scene");
    }

    public void RetryRound()
    {
        StopAllCoroutines();
        SoundManager.instance.stopMusic(.5f);
        roundSceneUIController.closeExistingUI();
        StartCoroutine(RetryRoundCoroutine());
    }

    private IEnumerator RetryRoundCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.fadeInCoroutine(0.5f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            if (enemy.TryGetComponent(out EnemyMovement enemyScript))
            {
                enemyScript.SetSpeed(speed);
                enemyScript.SetHealth(health);
                enemyScript.SetReward(currentRound.killPayout);
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

    void getBonuses()
    {
        BonusData[] bonuses = gameData.getBonusData();

        // Find the highest unlocked coin bonus
        foreach (BonusData bonus in bonuses)
        {
            if (bonus != null && bonus.isBought()) // Check if the bonus is unlocked
            {
                if (bonus.getCoinBonus() > coinMultiplier)
                {
                    coinMultiplier = bonus.getCoinBonus();
                }

                if (bonus.getDollarBonus() > dollarMultiplier)
                {
                    dollarMultiplier = bonus.getDollarBonus();
                }
            }
        }
    }

    // Get coin amount.
    public int getCoinAmount()
    {
        return coins;
    }

    // Add coins.
    public void addCoins(int amount)
    {
        coins += (int)(amount * coinMultiplier);
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

    public void EnemyDefeated()
    {
        if (gameOver) return;

        remainingEnemies--;

        // Check if the round is over (no remaining enemies)
        if (remainingEnemies <= 0)
        {
            int debt = gameData.getDebt();
            int reward = (int)(currentRound.winPayout * dollarMultiplier);

            if (debt > 0)
            {
                int owed = Mathf.Min((int)(reward * 0.3f), debt);  // 30% debt payment
                gameData.payDebt(owed);
                gameData.addDollars(reward - owed);
                roundSceneUIController.showRoundWonPanel(
                    gameData.getRoundNumber().ToString(),
                    reward.ToString(),
                    "- $" + owed.ToString(),
                    "$" + (reward - owed).ToString()
                );
            }
            else
            {
                gameData.addDollars(reward);
                roundSceneUIController.showRoundWonPanel(
                    gameData.getRoundNumber().ToString(),
                    reward.ToString(),
                    "",
                    ""
                );
            }

            if (gameData.getRoundNumber() == currentRound.roundNumber)
            {
                gameData.incrementRoundNumber();  // unlock next round
            }

        }
    }

    public void damageFieldhouse(float damage)
    {
        SoundManager.instance.playSoundEffect(allenFieldHouseDamageSoundEffect, transform, .5f);

        fieldhouseHealth -= damage;

        if (fieldhouseHealthBar != null)
        {
            fieldhouseHealthBar.UpdateHealthBar(fieldhouseHealth, maxFieldhouseHealth);
        }

        if (fieldhouseHealth <= 0 && !gameOver)
        {
            gameOver = true;
            roundSceneUIController.showRoundLostPanel();
        }
    }

    private List<Defense> getHealthDefenses()
    {
        List<Defense> healthDefenses = new List<Defense>();

        foreach (Transform transform in defensesParent)
        {
            GameObject defense = transform.gameObject;

            if (defense.CompareTag("HealthDefense"))
            {
                healthDefenses.Add(defense.GetComponent<Defense>());
            }
        }

        return healthDefenses;
    }

    public void regenHealthOnDefenses()
    {
        List<Defense> healthDefenses = getHealthDefenses();

        foreach (var healthDefense in healthDefenses)
        {
            healthDefense.ResetHealth();
        }

        int regenCost = getRegenCost();

        subtractCoins(regenCost);
    }

    public int getRegenCost()
    {
        int regenCost = 0;
        List<Defense> healthDefenses = getHealthDefenses();

        foreach (var healthDefense in healthDefenses)
        {
            regenCost += healthDefense.GetDefenseData().getCoinCost() / 2;
        }

        return regenCost;
    }
}

