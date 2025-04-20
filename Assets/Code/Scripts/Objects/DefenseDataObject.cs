using UnityEngine;

[CreateAssetMenu(fileName = "DefenseDataObject", menuName = "Scriptable Objects/DefenseDataObject")]

public class DefenseDataObject : PurchasableDataObject
{
    [Header("Defense Information")]
    [SerializeField] private bool isShownInDefensePanel;
    [SerializeField] private int coinCost;
    [SerializeField] private int level1UpgradeDollarCost;
    [SerializeField] private int level2UpgradeDollarCost;
    [SerializeField] private int level3UpgradeDollarCost;

    [Header("Defense Data")]
    [SerializeField] private int level = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    public bool IsShownInDefensePanel => isShownInDefensePanel;
    public int CoinCost => coinCost;
    public int Level => level;
    public GameObject Prefab => prefab;

    public int GetUpgradeCost()
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

    public void UpgradeLevel()
    {
        level++;
    }

    public void ResetLevel()
    {
        level = 1;
    }
}
