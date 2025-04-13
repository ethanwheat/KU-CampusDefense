using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }
    
    private List<EnemyMovement> activeEnemies = new List<EnemyMovement>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(EnemyMovement enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyMovement enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    private IEnumerator ApplySlowEffect(AbilityData ability)
    {
        float multiplier = ability.SlowMultiplier; // This will be 1f for non-SlowAll Ability types
        
        foreach (var enemy in activeEnemies)
        {
            enemy?.SetSpeedMultiplier(multiplier);
        }
        
        yield return new WaitForSeconds(ability.EffectDuration);
        
        foreach (var enemy in activeEnemies)
        {
            enemy?.ResetSpeedMultiplier();
        }
    }

    private IEnumerator ApplyFreezeEffect(float duration)
    {
        // Freeze enemies
        foreach (var enemy in activeEnemies)
        {
            enemy?.BlockMovement(true);
        }
        
        yield return new WaitForSeconds(duration);
        
        // Unfreeze enemies
        foreach (var enemy in activeEnemies)
        {
            enemy?.BlockMovement(false);
        }
    }

    public void ActivateAbility(AbilityData ability)
    {
        Debug.Log($"Activating ability: {ability.AbilityName}");
        
        switch (ability.Type)
        {
            case AbilityData.AbilityType.SlowAll:
                //StartCoroutine(ApplySlowEffect(ability.EffectDuration, ability.SlowMultiplier));
                StartCoroutine(ApplySlowEffect(ability)); 
                break;
                
            case AbilityData.AbilityType.FreezeAll:
                StartCoroutine(ApplyFreezeEffect(ability.EffectDuration));
                break;
        }
    }
}