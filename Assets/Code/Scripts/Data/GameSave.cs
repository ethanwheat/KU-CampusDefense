using System;

[System.Serializable]
public class GameSave
{
    public string id;
    public string name;
    public string creationTime;

    public string Id => id;
    public string Name => name;
    public string CreationTime => creationTime;

    public GameSave(string id, string name)
    {
        this.id = id;
        this.name = string.IsNullOrWhiteSpace(name) ? "Untitled Game Save" : name;
        creationTime = DateTime.UtcNow.ToString("o");
    }
}
