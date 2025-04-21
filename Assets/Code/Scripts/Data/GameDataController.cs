using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameDataController
{
    private static readonly string GameDataDirectory = Path.Combine(Application.persistentDataPath, "GameData");

    public static GameData CreateGameData(string id)
    {
        // Ensure GameData directory exists.
        if (!Directory.Exists(GameDataDirectory))
            Directory.CreateDirectory(GameDataDirectory);

        // Get game data file path.
        string gameDataFilePath = Path.Combine(GameDataDirectory, $"{id}.json");

        // Create new game data.
        GameData gameData = new GameData();

        // Write game data file.
        File.WriteAllText(gameDataFilePath, JsonUtility.ToJson(gameData, true));

        // Return game data.
        return gameData;
    }

    public static GameData GetGameData(string id)
    {
        if (Directory.Exists(GameDataDirectory))
        {
            string targetFileName = id + ".json";

            foreach (string filePath in Directory.GetFiles(GameDataDirectory, "*.json"))
            {
                if (Path.GetFileName(filePath) == targetFileName)
                {
                    string json = File.ReadAllText(filePath);
                    return JsonUtility.FromJson<GameData>(json);
                }
            }
        }

        return null;
    }
}
