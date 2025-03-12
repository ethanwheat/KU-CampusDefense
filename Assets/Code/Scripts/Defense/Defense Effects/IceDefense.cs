using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceDefense : MonoBehaviour, IDefenseEffect
{
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float damagePerSecond = 5f;

    private Dictionary<EnemyMovement, Coroutine> activeCoroutines = new Dictionary<EnemyMovement, Coroutine>();

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
}

