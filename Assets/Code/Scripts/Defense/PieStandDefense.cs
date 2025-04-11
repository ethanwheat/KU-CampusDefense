using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PieStandDefense : HealthDefense, IDefenseEffect
{
    [Header("Pie Stand Settings")]
    [SerializeField] private float fireRate = 0.6f;
    [SerializeField] private GameObject piePrefab;
    [SerializeField] private Transform firePoint;

    private float fireCountdown = 0f;
    private List<EnemyMovement> enemiesInRange = new List<EnemyMovement>();
    private float timeElapsed = 0f;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1f)
        {
            subtractHealth(1);
            timeElapsed = 0f;
        }

        if (getHealth() == 0)
        {
            Destroy(gameObject);
        }

        EnemyMovement nearestEnemy = GetNearestEnemy();

        if (nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

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
            if (other.TryGetComponent(out EnemyMovement enemy))
            {
                if (enemy != null && enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Remove(enemy);
                }
            }
        }
    }

    private void Shoot(EnemyMovement target)
    {
        if (target == null) return;

        GameObject pie = Instantiate(piePrefab, firePoint.position, firePoint.rotation);
        pie.transform.parent = getProjectilesParent();
        if (pie.TryGetComponent(out Pie pieScript))
        {
            pieScript.SetTarget(target);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        // Not needed for pie stand, but required by IDefenseEffect
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for pie stand, but required by IDefenseEffect
    }
}
