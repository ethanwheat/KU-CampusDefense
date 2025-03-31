using System.Collections;
using UnityEngine;

public class BaseballFieldDefense : MonoBehaviour, IDefenseEffect
{
    [Header("Baseball Field Settings")]
    [SerializeField] private GameObject baseballPrefab;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int ballsPerWave = 6;
    [SerializeField] private float spreadAngle = 360f;
    [SerializeField] private float duration = 20f;

    [Header("Health Bar Script")]
    [SerializeField] private HealthBar healthBar;

    private float destroyTime;
    private bool isFiring = false;

    private void OnEnable()
    {
        StartBaseballWaves();
    }

    private void StartBaseballWaves()
    {
        if (isFiring) return;

        isFiring = true;
        destroyTime = Time.time + duration;
        StartCoroutine(FireBaseballWaves());
    }

    private void Update()
    {
        if (!isFiring) return;

        float timeLeft = destroyTime - Time.time;
        healthBar.UpdateHealthBar(timeLeft, duration);

        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FireBaseballWaves()
    {
        while (Time.time < destroyTime)
        {
            for (int i = 0; i < ballsPerWave; i++)
            {
                FireRandomBaseball();
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void FireRandomBaseball()
    {
        float angle = Random.Range(0f, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        GameObject baseball = Instantiate(baseballPrefab, transform.position + Vector3.up * 1.5f, Quaternion.LookRotation(direction));
        // No need to manually apply speed — handled in Baseball.cs
    }

    // Required by IDefenseEffect interface
    public void ApplyEffect(EnemyMovement enemy) { }
    public void RemoveEffect(EnemyMovement enemy) { }
}
