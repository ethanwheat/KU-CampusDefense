using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChemistryLabSpillDefense : HealthDefense, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float normalSlowMultiplier = 0.3f;
    [SerializeField] private float sensitiveSlowMultiplier = 0.1f;
    [SerializeField] private float normalDamagePerSecond = 3f;
    [SerializeField] private float sensitiveDamagePerSecond = 6f;

    private float timeElapsed = 0f;

    private Dictionary<EnemyMovement, Coroutine> activeCoroutines = new Dictionary<EnemyMovement, Coroutine>();

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1f)
        {
            subtractHealth(1);
            timeElapsed = 0f;
        }

        if (getHealth() == 0)
        {
            foreach (var enemy in new List<EnemyMovement>(activeCoroutines.Keys))
            {
                RemoveEffect(enemy);
            }

            activeCoroutines.Clear();
            Destroy(gameObject);
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        if (enemy == null) return;

        if (IsChemicalSensitive(enemy))
        {
            enemy.SetSpeedMultiplier(sensitiveSlowMultiplier);
        }
        else
        {
            enemy.SetSpeedMultiplier(normalSlowMultiplier);
        }

        if (!activeCoroutines.ContainsKey(enemy))
        {
            Coroutine newCoroutine = StartCoroutine(ApplyDamageOverTime(enemy));
            activeCoroutines[enemy] = newCoroutine;
        }
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        if (enemy == null) return;

        enemy.ResetSpeedMultiplier();

        if (activeCoroutines.TryGetValue(enemy, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            activeCoroutines.Remove(enemy);
        }
    }

    private IEnumerator ApplyDamageOverTime(EnemyMovement enemy)
    {
        while (enemy != null && enemy.Health > 0)
        {
            if (IsChemicalSensitive(enemy))
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

    private bool IsChemicalSensitive(EnemyMovement enemy)
    {
        // Example: check enemy's tag
        return enemy.CompareTag("ChemicalSensitive");
    }
}
