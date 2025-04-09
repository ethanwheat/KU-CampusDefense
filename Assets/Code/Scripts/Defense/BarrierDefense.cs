using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDefense : HealthDefense, IDefenseEffect
{
    [Header("Barrier Defense Settings")]
    [SerializeField] private float damagePerSecond = 10f;

    [Header("Sounds")]
    [SerializeField] private AudioClip stopSoundEffect;
    [SerializeField] private AudioClip destroySoundEffect;

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
                    SoundManager.instance.playSoundEffect(stopSoundEffect, transform, .25f);
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

        SoundManager.instance.playSoundEffect(destroySoundEffect, transform, .25f);
        Destroy(gameObject);
    }
}
