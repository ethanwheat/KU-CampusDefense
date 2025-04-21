using System;
using UnityEditor;

[System.Serializable]
public class PurchasableData
{
    public string objectName;

    public string ObjectName => objectName;

    public PurchasableData(string name)
    {
        objectName = name;
    }
}