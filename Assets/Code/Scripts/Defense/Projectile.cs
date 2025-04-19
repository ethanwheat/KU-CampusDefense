using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private bool continuousDamage = false;

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            if (continuousDamage)
            {
                float damageThisFrame = damage * Time.deltaTime;
                enemy.TakeDamage(damageThisFrame);

                return;
            }

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public float Speed => speed;
}