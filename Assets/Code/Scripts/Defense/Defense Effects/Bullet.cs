using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public int damage = 10;

    private EnemyMovement target;

    public void SetTarget(EnemyMovement newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        //Debug.Log("Bullet spawned at: " + transform.position);
    }

    void Update()
    {
        if (target == null)
        {
            //Debug.Log("Target is null. Destroying bullet.");
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            //Debug.Log("Bullet hit target: " + target.name);
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        //Debug.Log("Bullet moving towards target: " + target.name);
    }

    private void HitTarget()
    {
        // Deal damage to the enemy
        //Debug.Log("Bullet hit target: " + target.name);
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}