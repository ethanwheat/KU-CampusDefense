using System.Collections;
using UnityEngine;

public class BewareOfThePhogDefense : Defense, IDefense
{
    [Header("Phog Defense Settings")]
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private float spreadAngle = 360;
    [SerializeField] private GameObject fogPrefab;
    [SerializeField] private Transform fogSpawnPoint;

    private float timeElapsed = 0f;

    public override void Start()
    {
        StartCoroutine(SpawnPhog());
        base.Start();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1f)
        {
            SubtractHealth(1);
            timeElapsed = 0f;
        }
    }

    IEnumerator SpawnPhog()
    {
        Transform parent = RoundManager.instance.ProjectilesParent;
        GameObject phog = Instantiate(fogPrefab, parent);
        float angle = Random.Range(0f, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        phog.transform.position = fogSpawnPoint.position;
        phog.transform.rotation = Quaternion.LookRotation(direction);

        yield return new WaitForSeconds(spawnRate);

        if (enabled)
        {
            StartCoroutine(SpawnPhog());
        }
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        // Not needed for beward of the phog defense, but required by IDefense
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for beward of the phog defense, but required by IDefense
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}
