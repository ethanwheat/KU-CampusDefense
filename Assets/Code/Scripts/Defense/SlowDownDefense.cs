using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowDownDefense : Defense, IDefense
{
    [Header("Settings")]
    [SerializeField] private float normalSlowMultiplier = 0.2f;
    [SerializeField] private float sensitiveSlowMultiplier = 0.2f;
    [SerializeField] private float normalDamagePerSecond = 5f;
    [SerializeField] private float sensitiveDamagePerSecond = 5f;

    private Dictionary<EnemyMovement, Coroutine> activeCoroutines = new Dictionary<EnemyMovement, Coroutine>();

    public void ApplyEffect(EnemyMovement enemy)
    {
        if (enemy.CompareTag("SensitiveEnemy"))
        {
            enemy.SetSpeedMultiplier(sensitiveSlowMultiplier);
        }
        else
        {
            enemy.SetSpeedMultiplier(normalSlowMultiplier);
        }

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
            if (enemy.CompareTag("SensitiveEnemy"))
            {
                enemy.TakeDamage(sensitiveDamagePerSecond);
            }
            else
            {
                enemy.TakeDamage(normalDamagePerSecond);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // Ensure all enemies are freed from the effect before destruction
    public override void OnDefenseDestroy()
    {
        foreach (var enemy in new List<EnemyMovement>(activeCoroutines.Keys))
        {
            RemoveEffect(enemy);
        }

        activeCoroutines.Clear();
        base.OnDefenseDestroy();
    }
}