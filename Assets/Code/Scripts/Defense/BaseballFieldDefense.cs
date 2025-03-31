using System.Collections;
using UnityEngine;

public class BaseballFieldDefense : MonoBehaviour, IDefenseEffect
{
    [Header("Baseball Field Settings")]
    [SerializeField] private GameObject baseballPrefab;
    [SerializeField] private float timeBetweenBaseballs = 1f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int ballsPerWave = 6;
    [SerializeField] private float spreadAngle = 360f;

    [Header("Building Placement Controller")]
    [SerializeField] private BuildingPlacementController buildingPlacementController;

    void Start()
    {
        if (buildingPlacementController.isRoundScene())
        {
            StartCoroutine(FireBaseballWaves());
        }
    }

    private IEnumerator FireBaseballWaves()
    {
        for (int i = 0; i < ballsPerWave; i++)
        {
            FireRandomBaseball();
            yield return new WaitForSeconds(timeBetweenBaseballs);
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(FireBaseballWaves());
    }

    private void FireRandomBaseball()
    {
        float angle = Random.Range(0f, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        Instantiate(baseballPrefab, transform.position + Vector3.up * 1.5f, Quaternion.LookRotation(direction));
        // No need to manually apply speed — handled in Baseball.cs
    }

    // Required by IDefenseEffect interface
    public void ApplyEffect(EnemyMovement enemy) { }
    public void RemoveEffect(EnemyMovement enemy) { }
}
