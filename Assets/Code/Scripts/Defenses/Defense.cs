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
    [SerializeField] private bool timedHealth = true;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthIncreasePerLevel = 50f;

    private float health;
    private float timeElapsed = 0f;
    private DefenseData defenseData;

    public DefenseObject DefenseObject => defenseObject;
    public DefenseData DefenseData => defenseData;
    public HealthBar HealthBar => healthBar;

    public virtual void Start()
    {
        RoundManager roundManager = RoundManager.instance;
        defenseData = GameDataManager.instance.GameData.GetDefenseData(defenseObject.ObjectName);

        if (defenseData == null)
        {
            enabled = false;
            return;
        }

        int defenseLevel = defenseData.Level;
        maxHealth += healthIncreasePerLevel * (defenseLevel - 1);
        ResetHealth();

        transform.parent = roundManager.DefensesParent;
        roundManager.AddDefense(this);
    }

    public virtual void Update()
    {
        if (timedHealth)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 1f)
            {
                SubtractHealth(1);
                timeElapsed = 0f;
            }
        }
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
        healthBar.UpdateHealthBar(health, maxHealth);

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
        RoundManager.instance.RemoveDefense(this);
        SoundManager.instance.PlaySoundEffect(destroySoundEffect, transform);
        Destroy(gameObject);
    }
}