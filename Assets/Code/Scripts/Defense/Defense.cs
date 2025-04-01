using UnityEngine;
using UnityEngine.SceneManagement;

public class Defense : MonoBehaviour
{
    [Header("Defense Data")]
    [SerializeField] private DefenseData defenseData;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    private Transform projectilesParent;

    // Get defense data.
    public DefenseData getDefenseData()
    {
        return defenseData;
    }

    // Get health bar.
    public HealthBar getHealthBar()
    {
        return healthBar;
    }

    // Get projectiles parent.
    public Transform getProjectilesParent()
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