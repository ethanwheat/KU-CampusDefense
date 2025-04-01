using System.Collections.Generic;
using UnityEngine;

public class TurretDefense : HealthDefense, IDefenseEffect
{
    [Header("Turret Settings")]
    [SerializeField] private float fireRate = 1f; // Shots per second
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private Transform firePoint; // Point where bullets are spawned

    private float fireCountdown = 0f;
    private List<EnemyMovement> enemiesInRange = new List<EnemyMovement>();
    private float destroyTime;

    void Start()
    {
        destroyTime = Time.time + getHealth();
    }

    void Update()
    {
        float time = Time.time;

        setHealth(destroyTime - time);

        // Destroy if destroy time is less then current time.
        if (destroyTime <= time)
        {
            Destroy(gameObject);
        }

        // Find the nearest enemy in range
        EnemyMovement nearestEnemy = GetNearestEnemy();

        if (nearestEnemy != null)
        {
            // Rotate turret head to face the enemy
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

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
                    //Debug.Log("Enemy entered range: " + enemy.name);
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
                    //Debug.Log("Enemy left range: " + enemy.name);
                }
            }
        }
    }

    private void Shoot(EnemyMovement target)
    {
        if (target == null)
        {
            //Debug.Log("No target to shoot at.");
            return;
        }

        //Debug.Log("Shooting at target: " + target.name);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.parent = getProjectilesParent();
        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.SetTarget(target);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        // Not needed for turret, but required by IDefenseEffect
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for turret, but required by IDefenseEffect
    }
}