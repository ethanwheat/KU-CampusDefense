using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceDefense : MonoBehaviour, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float damagePerSecond = 5f;
    [SerializeField] private float duration = 30f;

    private float destroyTime;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    private Dictionary<EnemyMovement, Coroutine> activeCoroutines = new Dictionary<EnemyMovement, Coroutine>();

    private void Start()
    {
        // Automatically destroy the ice after 'duration' seconds
        StartCoroutine(DestroyAfterTime());

        destroyTime = Time.time + duration;
    }

    private void Update()
    {
        healthBar.UpdateHealthBar(destroyTime - Time.time, duration);
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        enemy.SetSpeedMultiplier(slowMultiplier);

        // Start a new coroutine if this enemy isn’t already tracked
        if (!activeCoroutines.ContainsKey(enemy))
        {
            Coroutine newCoroutine = StartCoroutine(ApplyDamageOverTime(enemy));
            activeCoroutines[enemy] = newCoroutine;
        }
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        enemy.ResetSpeedMultiplier();

        // Stop and remove this enemy’s coroutine if it’s active
        if (activeCoroutines.TryGetValue(enemy, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            activeCoroutines.Remove(enemy);
        }
    }

    // Coroutine to apply damage every second
    private IEnumerator ApplyDamageOverTime(EnemyMovement enemy)
    {
        while (enemy != null && enemy.Health > 0)
        {
            enemy.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }

    // Coroutine to remove ice after a set time
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);

        // Ensure all enemies are freed from the effect before destruction
        foreach (var enemy in activeCoroutines.Keys)
        {
            RemoveEffect(enemy);
        }

        activeCoroutines.Clear();
        Destroy(gameObject);
    }
}