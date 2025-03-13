using UnityEngine;

[CreateAssetMenu(fileName = "DefenseData", menuName = "Scriptable Objects/DefenseData")]
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
