using UnityEngine;

public class ObjectData
{
    [Header("Object Information")]
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int id;
    [SerializeField] private int unlockRound;
    [SerializeField] private bool bought;

    [Header("Dollar Cost")]
    [SerializeField] private int dollarCost;

    public string getName()
    {
        return name;
    }

    public string getDescription()
    {
        return description;
    }

    public int getId()
    {
        return id;
    }

    public int getUnlockRound()
    {
        return unlockRound;
    }

    public bool isBought()
    {
        return bought;
    }

    public int getDollarCost()
    {
        return dollarCost;
    }
}

[System.Serializable]
public class DefenseData : ObjectData
{
    [Header("Coin Cost")]
    [SerializeField] private int coinCost;

    [Header("Level")]
    [SerializeField] private int level;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    public GameObject getPrefab()
    {
        return prefab;
    }

    public int getCoinCost()
    {
        return coinCost;
    }
}

[System.Serializable]
public class BonusData : ObjectData
{
    [Header("Currency Bonus")]
    [SerializeField] private float dollarBonus;
    [SerializeField] private float coinBonus;

    public float getDollarBonus()
    {
        return dollarBonus;
    }

    public float getCoinBonus()
    {
        return coinBonus;
    }
}

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Information")]
    [SerializeField] private int roundNumber;

    [Header("Game Data")]
    [SerializeField] private int dollars;
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private BonusData[] bonusData;

    public int getRoundNumber()
    {
        return roundNumber;
    }

    public int getDollars()
    {
        return dollars;
    }

    public DefenseData[] getDefenseData()
    {
        return defenseData;
    }

    public DefenseData getDefenseObjectData(int id)
    {
        foreach (var data in defenseData)
        {
            int dataId = data.getId();

            if (dataId == id)
            {
                return data;
            }
        }

        return null;
    }

    public BonusData getBonusObjectData(int id)
    {
        foreach (var data in bonusData)
        {
            int dataId = data.getId();

            if (dataId == id)
            {
                return data;
            }
        }

        return null;
    }
}
