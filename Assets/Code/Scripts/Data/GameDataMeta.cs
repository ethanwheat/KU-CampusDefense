using System;

[System.Serializable]
public class GameDataMeta
{
    public string Id;
    public string Name;
    public string CreationTime;

    public GameDataMeta(string id, string name)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? "Untitled Game Save" : name;
        CreationTime = DateTime.UtcNow.ToString("o");
    }
}
