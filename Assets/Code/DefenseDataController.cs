using UnityEngine;

[System.Serializable]
public class DefenseData
{
    public string name;
    public int id;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "DefenseDataController", menuName = "Scriptable Objects/DefenseDataController")]
public class DefenseDataController : ScriptableObject
{
    public DefenseData[] defenseData;

    public DefenseData[] getData()
    {
        return defenseData;
    }
}
