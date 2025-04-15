using UnityEngine;

public class Pie : MonoBehaviour
{
    [Header("Pie Settings")]
    public float speed = 0.8f;
    public int damage = 15;

    private EnemyMovement target;

    public void SetTarget(EnemyMovement newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        // Deal damage to the enemy
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
