using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeDefense : Defense, IDefense
{
    [Header("Blockade Defense Settings")]
    [SerializeField] private float damageToDefense = 10f;
    [SerializeField] private float damageToEnemy = 10f;

    private List<EnemyMovement> enemies = new List<EnemyMovement>();

    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            SubtractHealth(damageToDefense * Time.deltaTime);

            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                enemy.TakeDamage(damageToEnemy * Time.deltaTime);
            }
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        enemies.Add(enemy);
        enemy.BlockMovement(true);
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        enemies.Remove(enemy);
        enemy.BlockMovement(false);
    }

    public override void OnDefenseDestroy()
    {
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.BlockMovement(false);
        }

        base.OnDefenseDestroy();
    }
}
