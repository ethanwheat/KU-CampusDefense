using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] // optional but helpful
public class LibrarianDefense : HealthDefense, IDefenseEffect
{
    [Header("Librarian Settings")]
    [SerializeField] private float fireRate = 0.7f; // Shots per second
    [SerializeField] private GameObject bookPrefab; // Reference to the book prefab
    [SerializeField] private Transform firePoint; // Point where books are spawned

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

        // Destroy if health is 0.
        if (getHealth() == 0)
        {
            Destroy(gameObject);
        }

        // Find the nearest enemy in range
        EnemyMovement nearestEnemy = GetNearestEnemy();

        if (nearestEnemy != null)
        {
            // Rotate librarian head to face the enemy
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
        GameObject book = Instantiate(bookPrefab, firePoint.position, firePoint.rotation);
        book.transform.parent = getProjectilesParent();
        if (book.TryGetComponent(out Book bookScript))
        {
            bookScript.SetTarget(target);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        // Not needed for librarian, but required by IDefenseEffect
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for librarian, but required by IDefenseEffect
    }
}