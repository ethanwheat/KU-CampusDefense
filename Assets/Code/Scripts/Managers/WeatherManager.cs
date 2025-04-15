using System.Collections;
using UnityEngine;
using TMPro;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [Header("Weather UI")]
    [SerializeField] private GameObject rainOverlay;

    [Header("UI Controllers")]
    [SerializeField] private SmallMessagePopupPanel smallMessagePopupPanelController;

    [Header("Weather Settings")]
    [SerializeField] private float weatherChance = 0.3f; // 30% chance
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float duration = 5f;

    [Header("Lighting")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float darkenedLightIntensity = 0.3f;

    [Header("Round Manager")]
    [SerializeField] private RoundManager roundManager;

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
        // Wait until previous small message popup hides.
        yield return new WaitForSeconds(4f);

        // Set weather to triggered.
        weatherTriggeredThisRound = true;

        // Get panel fade controller.
        PanelFadeController panelFadeController = rainOverlay.GetComponent<PanelFadeController>();

        // Show overlays and popup
        panelFadeController.Show();

        // Show message.
        smallMessagePopupPanelController.showPanel("Rain Incoming! Enemies Slowed!");

        // Set directionalLightDuration and elapsedTime.
        float directionalLightFadeDuration = .3f;
        float elapsedTime = 0f;

        // Fade in directional light.
        while (elapsedTime < directionalLightFadeDuration)
        {
            directionalLight.intensity = Mathf.Lerp(originalLightIntensity, darkenedLightIntensity, elapsedTime / directionalLightFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set directional light to darkenedLightIntensitity.
        if (directionalLight != null)
        {
            directionalLight.intensity = darkenedLightIntensity;
        }

        // Slow enemies
        roundManager.SlowAllEnemies(slowMultiplier);

        // Wait for duration.
        yield return new WaitForSeconds(duration);

        // Restore enemies
        roundManager.ResetSlowAllEnemies();

        // Restore lighting and overlays
        panelFadeController.Hide();

        // Reset elapsedTime.
        elapsedTime = 0f;

        // Fade out directional light.
        while (elapsedTime < directionalLightFadeDuration)
        {
            directionalLight.intensity = Mathf.Lerp(darkenedLightIntensity, originalLightIntensity, elapsedTime / directionalLightFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set directional light to originalLightIntensity.
        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity;
        }
    }
}
