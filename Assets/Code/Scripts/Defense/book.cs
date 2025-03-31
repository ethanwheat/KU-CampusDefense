using UnityEngine;

public class Book : MonoBehaviour
{
    [Header("Book Settings")]
    public float speed = 0.8f;
    public int damage = 15;

    private EnemyMovement target;

    public void SetTarget(EnemyMovement newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        //Debug.Log("Book spawned at: " + transform.position);
    }

    void Update()
    {
        if (target == null)
        {
            //Debug.Log("Target is null. Destroying book.");
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            //Debug.Log("Book hit target: " + target.name);
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        //Debug.Log("Book moving towards target: " + target.name);
    }

    private void HitTarget()
    {
        // Deal damage to the enemy
        //Debug.Log("Book hit target: " + target.name);
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}