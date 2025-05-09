using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDataObject", menuName = "Scriptable Objects/AbilityDataObject")]
public class AbilityObject : ScriptableObject
{
    [Header("Ability Information")]
    [SerializeField] private string abilityName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int dollarCost;
    [SerializeField] private int unlockRound;
    [SerializeField] private float cooldownDuration;
    [SerializeField] private float effectDuration;

    [Header("Ability Type")]
    [SerializeField] private AbilityType type;

    [Header("Ability Settings")]
    [SerializeField] private float speed = 50f;

    [Header("Slow Parameters")]
    [Range(0.1f, 0.9f)]
    [SerializeField] private float slowMultiplier = 0.5f;

    public enum AbilityType { SlowAll, FreezeAll, BigJayRampage, BusRide }

    // Public properties
    public string AbilityName => abilityName;
    public Sprite Icon => icon;
    public int DollarCost => dollarCost;
    public int UnlockRound => unlockRound;
    public float CooldownDuration => cooldownDuration;
    public float EffectDuration => effectDuration;
    public AbilityType Type => type;
    public float SlowMultiplier => type == AbilityType.SlowAll ? slowMultiplier : 1f;
    public float Speed => speed;
}