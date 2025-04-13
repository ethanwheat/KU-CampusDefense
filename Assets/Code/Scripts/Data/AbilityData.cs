using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Ability Information")]
    [SerializeField] private string abilityName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int unlockRound;
    [SerializeField] private float cooldownDuration;
    [SerializeField] private float effectDuration;
    
    [Header("Ability Type")]
    [SerializeField] private AbilityType type;
    
    [Header("Slow Parameters")]
    [Range(0.1f, 0.9f)]
    [SerializeField] private float slowMultiplier = 0.5f;
    
    public enum AbilityType { SlowAll, FreezeAll, Other }
    
    // Public properties
    public string AbilityName => abilityName;
    public Sprite Icon => icon;
    public int UnlockRound => unlockRound;
    public float CooldownDuration => cooldownDuration;
    public float EffectDuration => effectDuration;
    public AbilityType Type => type;
    public float SlowMultiplier => type == AbilityType.SlowAll ? slowMultiplier : 1f;
}