using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  public PathNode currentNode; // The waypoint the object is moving toward
  public float speed;
  private bool isBlocked = false;

  // Update is called once per frame
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

  // Stop movement when touching an obstacle
  private void OnTriggerEnter(Collider other)
  {
    PlacementController placementController = other.gameObject.GetComponent<PlacementController>();
    bool isPlaced = placementController.isPlaced;

    if (isPlaced)
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
  }

  // Resume movement when no longer touching an obstacle
  private void OnTriggerExit(Collider other)
  {
    PlacementController placementController = other.gameObject.GetComponent<PlacementController>();
    bool isPlaced = placementController.isPlaced;

    if (isPlaced)
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
}
