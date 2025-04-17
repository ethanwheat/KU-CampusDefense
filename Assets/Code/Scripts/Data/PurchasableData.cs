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

    public string ObjectName => objectName;
    public string Description => description;
    public int DollarCost => dollarCost;
    public bool Locked => unlockRound > gameDataController.RoundNumber;
    public bool Bought => bought;
    public bool BoughtAtStart => boughtAtStart;
    public Sprite Sprite => sprite;

    // Set object to be bought or not.
    public void SetBought(bool boughtValue)
    {
        bought = boughtValue;
    }
}
