using UnityEngine;

public class Baseball : MonoBehaviour
{
    [Header("Baseball Settings")]
    public float speed = 10f;
    public float lifeTime = 5f;
    public int damage = 20;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
