using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        healthBarFill.fillAmount = fillAmount;

        // Optional: Color change for visual clarity
        if (fillAmount > 0.75f)
            healthBarFill.color = Color.green;
        else if (fillAmount > 0.25f)
            healthBarFill.color = Color.yellow;
        else
            healthBarFill.color = Color.red;
    }
}

