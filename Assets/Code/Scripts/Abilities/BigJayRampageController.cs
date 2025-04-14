using UnityEngine;

public class BigJayRampageController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    //[SerializeField] private ParticleSystem trailEffect;
    //[SerializeField] private AudioClip rampageSound;

    private PathNode currentNode;
    //private AudioSource audioSource;

    public void Initialize(PathNode startNode, float speed)
    {
        currentNode = startNode;
        movementSpeed = speed;
        //audioSource = GetComponent<AudioSource>();

        //if (trailEffect != null) trailEffect.Play();
        //if (rampageSound != null) audioSource.PlayOneShot(rampageSound);

        transform.position = currentNode.transform.position;
        // Face first path node
        if (currentNode != null && currentNode.GetNextNode() != null)
        {
            transform.LookAt(currentNode.GetNextNode().transform.position);
        }
    }

    void FixedUpdate()
    {
        if (currentNode == null) return;

        // Rotate toward the next waypoint
        Vector3 direction = currentNode.transform.position - transform.position;
        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, Time.deltaTime * 5f);
        }

        // Move along path
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentNode.transform.position,
            movementSpeed * Time.deltaTime
        );

        // Check if reached current node
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 0.1f)
        {
            currentNode = currentNode.GetNextNode();
            if (currentNode == null) Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyMovement>(out var enemy))
        {
            enemy.TakeDamage(enemy.Health); // Instant kill
        }
    }
}