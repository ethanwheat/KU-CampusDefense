using UnityEngine;

public class TrackedBullet : Bullet
{
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
        float distanceThisFrame = GetSpeed() * Time.deltaTime;

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }
}