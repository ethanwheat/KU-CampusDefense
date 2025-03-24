using UnityEngine;
public interface IDefenseEffect
{
    void ApplyEffect(EnemyMovement enemy);   // Called on Enter
    void RemoveEffect(EnemyMovement enemy);  // Called on Exit
}
