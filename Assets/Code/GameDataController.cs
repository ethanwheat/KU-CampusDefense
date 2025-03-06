using UnityEngine;

public class ObjectData
{
    [Header("Object Information")]
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int id;
    [SerializeField] private int unlockRound;
    [SerializeField] private bool bought;

    [Header("Building Costs")]
    [SerializeField] private int buildingCurrencyCost;

    public string getName()
    {
        return name;
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

    public int getBuildingCurrencyCost()
    {
        return buildingCurrencyCost;
    }
}

[System.Serializable]
public class DefenseData : ObjectData
{
    [Header("Round Costs")]
    [SerializeField] private int roundCurrencyCost;

    [Header("Level")]
    [SerializeField] private int level;

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    public GameObject getPrefab()
    {
        return prefab;
    }

    public int getRoundCurrencyCost()
    {
        return roundCurrencyCost;
    }
}

[System.Serializable]
public class DormData : ObjectData
{
    [Header("Currency Bonus")]
    [SerializeField] private float buildingCurrencyBonus;

    public float getBuildingCurrencyBonus()
    {
        return buildingCurrencyBonus;
    }
}

[CreateAssetMenu(fileName = "GameDataController", menuName = "Scriptable Objects/GameDataController")]
public class GameDataController : ScriptableObject
{
    [Header("Game Information")]
    [SerializeField] private int roundNumber;

    [Header("Game Data")]
    [SerializeField] private DefenseData[] defenseData;
    [SerializeField] private DormData[] dormData;

    public int getRoundNumber()
    {
        return roundNumber;
    }

    public DefenseData[] getDefenseData()
    {
        return defenseData;
    }

    public DormData[] getDormData()
    {
        return dormData;
    }
}
