using UnityEngine;

[CreateAssetMenu(fileName = "BonusDataObject", menuName = "Scriptable Objects/BonusDataObject")]
public class BonusObject : PurchasableObject
{
    [Header("Currency Bonus")]
    [SerializeField] private float dollarBonus;
    [SerializeField] private float coinBonus;

    public float DollarBonus => dollarBonus;
    public float CoinBonus => coinBonus;
}
