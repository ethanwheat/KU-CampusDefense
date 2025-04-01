using System.Collections.Generic;
using UnityEngine;

public class MascotDistractionDefense : HealthDefense, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float damageToEnemyPerSecond = 10f;
    [SerializeField] private float damageToSelfPerSecond = 15f;

    private List<EnemyMovement> enemies = new List<EnemyMovement>();

    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            if (!enemies.Contains(enemy))
            {
                ApplyEffect(enemy);
                enemies.Add(enemy);
            }

            enemy.TakeDamage(damageToEnemyPerSecond * Time.deltaTime); // hurt enemy
            TakeDamage(damageToSelfPerSecond * Time.deltaTime); // mascot takes damage
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            RemoveEffect(enemy); // unfreeze enemy
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

    private void TakeDamage(float damage)
    {
        subtractHealth(damage);

        if (getHealth() <= 0)
        {
            DestroyMascot();
        }
    }

    private void DestroyMascot()
    {
        // Free any enemies still affected
        foreach (EnemyMovement enemy in enemies)
        {
            RemoveEffect(enemy);
        }

        Destroy(gameObject);
    }
}
