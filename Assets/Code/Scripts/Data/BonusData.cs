using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/BonusData")]
public class BonusData : PurchasableData
{
    [Header("Currency Bonus")]
    [SerializeField] private float dollarBonus;
    [SerializeField] private float coinBonus;

    public float DollarBonus => dollarBonus;
    public float CoinBonus => coinBonus;
}
