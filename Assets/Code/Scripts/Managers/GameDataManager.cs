using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [Header("Game Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private GameDataMeta gameDataMeta;

    [Header("Selected Round")]
    [SerializeField] private RoundObject selectedRound;

    [Header("Game Data Object")]
    [SerializeField] private GameDataObject gameDataObject;

    private Coroutine autosaveCoroutine;

    public GameData GameData => gameData;
    public GameDataMeta GameDataMeta => gameDataMeta;
    public RoundObject SelectedRound => selectedRound;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CreateGameData(string name)
    {
        try
        {
            // Create game data.
            gameData = new GameData()
            {
                RoundNumber = 1,
                Dollars = 0,
                DefenseData = { new DefenseData("Mascot Distraction") }
            };

            // Set selected round.
            selectedRound = gameDataObject.GetRoundObject(gameData.RoundNumber);

            // Create unique id.
            string guid = Guid.NewGuid().ToString();

            // Get current time.
            string currentTime = DateTime.UtcNow.ToString("o");

            // Create game data meta.
            gameDataMeta = new GameDataMeta()
            {
                Guid = guid,
                Name = string.IsNullOrWhiteSpace(name) ? "Untitled Game Save" : name,
                CreationTime = currentTime,
                LastModified = currentTime
            };

            SaveGameData();

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not create game data: " + exception);
            return false;
        }
    }

    public bool LoadGameData(string guid)
    {
        try
        {
            // Get game data file and game data meta file.
            string gameDataFilePath = Path.Combine(Application.persistentDataPath, $"{guid}.json");
            string gameDataMetaFilePath = Path.Combine(Application.persistentDataPath, $"{guid}.meta.json");

            // Get game data json and game data meta json.
            string gameDataJson = File.ReadAllText(gameDataFilePath);
            string gameDataMetaJson = File.ReadAllText(gameDataMetaFilePath);

            // Set game data and game data meta.
            gameData = JsonUtility.FromJson<GameData>(gameDataJson);
            gameDataMeta = JsonUtility.FromJson<GameDataMeta>(gameDataMetaJson);

            // Reset selected round.
            selectedRound = gameDataObject.GetRoundObject(gameData.RoundNumber);

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not load game data: " + exception);
            return false;
        }
    }

    public void TriggerAutosave()
    {
        if (autosaveCoroutine != null)
        {
            StopCoroutine(autosaveCoroutine);
        }

        autosaveCoroutine = StartCoroutine(TriggerAutosaveCoroutine());
    }

    private IEnumerator TriggerAutosaveCoroutine()
    {
        yield return new WaitForSeconds(1f);

        SaveGameData();
        autosaveCoroutine = null;
    }

    public bool SaveGameData()
    {
        try
        {
            // Update last modified to current time.
            gameDataMeta.SetLastModified(DateTime.UtcNow.ToString("o"));

            // Get game data file and game data meta file.
            string gameDataFilePath = Path.Combine(Application.persistentDataPath, $"{gameDataMeta.Guid}.json");
            string gameDataMetaFilePath = Path.Combine(Application.persistentDataPath, $"{gameDataMeta.Guid}.meta.json");

            // Write data to game data file and game data meta file.
            File.WriteAllText(gameDataFilePath, JsonUtility.ToJson(gameData, true));
            File.WriteAllText(gameDataMetaFilePath, JsonUtility.ToJson(gameDataMeta, true));

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not save game data: " + exception);
            return false;
        }
    }

    public bool DeleteGameData(string guid)
    {
        try
        {
            // Get files.
            string gameDataFilePath = Path.Combine(Application.persistentDataPath, $"{guid}.json");
            string gameDataMetaFilePath = Path.Combine(Application.persistentDataPath, $"{guid}.meta.json");

            // Delete game data file.
            if (File.Exists(gameDataFilePath))
            {
                File.Delete(gameDataFilePath);
            }

            // Delete game data meta file.
            if (File.Exists(gameDataMetaFilePath))
            {
                File.Delete(gameDataMetaFilePath);
            }

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not delete game data: " + exception);
            return false;
        }
    }

    public List<GameDataMeta> GetAllGameMetaData()
    {
        List<GameDataMeta> gameDataMeta = new List<GameDataMeta>();

        foreach (string filePath in Directory.GetFiles(Application.persistentDataPath, "*.meta.json"))
        {
            string json = File.ReadAllText(filePath);
            gameDataMeta.Add(JsonUtility.FromJson<GameDataMeta>(json));
        }

        return gameDataMeta;
    }

    public void SetSelectedRound(RoundObject roundObject)
    {
        selectedRound = roundObject;
    }
}