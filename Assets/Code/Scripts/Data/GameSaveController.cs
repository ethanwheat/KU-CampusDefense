using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameSaveController
{
    private static readonly string GameSaveFilePath = Path.Combine(Application.persistentDataPath, "GameSaves.json");

    public static GameData CreateGameSave(string name)
    {
        // Create unique id.
        string id = Guid.NewGuid().ToString();

        // Create new game save.
        GameSave gameSave = new GameSave(id, name);

        // Add game save to game save list.
        GameSaveList gameSaveList = GetGameSaveList();
        gameSaveList.Add(gameSave);

        // Write data to game saves file.
        File.WriteAllText(GameSaveFilePath, JsonUtility.ToJson(gameSaveList, true));

        // Return game save.
        return GameDataController.CreateGameData(id);
    }

    public static void DeleteGameSave(string id)
    {
        GameSaveList gameSaveList = GetGameSaveList();
        gameSaveList.GameSaves.RemoveAll(save => save.Id == id);
        File.WriteAllText(GameSaveFilePath, JsonUtility.ToJson(gameSaveList, true));
    }

    public static GameSaveList GetGameSaveList()
    {
        if (File.Exists(GameSaveFilePath))
        {
            string gameSavesJson = File.ReadAllText(GameSaveFilePath);
            return JsonUtility.FromJson<GameSaveList>(gameSavesJson);
        }

        return new GameSaveList();
    }
}