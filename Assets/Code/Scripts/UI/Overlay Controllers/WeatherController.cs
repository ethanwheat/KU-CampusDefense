using System.Collections;
using UnityEngine;
using TMPro;

public class WeatherController : MonoBehaviour
{
    public static WeatherController Instance;

    [Header("Weather UI")]
    [SerializeField] private GameObject rainOverlay;
    [SerializeField] private GameObject darkenOverlay; // Optional dark screen tint
    [SerializeField] private TextMeshProUGUI weatherPopupText;

    [Header("Weather Settings")]
    [SerializeField] private float weatherChance = 0.3f; // 30% chance
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float duration = 5f;

    [Header("Lighting")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float darkenedLightIntensity = 0.3f;

    private bool weatherTriggeredThisRound = false;
    private float originalLightIntensity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (directionalLight != null)
            {
                originalLightIntensity = directionalLight.intensity;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetWeatherForNewRound()
    {
        weatherTriggeredThisRound = false;
    }

    public void TryActivateWeather()
    {
        if (weatherTriggeredThisRound) return;

        if (Random.value <= weatherChance)
        {
            StartCoroutine(ActivateWeather());
        }
    }

    IEnumerator ActivateWeather()
    {
        weatherTriggeredThisRound = true;

        // Show overlays and popup
        rainOverlay.SetActive(true);
        if (darkenOverlay != null) darkenOverlay.SetActive(true);

        weatherPopupText.text = "Rain Incoming! Enemies Slowed!";
        weatherPopupText.gameObject.SetActive(true);
        StartCoroutine(HidePopupAfterSeconds(3f));

        // Dim lighting
        if (directionalLight != null)
        {
            directionalLight.intensity = darkenedLightIntensity;
        }

        // Slow enemies
        foreach (var enemy in FindObjectsOfType<EnemyMovement>())
        {
            enemy.SetSpeedMultiplier(slowMultiplier);
        }

        yield return new WaitForSeconds(duration);

        // Restore enemies
        foreach (var enemy in FindObjectsOfType<EnemyMovement>())
        {
            enemy.ResetSpeedMultiplier();
        }

        // Restore lighting and overlays
        rainOverlay.SetActive(false);
        if (darkenOverlay != null) darkenOverlay.SetActive(false);

        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity;
        }
    }

    IEnumerator HidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        weatherPopupText.gameObject.SetActive(false);
    }
}
