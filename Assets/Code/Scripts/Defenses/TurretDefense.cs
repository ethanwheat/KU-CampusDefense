using System.Collections.Generic;
using UnityEngine;

public class TurretDefense : Defense, IDefense
{
    [Header("Turret Prefabs")]
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab

    [Header("Turrent Game Objects")]
    [SerializeField] private Transform firePoint; // Point where projectiles are spawned

    [Header("Turret Sounds")]
    [SerializeField] private AudioClip shootSoundEffect;

    [Header("Turret Settings")]
    [SerializeField] private bool faceEnemy = true;
    [SerializeField] private float fireRate = 1f; // Shots per second

    private List<EnemyMovement> enemiesInRange = new List<EnemyMovement>();
    private float fireCountdown = 0f;

    public override void Update()
    {
        // Call update on base.
        base.Update();

        // Find the nearest enemy in range
        EnemyMovement nearestEnemy = GetNearestEnemy();

        if (nearestEnemy != null)
        {
            // Rotate turret head to face the enemy
            if (faceEnemy)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            }

            // Shoot at the enemy
            if (fireCountdown <= 0f)
            {
                Shoot(nearestEnemy);
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    private EnemyMovement GetNearestEnemy()
    {
        EnemyMovement nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (EnemyMovement enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }

    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            // Add enemy to the list when it enters the range
            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                if (!enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enabled)
        {
            // Remove enemy from the list when it leaves the range
            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                if (enemy != null && enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Remove(enemy);
                }
            }
        }
    }

    public void Shoot(EnemyMovement target)
    {
        if (target == null)
        {
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.transform.parent = RoundManager.instance.ProjectilesParent;
        if (projectile.TryGetComponent(out TrackedProjectile projectileScript))
        {
            projectileScript.SetTarget(target);
        }
        SoundManager.instance.PlaySoundEffect(shootSoundEffect, transform);
    }

    public virtual void ApplyEffect(EnemyMovement enemy)
    {
        // Not needed for turret, but required by IDefense
    }

    public virtual void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for turret, but required by IDefense
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}