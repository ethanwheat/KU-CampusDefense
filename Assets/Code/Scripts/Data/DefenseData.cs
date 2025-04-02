using UnityEngine;

[CreateAssetMenu(fileName = "DefenseData", menuName = "Scriptable Objects/DefenseData")]

public class DefenseData : PurchasableData
{
    [Header("Defense Information")]
    [SerializeField] private int coinCost;
    [SerializeField] private bool showInDefensePanel;
    [SerializeField] private int level1UpgradeDollarCost;
    [SerializeField] private int level2UpgradeDollarCost;
    [SerializeField] private int level3UpgradeDollarCost;

    [Header("Defense Data")]
    [SerializeField] private int level = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    // Get defense prefab.
    public GameObject getPrefab()
    {
        return prefab;
    }

    // Get defense coin cost.
    public int getCoinCost()
    {
        return coinCost;
    }

    // Get if shown in defense panel.
    public bool isShownInDefensePanel()
    {
        return showInDefensePanel;
    }

    // Get defense level.
    public int getLevel()
    {
        return level;
    }

    public int getUpgradeCost()
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

    public void upgradeLevel()
    {
        level++;
    }

    public void resetLevel()
    {
        level = 1;
    }
}
