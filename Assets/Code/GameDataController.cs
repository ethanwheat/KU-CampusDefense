using UnityEngine;

public class ObjectData
{
    [Header("Object Information")]
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private int dollarCost;
    [SerializeField] private int unlockRound;
    [SerializeField] private bool boughtAtStart;

    [Header("Object Data")]
    [SerializeField] private bool bought;

    // Get the objects name.
    public string getName()
    {
        return name;
    }

    // Get the objects id.
    public int getId()
    {
        return id;
    }

    // Get the objects description.
    public string getDescription()
    {
        return description;
    }

    // Get objects unlock round.
    public int getUnlockRound()
    {
        return unlockRound;
    }

    // Get objects dollar cost.
    public int getDollarCost()
    {
        return dollarCost;
    }

    // Get if the object is bought.
    public bool isBought()
    {
        return bought;
    }

    // Get if the object is bought at start.
    public bool isBoughtAtStart()
    {
        return boughtAtStart;
    }

    // Set object to be bought or not.
    public void setBought(bool boughtValue)
    {
        bought = boughtValue;
    }
}

[System.Serializable]
public class DefenseData : ObjectData
{
    [Header("Defense Information")]
    [SerializeField] private int coinCost;

    [Header("Defense Data")]
    [SerializeField] private int level;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    // Get defense sprite.
    public Sprite getSprite()
    {
        return sprite;
    }

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
}

[System.Serializable]
public class BonusData : ObjectData
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

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Initialization")]
    [SerializeField] private int startingDollarAmount;

    [Header("Game Data")]
    [SerializeField] private int roundNumber;
    [SerializeField] private int dollars;
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private BonusData[] bonusData;

    // Get round number.
    public int getRoundNumber()
    {
        return roundNumber;
    }

    // Increment round number.
    public void incrementRoundNumber()
    {
        roundNumber += 1;
    }

    // Get dollar amount.
    public int getDollarAmount()
    {
        return dollars;
    }

    // Add dollars.
    public void addDollars(int amount)
    {
        dollars += amount;
    }

    // Subtract dollars.
    public void subtractDollars(int amount)
    {
        int futureAmount = dollars - amount;

        if (futureAmount >= 0)
        {
            dollars = futureAmount;
        }
    }

    // Get defense data.
    public DefenseData[] getDefenseData()
    {
        return defenseData;
    }

    // Get bonus data.
    public BonusData[] getBonusData()
    {
        return bonusData;
    }

    // Get defense data by id.
    public DefenseData getDefenseDataById(int id)
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

    // Get bonus data by id.
    public BonusData getBonusObjectDataById(int id)
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

    // Reset game data.
    public void resetGameData()
    {
        // Reset dollars to starting dollar amount.
        dollars = startingDollarAmount;

        foreach (var data in defenseData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
        }

        foreach (var data in bonusData)
        {
            bool isBoughtAtStart = data.isBoughtAtStart();
            data.setBought(isBoughtAtStart);
        }
    }
}
