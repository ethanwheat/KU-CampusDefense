[System.Serializable]
public class GameDataMeta
{
    public string Guid;
    public string Name;
    public string CreationTime;
    public string LastModified;

    public void SetLastModified(string lastModified)
    {
        LastModified = lastModified;
    }
}
