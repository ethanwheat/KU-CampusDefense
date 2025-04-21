[System.Serializable]
public class BonusData : PurchasableData
{
    public float CoinBonus;
    public float DollarBonus;

    public BonusData(string name) : base(name) { }
}