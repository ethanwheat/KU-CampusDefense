using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableObject", menuName = "Scriptable Objects/PurchasableObject")]
public class PurchasableObject : ScriptableObject
{
    [Header("Object Information")]
    [SerializeField] private string objectName;
    [SerializeField] private string description;
    [SerializeField] private int dollarCost;
    [SerializeField] private int unlockRound;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite;

    public string ObjectName => objectName;
    public string Description => description;
    public int DollarCost => dollarCost;
    public int UnlockRound => unlockRound;
    public Sprite Sprite => sprite;
}
