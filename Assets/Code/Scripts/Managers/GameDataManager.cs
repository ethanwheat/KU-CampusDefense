using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [Header("Game Data Initialization")]
    [SerializeField]
    private GameData startingGameData = new GameData()
    {
        RoundNumber = 1,
        Dollars = 100,
        DefenseData = { new DefenseData("Barrier") }
    };

    [Header("Game Data")]
    [SerializeField] private GameData gameData;

    public GameData GameData => gameData;

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
        // Create unique id.
        string id = Guid.NewGuid().ToString();

        // Create game data meta.
        GameDataMeta gameDataMeta = new GameDataMeta()
        {
            Id = id,
            Name = string.IsNullOrWhiteSpace(name) ? "Untitled Game Save" : name
        };

        // Set names of files.
        string gameDataFilePath = Path.Combine(Application.persistentDataPath, $"{id}.json");
        string gameDataMetaFilePath = Path.Combine(Application.persistentDataPath, $"{id}.meta.json");

        // Write data to game saves file.
        File.WriteAllText(gameDataFilePath, JsonUtility.ToJson(startingGameData, true));
        File.WriteAllText(gameDataMetaFilePath, JsonUtility.ToJson(gameDataMeta, true));

        // Set game data to be new game data.
        gameData = startingGameData;

        return true;
    }

    public bool LoadGameData(string id)
    {
        string targetFileName = id + ".json";

        foreach (string filePath in Directory.GetFiles(Application.persistentDataPath, "*.json"))
        {
            if (Path.GetFileName(filePath) == targetFileName)
            {
                string json = File.ReadAllText(filePath);
                gameData = JsonUtility.FromJson<GameData>(json);
                return true;
            }
        }

        return false;
    }

    public void DeleteGameData(string id)
    {
        // Get files.
        string gameDataFilePath = Path.Combine(Application.persistentDataPath, $"{id}.json");
        string gameDataMetaFilePath = Path.Combine(Application.persistentDataPath, $"{id}.meta.json");

        if (File.Exists(gameDataFilePath))
        {
            File.Delete(gameDataFilePath);
        }

        if (File.Exists(gameDataMetaFilePath))
        {
            File.Delete(gameDataMetaFilePath);
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
}