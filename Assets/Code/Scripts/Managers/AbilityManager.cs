using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    [Header("Dependencies")]
    [SerializeField] private RoundManager roundManager;

    private List<EnemyMovement> activeEnemies = new List<EnemyMovement>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(EnemyMovement enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyMovement enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    // *********** SLOW ALL ENEMIES ***********
    private IEnumerator ApplySlowEffect(AbilityData ability)
    {
        float multiplier = ability.SlowMultiplier; // This will be 1f for non-SlowAll Ability types

        foreach (var enemy in activeEnemies)
        {
            enemy?.SetSpeedMultiplier(multiplier);
        }

        yield return new WaitForSeconds(ability.EffectDuration);

        foreach (var enemy in activeEnemies)
        {
            enemy?.ResetSpeedMultiplier();
        }
    }

    // *********** FREEZE ALL ENEMIES ***********
    private IEnumerator ApplyFreezeEffect(float duration)
    {
        // Freeze enemies
        foreach (var enemy in activeEnemies)
        {
            enemy?.BlockMovement(true);
        }

        yield return new WaitForSeconds(duration);

        // Unfreeze enemies
        foreach (var enemy in activeEnemies)
        {
            enemy?.BlockMovement(false);
        }
    }

    // *********** BIG JAY ABILITY ***********
    [Header("Prefabs")]
    [SerializeField] private GameObject bigJayRampagePrefab;
    [SerializeField] private PathNode[] startNodes; // Add this to reference the start nodes

    private IEnumerator ActivateBigJayRampage(AbilityData ability)
    {
        // If no start nodes are available, return
        if (startNodes == null || startNodes.Length == 0)
        {
            Debug.Log("No start nodes available for Big Jay");
            yield break;
        }

        // Choose a random spawn point
        int spawnIndex = Random.Range(0, startNodes.Length);
        PathNode startNode = startNodes[spawnIndex];

        // Spawn Big Jay at the chosen start node
        var bigJay = Instantiate(
            bigJayRampagePrefab,
            startNode.transform.position,
            Quaternion.identity
        );

        var controller = bigJay.GetComponent<BigJayRampageController>();
        if (controller != null)
        {
            controller.Initialize(startNode, ability.BigJaySpeed);
        }
        else
        {
            Debug.LogError("BigJayRampageController missing on prefab!");
            Destroy(bigJay);
            yield break;
        }

        // Wait until Big Jay is destroyed
        yield return new WaitWhile(() => bigJay != null);
    }

    public void ActivateAbility(AbilityData ability)
    {
        Debug.Log($"Activating ability: {ability.AbilityName}");

        switch (ability.Type)
        {
            case AbilityData.AbilityType.SlowAll:
                StartCoroutine(ApplySlowEffect(ability));
                break;

            case AbilityData.AbilityType.FreezeAll:
                StartCoroutine(ApplyFreezeEffect(ability.EffectDuration));
                break;

            case AbilityData.AbilityType.BigJayRampage:
                StartCoroutine(ActivateBigJayRampage(ability));
                break;
        }
    }
}