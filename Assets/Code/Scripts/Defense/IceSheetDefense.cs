using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceSheetDefense : HealthDefense, IDefenseEffect
{
    [Header("Settings")]
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private float damagePerSecond = 5f;

    [Header("Sounds")]
    [SerializeField] private AudioClip slowSoundEffect;
    [SerializeField] private AudioClip destroySoundEffect;

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

        // Destroy if destroy time is less then current time.
        if (getHealth() == 0)
        {
            // Ensure all enemies are freed from the effect before destruction
            foreach (var enemy in new List<EnemyMovement>(activeCoroutines.Keys))
            {
                RemoveEffect(enemy);
            }

            activeCoroutines.Clear();
            SoundManager.instance.playSoundEffect(destroySoundEffect, transform, .25f);
            Destroy(gameObject);
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
            SoundManager.instance.playSoundEffect(slowSoundEffect, transform, .25f);
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