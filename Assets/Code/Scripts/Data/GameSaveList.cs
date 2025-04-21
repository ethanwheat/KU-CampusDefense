using System.Collections.Generic;

[System.Serializable]
public class GameSaveList
{
    public List<GameSave> gameSaves = new List<GameSave>();

    public List<GameSave> GameSaves => gameSaves;

    public void Add(GameSave gameSave)
    {
        gameSaves.Add(gameSave);
    }
}