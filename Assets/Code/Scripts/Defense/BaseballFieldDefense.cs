using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BaseballFieldDefense : Defense, IDefense
{
    [Header("Baseball Field Settings")]
    [SerializeField] private int ballsPerWave = 6;
    [SerializeField] private int ballsIncreasePerLevel = 2;
    [SerializeField] private float timeBetweenBaseballs = 1f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float spreadAngle = 360f;

    [Header("Baseball Field Prefabs")]
    [SerializeField] private GameObject baseballPrefab;

    [Header("Baseball Field Sounds")]
    [SerializeField] private AudioClip shootSoundEffect;

    [SerializeField] private AudioClip rechargeSoundEffect;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent onDefenseStart;

    void Start()
    {
        DefenseData defenseData = GetDefenseData();
        bool isBought = defenseData.isBought();
        int level = defenseData.getLevel();

        if (isBought)
        {
            onDefenseStart.Invoke();
            ballsPerWave += ballsIncreasePerLevel * (level - 1);
            StartCoroutine(FireBaseballWaves());
        }
    }

    private IEnumerator FireBaseballWaves()
    {
        HealthBar healthBar = GetHealthBar();

        int ballsLeft = ballsPerWave;

        for (int i = 0; i < ballsPerWave; i++)
        {
            ballsLeft--;
            healthBar.UpdateHealthBar(ballsLeft, ballsPerWave);
            FireRandomBaseball();
            yield return new WaitForSeconds(timeBetweenBaseballs);
        }

        SoundManager.instance.playSoundEffect(rechargeSoundEffect, transform, .5f);
        healthBar.UpdateHealthBar(ballsPerWave, ballsPerWave);

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(FireBaseballWaves());
    }

    private void FireRandomBaseball()
    {
        float angle = Random.Range(0f, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        GameObject ball = Instantiate(baseballPrefab, transform.position + Vector3.up * 1.5f, Quaternion.LookRotation(direction));
        ball.transform.parent = GetProjectilesParent();
        SoundManager.instance.playSoundEffect(shootSoundEffect, transform, .5f);
        // No need to manually apply speed — handled in Baseball.cs
    }

    // Required by IDefenseEffect interface
    public void ApplyEffect(EnemyMovement enemy) { }
    public void RemoveEffect(EnemyMovement enemy) { }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}
