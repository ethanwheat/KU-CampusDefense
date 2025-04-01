using System;
using UnityEngine;

public class HealthDefense : Defense
{
    [Header("Defense Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthIncreasePerLevel = 50f;

    private float health;

    void Awake()
    {
        int defenseLevel = getDefenseData().getLevel();

        maxHealth += healthIncreasePerLevel * (defenseLevel - 1);

        resetHealth();
    }

    // Get health.
    public float getHealth()
    {
        return health;
    }

    // Set health.
    public void setHealth(float amount)
    {
        health = Mathf.Clamp(amount, 0, maxHealth);
        getHealthBar().UpdateHealthBar(health, maxHealth);
    }

    // Subtract health.
    public void subtractHealth(float amount)
    {
        setHealth(health - amount);
    }

    // Reset health to max health.
    public void resetHealth()
    {
        setHealth(maxHealth);
    }
}