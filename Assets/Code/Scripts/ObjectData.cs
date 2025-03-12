using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    [Header("Object Information")]
    [SerializeField] private string objectName;
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
