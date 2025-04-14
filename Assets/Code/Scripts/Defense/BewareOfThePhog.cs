using System.Collections.Generic;
using UnityEngine;

public class BewareOfThePhog : Defense, IDefense
{
    [Header("Phog Settings")]
    [SerializeField] private float damagePerSecond = 3f;
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private GameObject fogPrefab;
    [SerializeField] private Transform fogSpawnPoint;

    private GameObject spawnedFog;
    private List<EnemyMovement> affectedEnemies = new List<EnemyMovement>();

    private void Start()
    {
        if (fogPrefab != null && fogSpawnPoint != null)
        {
            spawnedFog = Instantiate(fogPrefab, fogSpawnPoint.position, Quaternion.identity);
            spawnedFog.transform.SetParent(transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enabled && other.TryGetComponent(out EnemyMovement enemy))
        {
            if (!affectedEnemies.Contains(enemy))
            {
                ApplyEffect(enemy);
                affectedEnemies.Add(enemy);
            }

            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enabled && other.TryGetComponent(out EnemyMovement enemy) && affectedEnemies.Contains(enemy))
        {
            RemoveEffect(enemy);
            affectedEnemies.Remove(enemy);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        enemy.SetSpeedMultiplier(slowMultiplier);
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        enemy.ResetSpeedMultiplier();
    }

    private void Update()
    {
        if (GetHealth() <= 0)
        {
            CleanupFog();
        }
    }

    private void CleanupFog()
    {
        foreach (var enemy in affectedEnemies)
        {
            RemoveEffect(enemy);
        }

        if (spawnedFog != null)
        {
            Destroy(spawnedFog);
        }

        Destroy(gameObject);
    }
}
