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
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }

            SubtractHealth(damageToDefense * Time.deltaTime);
            enemy.TakeDamage(damageToEnemy * Time.deltaTime);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        enemy.BlockMovement(true);
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        enemy.BlockMovement(false);
    }

    public override void OnDefenseDestroy()
    {
        foreach (EnemyMovement enemy in enemies)
        {
            RemoveEffect(enemy);
        }

        base.OnDefenseDestroy();
    }
}
