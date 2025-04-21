using System;
using Unity.VisualScripting;
using UnityEditor;

[System.Serializable]
public class PurchasableData
{
    public string ObjectName;
    public bool Bought;

    public PurchasableData(string name, bool bought)
    {
        ObjectName = name;
        Bought = bought;
    }

    // Set object to be bought or not.
    public void SetBought(bool boughtValue)
    {
        Bought = boughtValue;
    }
}