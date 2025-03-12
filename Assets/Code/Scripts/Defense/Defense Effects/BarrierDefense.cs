using UnityEngine;

public class BarrierDefense : MonoBehaviour, IDefenseEffect
{
    public void ApplyEffect(EnemyMovement enemy)
    {
        enemy.BlockMovement(true);
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        enemy.BlockMovement(false);
    }
}
