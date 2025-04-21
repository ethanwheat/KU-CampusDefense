[System.Serializable]
public class BonusData : PurchasableData
{
    public float CoinBonus;
    public float DollarBonus;

    public BonusData(string name, bool bought) : base(name, bought) { }
}