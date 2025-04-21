using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableDataObject", menuName = "Scriptable Objects/PurchasableDataObject")]
public class PurchasableDataObject : ScriptableObject
{
    [Header("Object Information")]
    [SerializeField] private string objectName;
    [SerializeField] private string description;
    [SerializeField] private int dollarCost;
    [SerializeField] private int unlockRound;

    [Header("Object Data")]
    [SerializeField] private bool bought;

    [Header("Game Data Controller")]
    [SerializeField] private GameDataObject gameDataController;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    public string ObjectName => objectName;
    public string Description => description;
    public int DollarCost => dollarCost;
    public bool Locked => unlockRound > gameDataController.RoundNumber;
    public bool Bought => bought;
    public Sprite Sprite => sprite;

    // Set object to be bought or not.
    public void SetBought(bool boughtValue)
    {
        bought = boughtValue;
    }
}
