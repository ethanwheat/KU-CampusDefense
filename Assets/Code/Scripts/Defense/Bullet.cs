using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
}