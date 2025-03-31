using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDefense : MonoBehaviour, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damagePerSecond = 10f;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    private float currentHealth;
    private List<EnemyMovement> enemies = new List<EnemyMovement>();

    private void Start()
    {
        currentHealth = maxHealth;
    }

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
        currentHealth -= damage;

        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            DestroyBarrier();
        }
    }

    private void DestroyBarrier()
    {
        // Resume movement for all enemies within the barrier area
        foreach (EnemyMovement enemy in enemies)
        {
            RemoveEffect(enemy);
        }

        Destroy(gameObject);
    }
}
