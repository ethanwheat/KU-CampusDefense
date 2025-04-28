using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private bool immediateKill = false;
    [SerializeField] private bool continuousDamage = false;

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            float updatedDamage = immediateKill ? enemy.Health : damage;

            if (continuousDamage)
            {
                float damageThisFrame = updatedDamage * Time.deltaTime;
                enemy.TakeDamage(damageThisFrame);

                return;
            }

            enemy.TakeDamage(updatedDamage);
            Destroy(gameObject);
        }
    }

    public float Speed => speed;
}