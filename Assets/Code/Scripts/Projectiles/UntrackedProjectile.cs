using UnityEngine;

public class UntrackedProjectile : Projectile
{
    [Header("Untracked Projectile Settings")]
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float growthRate = 0;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        float scaleIncrease = growthRate * Time.deltaTime;
        transform.localScale += Vector3.one * scaleIncrease;
    }
}
