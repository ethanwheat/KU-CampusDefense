using UnityEngine;

public class Defense : MonoBehaviour
{
    [Header("Defense Data Object")]
    [SerializeField] private DefenseObject defenseObject;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    [Header("Defense Sounds")]
    [SerializeField] private AudioClip destroySoundEffect;

    [Header("Defense Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthIncreasePerLevel = 50f;

    private float health;
    private GameDataManager gameDataManager;
    private RoundManager roundManager;
    private SoundManager soundManager;
    private DefenseData defenseData;

    public DefenseObject DefenseObject => defenseObject;
    public DefenseData DefenseData => defenseData;

    public virtual void Start()
    {
        gameDataManager = GameDataManager.instance;
        roundManager = RoundManager.instance;
        soundManager = SoundManager.instance;

        gameDataManager.GameData.GetDefenseData(defenseObject.ObjectName);

        int defenseLevel = defenseObject.Level;
        maxHealth += healthIncreasePerLevel * (defenseLevel - 1);
        ResetHealth();

        transform.parent = roundManager.DefensesParent;
        roundManager.AddDefense(this);
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
        roundManager.RemoveDefense(this);
        soundManager.PlaySoundEffect(destroySoundEffect, transform, .5f);
        Destroy(gameObject);
    }

    // Get health bar.
    public HealthBar GetHealthBar()
    {
        return healthBar;
    }
}