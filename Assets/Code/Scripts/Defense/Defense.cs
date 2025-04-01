using System;
using UnityEngine;

public class Defense : MonoBehaviour
{
    [Header("Defense Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float levelHealthMultiplier = 1.5f;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    [Header("Defense Data")]
    [SerializeField] private DefenseData defenseData;

    private float health;

    void Start()
    {
        int defenseLevel = defenseData.getLevel();

        if (defenseLevel > 1)
        {
            maxHealth = maxHealth * levelHealthMultiplier * (defenseLevel - 1);
        }

        resetHealth();
    }

    // Get max health.
    public float getMaxHealth()
    {
        return maxHealth;
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
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Subtract health.
    public void subtractHealth(float amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Reset health to max health.
    public void resetHealth()
    {
        setHealth(maxHealth);
    }

    // Get defense data.
    public DefenseData getDefenseData()
    {
        return defenseData;
    }
}