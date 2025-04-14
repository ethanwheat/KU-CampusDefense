using UnityEngine;
using UnityEngine.SceneManagement;

public class Defense : MonoBehaviour
{
    [Header("Defense Data")]
    [SerializeField] private DefenseData defenseData;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    [Header("Defense Sounds")]
    [SerializeField] private AudioClip destroySoundEffect;

    [Header("Defense Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthIncreasePerLevel = 50f;

    private float health;
    private Transform projectilesParent;

    void Awake()
    {
        int defenseLevel = getDefenseData().getLevel();

        maxHealth += healthIncreasePerLevel * (defenseLevel - 1);

        ResetHealth();
    }

    // Get health.
    public float GetHealth()
    {
        return health;
    }

    // Set health.
    public void SetHealth(float amount)
    {
        health = Mathf.Clamp(amount, 0, maxHealth);
        GetHealthBar().UpdateHealthBar(health, maxHealth);

        if (health == 0)
        {
            OnDefenseDestroy();
        }
    }

    // Subtract health.
    public void SubtractHealth(float amount)
    {
        SetHealth(health - amount);
    }

    // Reset health to max health.
    public void ResetHealth()
    {
        SetHealth(maxHealth);
    }

    // Called when defense is out of health.
    public virtual void OnDefenseDestroy()
    {
        SoundManager.instance.playSoundEffect(destroySoundEffect, transform, .5f);
        Destroy(gameObject);
    }

    // Get defense data.
    public DefenseData getDefenseData()
    {
        return defenseData;
    }

    // Get health bar.
    public HealthBar GetHealthBar()
    {
        return healthBar;
    }

    // Get projectiles parent.
    public Transform GetProjectilesParent()
    {
        if (projectilesParent)
        {
            return projectilesParent;
        }

        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "Projectiles")
            {
                projectilesParent = currentGameObject.transform;
                break;
            }
        }

        if (!projectilesParent)
        {
            projectilesParent = new GameObject("Projectiles").transform;
        }

        return projectilesParent;
    }
}