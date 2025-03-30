using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityController : MonoBehaviour
{
    [SerializeField] protected AbilityData abilityData;
    
    private bool isOnCooldown = false;
    private float currentCooldown = 0f;
    private float currentEffectTime = 0f;
    private bool isActive = false;
    
    public UnityEvent OnAbilityActivated;
    public UnityEvent OnAbilityEnded;
    public UnityEvent OnCooldownStarted;
    public UnityEvent OnCooldownEnded;
    
    public AbilityData Data => abilityData;
    public bool IsOnCooldown => isOnCooldown;
    public float CurrentCooldown => currentCooldown;
    public bool IsActive => isActive;
    
    private void Update()
    {
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                EndCooldown();
            }
        }
        
        if (isActive)
        {
            currentEffectTime -= Time.deltaTime;
            if (currentEffectTime <= 0f)
            {
                EndAbilityEffect();
            }
        }
    }
    
    public void ActivateAbility()
    {
        if (isOnCooldown) return;
        
        StartAbilityEffect();
        StartCooldown();
        OnAbilityActivated.Invoke();
    }
    
    protected virtual void StartAbilityEffect()
    {
        isActive = true;
        currentEffectTime = abilityData.EffectDuration;
        // Specific effect logic will be in child classes
    }
    
    protected virtual void EndAbilityEffect()
    {
        isActive = false;
        OnAbilityEnded.Invoke();
        // Specific cleanup logic will be in child classes
    }
    
    private void StartCooldown()
    {
        isOnCooldown = true;
        currentCooldown = abilityData.CooldownTime;
        OnCooldownStarted.Invoke();
    }
    
    private void EndCooldown()
    {
        isOnCooldown = false;
        OnCooldownEnded.Invoke();
    }
}