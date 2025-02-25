using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public PathNode currentNode; // The waypoint the object is moving toward
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
      if (currentNode == null) return;

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
}
