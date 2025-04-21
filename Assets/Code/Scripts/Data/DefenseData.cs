[System.Serializable]
public class DefenseData : PurchasableData
{
    public int level;

    public int Level => level;

    public DefenseData(string name, int startingLevel = 1) : base(name)
    {
        level = startingLevel;
    }
}