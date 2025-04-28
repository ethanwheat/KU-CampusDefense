using UnityEngine;

[RequireComponent(typeof(Collider))] // optional but helpful
public class FailedTestsDefense : TurretDefense, IDefense
{
    [Header("Failed Tests Settings")]
    [SerializeField] private float eliminatePercent = 0.25f;

    public override void Update()
    {
        DefenseUpdate();
    }

    public override void ApplyEffect(EnemyMovement enemy)
    {
        float roll = Random.value;
        if (roll < eliminatePercent)
        {
            Shoot(enemy);
        }
    }

    public override void RemoveEffect(EnemyMovement enemy)
    {
        // Not needed for librarian, but required by IDefense
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}