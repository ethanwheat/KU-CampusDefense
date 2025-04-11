using System;
using System.Collections.Generic;
using UnityEngine;

public class CampusSecurityStop : HealthDefense, IDefenseEffect
{
    [Header("Campus Security Stop Settings")]
    [SerializeField] private float damagePerSecond = 10f;

    private List<EnemyMovement> enemies = new List<EnemyMovement>();

    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                if (!enemies.Contains(enemy))
                {
                    ApplyEffect(enemy);
                    enemies.Add(enemy);
                }

                TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enabled)
        {
            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                RemoveEffect(enemy);
                enemies.Remove(enemy);
            }
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
            DestroyDefense();
        }
    }

    private void DestroyDefense()
    {
        // Resume movement for all enemies within the area
        foreach (EnemyMovement enemy in enemies)
        {
            RemoveEffect(enemy);
        }

        Destroy(gameObject);
    }
}
