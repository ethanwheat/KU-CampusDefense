[System.Serializable]
public class DefenseData : PurchasableData
{
    public int Level;

    public DefenseData(string name, bool bought, int startingLevel = 1) : base(name, bought)
    {
        Level = startingLevel;
    }
}