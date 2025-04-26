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
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mascotPrefab;

    [Header("Prefab Speeds")]
    [SerializeField] private float fanSpeed = 15f;
    [SerializeField] private float coachSpeed = 10f;
    [SerializeField] private float cheerleaderSpeed = 25f;
    [SerializeField] private float playerSpeed = 20f;
    [SerializeField] private float mascotSpeed = 12f;

    [Header("Prefab Healths")]
    [SerializeField] private float fanHealth = 100;
    [SerializeField] private float coachHealth = 200;
    [SerializeField] private float cheerleaderHealth = 50;
    [SerializeField] private float playerHealth = 150;
    [SerializeField] private float mascotHealth = 300;

    [Header("Round Initialization")]
    [SerializeField] private BoxCollider[] spawnAreas;  // Where enemies will spawn
    [SerializeField] private PathNode[] startNodes;

    [Header("Round Configuration")]
    [SerializeField] private RoundObject currentRound;
    [SerializeField] private float coinMultiplier = 1;
    [SerializeField] private float dollarMultiplier = 1;

    [Header("Round Data")]
    [SerializeField] private int coins;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int numWaves = 0;
    [SerializeField] private int remainingEnemies;

    [Header("Allen Fieldhouse")]
    [SerializeField] private float fieldhouseHealth;
    [SerializeField] private HealthBar fieldhouseHealthBar; // Reference to UI HealthBar

    [Header("Sounds")]
    [SerializeField] private AudioClip allenFieldHouseDamageSoundEffect;
    [SerializeField] private AudioClip roundWonMusic;
    [SerializeField] private AudioClip roundLostMusic;

    [Header("UI Controllers")]
    [SerializeField] private LoadingBackgroundController loadingBackgroundController;

    [Header("Parents")]
    [SerializeField] private Transform placementParent;
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private Transform defensesParent;
    [SerializeField] private Transform projectilesParent;

    [Header("Game Data Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private GameData gameData;
    private RoundSceneCanvasController roundSceneUIController;
    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private List<Defense> defenses = new List<Defense>();

    private float maxFieldhouseHealth;
    private bool isAllEnemiesSlowed = false;
    private bool isAllEnemiesFrozen = false;
    private float slowMultiplier = 1;
    private bool gameOver = false;

    public Transform PlacementParent => placementParent;
    public Transform DefensesParent => defensesParent;
    public Transform ProjectilesParent => projectilesParent;
    public int Coins => coins;

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
        GameDataManager gameDataManager = GameDataManager.instance;

        gameData = gameDataManager.GameData;
        roundSceneUIController = RoundSceneCanvasController.instance;
        currentRound = gameDataManager.SelectedRound;

        numWaves = currentRound.Waves.Length;

        fieldhouseHealth = 0f;

        foreach (Wave wave in currentRound.Waves)
        {
            remainingEnemies += wave.Fans + wave.Cheerleaders + wave.Coaches + wave.Players + wave.Mascots;
            fieldhouseHealth += (wave.Fans * fanHealth + wave.Cheerleaders * cheerleaderHealth + wave.Coaches * coachHealth + wave.Players * playerHealth + wave.Mascots * mascotHealth) / 2;
        }

        maxFieldhouseHealth = fieldhouseHealth;

        GetBonuses();

        StartCoroutine(StartRound());
    }

    // Start round.
    IEnumerator StartRound()
    {
        roundSceneUIController.UpdateWaveUI(currentWave + 1, numWaves);

        if (currentRound.OpponentImage != null)
        {
            roundSceneUIController.SetOpponentImage(currentRound.OpponentImage);
        }

        yield return new WaitForSeconds(1f);

        while (currentWave < numWaves)
        {
            while (isAllEnemiesFrozen)
            {
                yield return null;
            }

            WeatherManager.instance.ResetWeatherForNewRound();
            WeatherManager.instance.TryActivateWeather();
            roundSceneUIController.UpdateWaveUI(currentWave + 1, numWaves);
            roundSceneUIController.ShowWavePopupPanel((currentWave + 1).ToString());
            yield return StartCoroutine(SpawnEnemies(currentRound.Waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(10f);
        }
    }

    public void EndRound()
    {
        StopAllCoroutines();
        SoundManager.instance.StopMusic();
        roundSceneUIController.CloseExistingUI();
        StartCoroutine(EndRoundCoroutine());
    }

    private IEnumerator EndRoundCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(.5f));
        Time.timeScale = 1f;
        SceneManager.LoadScene("Building Scene");
    }

    public void RetryRound()
    {
        StopAllCoroutines();
        SoundManager.instance.StopMusic();
        roundSceneUIController.CloseExistingUI();
        StartCoroutine(RetryRoundCoroutine());
    }

    private IEnumerator RetryRoundCoroutine()
    {
        yield return StartCoroutine(loadingBackgroundController.FadeInCoroutine(0.5f));
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        yield return SpawnEnemyType(fanPrefab, wave.Fans, wave.SpawnInterval, fanSpeed, fanHealth);
        yield return SpawnEnemyType(coachPrefab, wave.Coaches, wave.SpawnInterval, coachSpeed, coachHealth);
        yield return SpawnEnemyType(cheerleaderPrefab, wave.Cheerleaders, wave.SpawnInterval, cheerleaderSpeed, cheerleaderHealth);
        yield return SpawnEnemyType(playerPrefab, wave.Players, wave.SpawnInterval, playerSpeed, playerHealth);
        yield return SpawnEnemyType(mascotPrefab, wave.Mascots, wave.SpawnInterval, mascotSpeed, mascotHealth);
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
                enemyScript.SetReward(currentRound.KillPayout);

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

    void GetBonuses()
    {
        List<BonusObject> bonusObjects = gameDataObject.BonusObjects;
        // BonusObject bonusObject = gameData.Get

        // Find the highest unlocked coin bonus
        foreach (BonusObject bonusObject in bonusObjects)
        {
            BonusData bonusData = gameData.GetBonusData(bonusObject.ObjectName);

            if (bonusData != null) // Check if the bonus is unlocked
            {
                if (bonusObject.CoinBonus > coinMultiplier)
                {
                    coinMultiplier = bonusObject.CoinBonus;
                }

                if (bonusObject.DollarBonus > dollarMultiplier)
                {
                    dollarMultiplier = bonusObject.DollarBonus;
                }
            }
        }
    }

    // Add coins.
    public void AddCoins(int amount)
    {
        coins += (int)(amount * coinMultiplier);
        roundSceneUIController.UpdateCoinUI();
    }

    // Subtract coins.
    public void SubtractCoins(int amount)
    {
        coins = Mathf.Max(0, coins - amount);
        roundSceneUIController.UpdateCoinUI();
    }

    public void EnemyDefeated(EnemyMovement enemyDefeated)
    {
        if (gameOver) return;

        remainingEnemies--;
        enemies.Remove(enemyDefeated);

        // Check if the round is over (no remaining enemies)
        if (remainingEnemies <= 0)
        {
            Time.timeScale = 0f;
            PauseMenuCanvasController.instance.enabled = false;
            SoundManager.instance.PlayMusic(roundWonMusic, transform);

            int debt = gameData.GetDebt();
            int reward = (int)(currentRound.WinPayout * dollarMultiplier);

            if (debt > 0)
            {
                int owed = Mathf.Min((int)(reward * 0.3f), debt);  // 30% debt payment
                gameData.PayDebtOnAllLoans(owed);
                gameData.AddDollars(reward - owed);
                roundSceneUIController.ShowRoundWonPanel(
                    gameData.RoundNumber.ToString(),
                    reward.ToString(),
                    "- $" + owed.ToString(),
                    "$" + (reward - owed).ToString()
                );
            }
            else
            {
                gameData.AddDollars(reward);
                roundSceneUIController.ShowRoundWonPanel(
                    gameData.RoundNumber.ToString(),
                    reward.ToString(),
                    "",
                    ""
                );
            }

            if (gameData.RoundNumber == currentRound.RoundNumber && currentRound.RoundNumber < gameDataObject.RoundObjects.Capacity)
            {
                gameData.IncrementRoundNumber();  // unlock next round

                RoundObject roundObject = gameDataObject.GetRoundObject(gameData.RoundNumber);
                GameDataManager.instance.SetSelectedRound(roundObject);
            }
        }
    }

    public void DamageFieldhouse(float damage)
    {
        SoundManager.instance.PlaySoundEffect(allenFieldHouseDamageSoundEffect, transform);

        fieldhouseHealth -= damage;

        if (fieldhouseHealthBar != null)
        {
            fieldhouseHealthBar.UpdateHealthBar(fieldhouseHealth, maxFieldhouseHealth);
        }

        if (fieldhouseHealth <= 0 && !gameOver)
        {
            gameOver = true;
            Time.timeScale = 0f;
            PauseMenuCanvasController.instance.enabled = false;
            SoundManager.instance.PlayMusic(roundLostMusic, transform);
            roundSceneUIController.ShowRoundLostPanel();
        }
    }

    public void AddDefense(Defense defense)
    {
        defenses.Add(defense);
    }

    public void RemoveDefense(Defense defense)
    {
        defenses.Remove(defense);
    }

    public void RegenHealthOnDefenses()
    {
        foreach (var defense in defenses)
        {
            if (defense.gameObject.CompareTag("HealthDefense"))
            {
                defense.ResetHealth();
            }
        }

        int regenCost = GetRegenCost();

        SubtractCoins(regenCost);
    }

    public int GetRegenCost()
    {
        int regenCost = 0;

        foreach (var defense in defenses)
        {
            if (defense.gameObject.CompareTag("HealthDefense"))
            {
                regenCost += defense.DefenseObject.CoinCost / 2;
            }
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

