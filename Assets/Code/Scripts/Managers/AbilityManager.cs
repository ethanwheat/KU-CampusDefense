using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    [Header("Parent")]
    [SerializeField] private Transform abilitiesParent;

    private RoundManager roundManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        roundManager = RoundManager.instance;
    }

    // *********** SLOW ALL ENEMIES ***********
    private IEnumerator ApplySlowEffect(AbilityObject ability)
    {
        float multiplier = ability.SlowMultiplier; // This will be 1f for non-SlowAll Ability types

        roundManager.SlowAllEnemies(multiplier);

        yield return new WaitForSeconds(ability.EffectDuration);

        roundManager.ResetSlowAllEnemies();
    }

    // *********** FREEZE ALL ENEMIES ***********
    private IEnumerator ApplyFreezeEffect(float duration)
    {
        // Freeze enemies
        roundManager.FreezeAllEnemies();

        yield return new WaitForSeconds(duration);

        // Unfreeze enemies
        roundManager.ResetFreezeAllEnemies();
    }

    // *********** BIG JAY ABILITY ***********
    [Header("Prefabs")]
    [SerializeField] private GameObject bigJayRampagePrefab;
    [SerializeField] private PathNode[] startNodes; // Add this to reference the start nodes

    private IEnumerator ActivateBigJayRampage(AbilityObject ability)
    {
        // If no start nodes are available, return
        if (startNodes == null || startNodes.Length == 0)
        {
            yield break;
        }

        // Choose a random spawn point
        int spawnIndex = Random.Range(0, startNodes.Length);
        PathNode startNode = startNodes[spawnIndex];

        // Get collider.
        Collider collider = startNode.GetComponent<Collider>();

        // Spawn Big Jay at the chosen start node
        var bigJay = Instantiate(
            bigJayRampagePrefab,
            abilitiesParent
        );

        bigJay.transform.position = startNode.transform.position;
        bigJay.transform.rotation = Quaternion.identity;

        var controller = bigJay.GetComponent<BigJayRampageController>();
        if (controller != null)
        {
            controller.Initialize(startNode, ability.BigJaySpeed);
        }
        else
        {
            Debug.LogError("BigJayRampageController missing on BIG JAY prefab!");
            Destroy(bigJay);
            yield break;
        }

        // Wait until Big Jay is destroyed
        yield return new WaitWhile(() => bigJay != null);
    }

    // ************ BUS RIDE **************
    [SerializeField] private GameObject busRidePrefab;

    private IEnumerator ActivateBusRide(AbilityData ability)
    {
        if (startNodes == null || startNodes.Length == 0)
        {
            Debug.Log("No start nodes available for Bus Ride");
            yield break;
        }

        int spawnIndex = Random.Range(0, startNodes.Length);
        PathNode startNode = startNodes[spawnIndex];

        var bus = Instantiate(
            busRidePrefab,
            startNode.transform.position,
            Quaternion.identity
        );

        var controller = bus.GetComponent<BigJayRampageController>();
        if (controller != null)
        {
            controller.Initialize(startNode, ability.BusSpeed);
        }
        else
        {
            Debug.LogError("BigJayRampageController missing on BUS prefab!");
            Destroy(bus);
            yield break;
        }

        // Wait until Bus Ride is destroyed
        yield return new WaitWhile(() => bus != null);
    }

    public void ActivateAbility(AbilityData ability)
    {
        switch (ability.Type)
        {
            case AbilityObject.AbilityType.SlowAll:
                StartCoroutine(ApplySlowEffect(ability));
                break;

            case AbilityObject.AbilityType.FreezeAll:
                StartCoroutine(ApplyFreezeEffect(ability.EffectDuration));
                break;

            case AbilityObject.AbilityType.BigJayRampage:
                StartCoroutine(ActivateBigJayRampage(ability));
                break;

            case AbilityData.AbilityType.BusRide:
                StartCoroutine(ActivateBusRide(ability));
                break;
        }
    }

    public void StartEffectRoutine(AbilityButtonController abilityButtonController, AbilityObject abilityData)
    {
        StartCoroutine(EffectRoutine(abilityButtonController, abilityData));
    }

    private IEnumerator EffectRoutine(AbilityButtonController abilityButtonController, AbilityObject abilityData)
    {
        float remainingEffect = abilityData.EffectDuration;

        abilityButtonController.setIsEffectActive(true);
        abilityButtonController.ShowTimer(true);
        abilityButtonController.SetTimerEffectColor();

        while (remainingEffect > 0)
        {
            remainingEffect -= Time.deltaTime;
            abilityButtonController.SetTimerText(Mathf.CeilToInt(remainingEffect).ToString());
            yield return null;
        }

        abilityButtonController.setIsEffectActive(false);
        StartCoroutine(CooldownRoutine(abilityButtonController, abilityData));
    }

    private IEnumerator CooldownRoutine(AbilityButtonController abilityButtonController, AbilityObject abilityData)
    {
        float remainingCooldown = abilityData.CooldownDuration;

        abilityButtonController.setIsOnCooldown(true);
        abilityButtonController.SetTimerCooldownColor();

        while (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            abilityButtonController.SetTimerText(Mathf.CeilToInt(remainingCooldown).ToString());
            yield return null;
        }

        abilityButtonController.ShowTimer(false);
        abilityButtonController.setIsOnCooldown(false);
    }
}