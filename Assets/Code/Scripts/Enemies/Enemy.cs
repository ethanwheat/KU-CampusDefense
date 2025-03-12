using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public PathNode currentNode; // The waypoint the object is moving toward
    private bool isBlocked = false;

    void Start()
    {
        if (currentNode == null)
        {
            Debug.LogError("Current node is not assigned.");
        }
    }

    void FixedUpdate()
    {
        if (currentNode == null || isBlocked) return;

        // Rotate toward the next waypoint
        Vector3 direction = currentNode.transform.position - transform.position;
        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, Time.deltaTime * 5f);
        }

        // Move toward the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        // If close enough, pick the next waypoint
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 0.1f)
        {
            currentNode = currentNode.GetNextNode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isBlocked = true;
        }
        else if (other.CompareTag("Ice"))
        {
            speed /= 5;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isBlocked = false;
        }
        else if (other.CompareTag("Ice"))
        {
            speed *= 5;
        }
    }
}