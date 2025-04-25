using UnityEngine;

[CreateAssetMenu(fileName = "DefenseObject", menuName = "Scriptable Objects/DefenseObject")]

public class DefenseObject : PurchasableObject
{
    [Header("Defense Information")]
    [SerializeField] private bool isShownInDefensePanel;
    [SerializeField] private int coinCost;
    [SerializeField] private int level1UpgradeDollarCost;
    [SerializeField] private int level2UpgradeDollarCost;
    [SerializeField] private int level3UpgradeDollarCost;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    public bool IsShownInDefensePanel => isShownInDefensePanel;
    public int CoinCost => coinCost;
    public GameObject Prefab => prefab;

    public int GetUpgradeCost(int level)
    {
        if (level == 1)
        {
            return level1UpgradeDollarCost;
        }
        else if (level == 2)
        {
            return level2UpgradeDollarCost;
        }
        else
        {
            return level3UpgradeDollarCost;
        }
    }
}
