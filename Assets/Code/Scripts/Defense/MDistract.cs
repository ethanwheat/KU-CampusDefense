using System.Collections.Generic;
using UnityEngine;

public class MascotDefense : MonoBehaviour, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float maxHealth = 75f;
    [SerializeField] private float damageToEnemyPerSecond = 10f;
    [SerializeField] private float damageToSelfPerSecond = 15f;

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
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
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
