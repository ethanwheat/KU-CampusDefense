using System;

[System.Serializable]
public class GameDataMeta
{
    public string Id;
    public string Name;
    public string CreationTime;

    public GameDataMeta()
    {
        CreationTime = DateTime.UtcNow.ToString("o");
    }
}
