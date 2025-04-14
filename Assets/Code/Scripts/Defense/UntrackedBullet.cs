using UnityEngine;

public class UntrackedBullet : Bullet
{
    [Header("Untracked Bullet Settings")]
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * GetSpeed() * Time.deltaTime);
    }
}
