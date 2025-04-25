using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowDownDefense : Defense, IDefense
{
    [Header("Settings")]
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float damagePerSecond = 5f;

    private float timeElapsed = 0f;

    private Dictionary<EnemyMovement, Coroutine> activeCoroutines = new Dictionary<EnemyMovement, Coroutine>();

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1f)
        {
            SubtractHealth(1);
            timeElapsed = 0f;
        }
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
            if (!enabled) { yield return new WaitUntil(() => enabled); }

            enemy.TakeDamage(damagePerSecond);
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