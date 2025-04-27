using UnityEngine;

[RequireComponent(typeof(Collider))] // optional but helpful
public class FailedTestsDefense : Defense, IDefense
{
    [Header("Failed Tests Settings")]
    [SerializeField] private float eliminatePercent = 0.1f;

    public void ApplyEffect(EnemyMovement enemy)
    {
        float roll = Random.value;
        if (roll < eliminatePercent)
        {
            enemy.TakeDamage(enemy.Health);
        }
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for librarian, but required by IDefense
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}