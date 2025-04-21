[System.Serializable]
public class DefenseData : PurchasableData
{
    public int Level;

    public DefenseData(string name, int startingLevel = 1) : base(name)
    {
        Level = startingLevel;
    }

    public void UpgradeLevel()
    {
        Level++;
    }

    public void SetLevel(int newLevel)
    {
        Level = newLevel;
    }

    public void ResetLevel()
    {
        Level = 1;
    }
}