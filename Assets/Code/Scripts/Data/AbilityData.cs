using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Ability Information")]
    [SerializeField] private string abilityName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float effectDuration;

    public string AbilityName => abilityName;
    public string Description => description;
    public Sprite Icon => icon;
    public float CooldownTime => cooldownTime;
    public float EffectDuration => effectDuration;
}