using System.Collections;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private AbilityObject abilityObject;
    private PathNode currentNode;

    public void Initialize(AbilityObject abilityObject, PathNode startNode)
    {
        this.abilityObject = abilityObject;
        currentNode = startNode;
        movementSpeed = abilityObject.Speed;

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
            if (abilityObject.Type == AbilityObject.AbilityType.BigJayRampage)
            {
                StartCoroutine(BigJayRampageCoroutine(enemy));
                return;
            }

            if (abilityObject.Type == AbilityObject.AbilityType.BusRide)
            {
                enemy.TakeDamage(enemy.Health); // Instant kill
                return;
            }
        }
    }

    IEnumerator BigJayRampageCoroutine(EnemyMovement enemyMovement)
    {
        enemyMovement.SetSpeedMultiplier(.5f);

        yield return new WaitForSeconds(30f);

        enemyMovement.ResetSpeedMultiplier();
    }
}