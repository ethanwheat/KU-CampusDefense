using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableData", menuName = "Scriptable Objects/PurchasableData")]
public class PurchasableData : ScriptableObject
{
    [Header("Object Information")]
    [SerializeField] private string objectName;
    [SerializeField] private string description;
    [SerializeField] private int dollarCost;
    [SerializeField] private int unlockRound;
    [SerializeField] private bool boughtAtStart;

    [Header("Object Data")]
    [SerializeField] private bool bought;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataController gameDataController;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    // Get the objects name.
    public string getName()
    {
        return objectName;
    }

    // Get the objects description.
    public string getDescription()
    {
        return description;
    }

    // Get objects dollar cost.
    public int getDollarCost()
    {
        return dollarCost;
    }

    // Get objects unlock round.
    public bool isLocked()
    {
        return unlockRound > gameDataController.getRoundNumber();
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

    // Get sprite.
    public Sprite getSprite()
    {
        return sprite;
    }
}
