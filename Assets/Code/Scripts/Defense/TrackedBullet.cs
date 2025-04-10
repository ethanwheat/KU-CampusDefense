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
            //Debug.Log("Target is null. Destroying bullet.");
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = GetSpeed() * Time.deltaTime;

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        //Debug.Log("Bullet moving towards target: " + target.name);
    }
}