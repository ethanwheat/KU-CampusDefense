using System;
using UnityEngine;

public class BarrierDefense : MonoBehaviour, IDefenseEffect
{
    public float maxHealth = 100f;
    private float currentHealth;
    public float damagePerSecond = 10f;

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
                ApplyEffect(enemy);
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

        if (currentHealth <= 0)
        {
            DestroyBarrier();
        }
    }

    private void DestroyBarrier()
    {
        // Resume movement for all enemies within the barrier area
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyMovement enemy))
            {
                RemoveEffect(enemy);
            }
        }

        Destroy(gameObject);
    }
}
