using System.Collections.Generic;
using UnityEngine;

public class SlowAllEnemiesAbility : AbilityController
{
    [SerializeField] private float speedMultiplier = 0.5f; // 50% speed (maybe more?)
    
    private List<EnemyMovement> activeEnemies = new List<EnemyMovement>();
    
    protected override void StartAbilityEffect()
    {
        base.StartAbilityEffect();
        
        // Find all active enemies
        activeEnemies.Clear();
        var enemies = FindObjectsOfType<EnemyMovement>();
        foreach (var enemy in enemies)
        {
            activeEnemies.Add(enemy);
            enemy.SetSpeedMultiplier(speedMultiplier);
        }
    }
    
    protected override void EndAbilityEffect()
    {
        // Reset all affected enemies
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null) // Check if enemy still exists
            {
                enemy.ResetSpeedMultiplier();
            }
        }
        activeEnemies.Clear();
        
        base.EndAbilityEffect();
    }
}