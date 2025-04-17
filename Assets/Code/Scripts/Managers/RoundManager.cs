using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

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

    [Header("Parents")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private Transform defensesParent;
    [SerializeField] private Transform placementParent;

    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private List<Defense> defenses = new List<Defense>();
    private List<Defense> healthDefenses = new List<Defense>();
    private bool isAllEnemiesSlowed = false;
    private bool isAllEnemiesFrozen = false;
    private float slowMultiplier = 1;

    public Transform PlacementParent => placementParent;
    public Transform DefenseParent => defensesParent;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentRound = gameData.SelectedRound;
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
            WeatherManager.instance.ResetWeatherForNewRound();
            WeatherManager.instance.TryActivateWeather();
            roundSceneUIController.updateWaveUI(currentWave + 1, numWaves);
            roundSceneUIController.showWavePopupPanel((currentWave + 1).ToString());
            yield return StartCoroutine(SpawnEnemies(currentRound.waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(10f);
        }
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
            while (isAllEnemiesFrozen)
            {
                yield return null;
            }

            int spawnIndex = Random.Range(0, spawnAreas.Length);
            Vector3 spawnPosition = GetRandomPointInBounds(spawnAreas[spawnIndex].bounds);

            // Set the parent of the new enemy.
            GameObject enemy = Instantiate(prefab, enemiesParent.transform);
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            if (enemy.TryGetComponent(out EnemyMovement enemyScript))
            {
                enemies.Add(enemyScript);
                enemyScript.SetSpeed(speed);
                enemyScript.SetHealth(health);
                enemyScript.SetReward(currentRound.killPayout);

                if (isAllEnemiesSlowed)
                {
                    enemyScript.SetSpeedMultiplier(slowMultiplier);
                }

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
        BonusData[] bonuses = gameData.BonusData;

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

    public void EnemyDefeated(EnemyMovement enemyDefeated)
    {
        if (gameOver) return;

        remainingEnemies--;
        enemies.Remove(enemyDefeated);

        // Check if the round is over (no remaining enemies)
        if (remainingEnemies <= 0)
        {
            GameOver();

            int debt = gameData.getDebt();
            int reward = (int)(currentRound.winPayout * dollarMultiplier);

            if (debt > 0)
            {
                int owed = Mathf.Min((int)(reward * 0.3f), debt);  // 30% debt payment
                gameData.payDebt(owed);
                gameData.addDollars(reward - owed);
                roundSceneUIController.showRoundWonPanel(
                    gameData.RoundNumber.ToString(),
                    reward.ToString(),
                    "- $" + owed.ToString(),
                    "$" + (reward - owed).ToString()
                );
            }
            else
            {
                gameData.addDollars(reward);
                roundSceneUIController.showRoundWonPanel(
                    gameData.RoundNumber.ToString(),
                    reward.ToString(),
                    "",
                    ""
                );
            }

            if (gameData.RoundNumber == currentRound.roundNumber)
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
            GameOver();
            roundSceneUIController.showRoundLostPanel();
        }
    }

    private void GameOver()
    {
        gameOver = true;

        foreach (var enemy in enemies)
        {
            enemy.enabled = false;
        }

        foreach (var defense in defenses)
        {
            defense.enabled = false;
        }
    }

    public void addDefense(Defense defense)
    {
        defenses.Add(defense);

        if (defense.gameObject.CompareTag("HealthDefense"))
        {
            healthDefenses.Add(defense);
        }
    }

    public void removeDefense(Defense defense)
    {
        defenses.Remove(defense);

        if (healthDefenses.Contains(defense))
        {
            healthDefenses.Remove(defense);
        }
    }

    public void regenHealthOnDefenses()
    {
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

        foreach (var healthDefense in healthDefenses)
        {
            regenCost += healthDefense.getDefenseData().getCoinCost() / 2;
        }

        return regenCost;
    }

    public void SlowAllEnemies(float multiplier)
    {
        isAllEnemiesSlowed = true;
        slowMultiplier = multiplier;

        foreach (var enemy in enemies)
        {
            enemy.SetSpeedMultiplier(multiplier);
        }
    }


    public void ResetSlowAllEnemies()
    {
        isAllEnemiesSlowed = false;
        slowMultiplier = 1;

        foreach (var enemy in enemies)
        {
            enemy.ResetSpeedMultiplier();
        }
    }

    public void FreezeAllEnemies()
    {
        isAllEnemiesFrozen = true;

        foreach (var enemy in enemies)
        {
            enemy.BlockMovement(true);
        }
    }

    public void ResetFreezeAllEnemies()
    {
        isAllEnemiesFrozen = false;

        foreach (var enemy in enemies)
        {
            enemy.BlockMovement(false);
        }
    }
}

