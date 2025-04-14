using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Scriptable Objects/BonusData")]
public class BonusData : PurchasableData
{
    [Header("Currency Bonus")]
    [SerializeField] private float dollarBonus;
    [SerializeField] private float coinBonus;

    // Get dollar bonus.
    public float getDollarBonus()
    {
        return dollarBonus;
    }

    // Get coin bonus.
    public float getCoinBonus()
    {
        return coinBonus;
    }
}
