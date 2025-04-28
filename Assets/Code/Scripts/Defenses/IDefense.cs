public interface IDefense
{
    void OnDefenseDestroy();
    void ApplyEffect(EnemyMovement enemy);
    void RemoveEffect(EnemyMovement enemy);
}
