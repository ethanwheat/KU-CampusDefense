/*using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemiesAbility : AbilityController
{
    private List<EnemyMovement> frozenEnemies = new List<EnemyMovement>();
    
    protected override void StartAbilityEffect()
    {
        base.StartAbilityEffect();
        
        frozenEnemies.Clear();
        var enemies = FindObjectsOfType<EnemyMovement>();
        foreach (var enemy in enemies)
        {
            frozenEnemies.Add(enemy);
            enemy.BlockMovement(true);
        }
    }
    
    protected override void EndAbilityEffect()
    {
        foreach (var enemy in frozenEnemies)
        {
            if (enemy != null)
            {
                enemy.BlockMovement(false);
            }
        }
        frozenEnemies.Clear();
        
        base.EndAbilityEffect();
    }
}*/