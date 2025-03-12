using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  public PathNode currentNode; // The waypoint the object is moving toward
  [SerializeField] private float speed;
  [SerializeField] private float health; 
  [SerializeField] private HealthBar healthBar; // Reference to the HealthBar script

  private float baseSpeed;
  private float maxHealth;
  private bool isBlocked = false;

  public float Health => health; // read only access

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
      var nextNode = currentNode.GetNextNode();
      if (nextNode != null)
      {
        currentNode = currentNode.GetNextNode();
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }

  // defense reactions
  private void OnTriggerEnter(Collider other)
  {
    if (!other.TryGetComponent(out DefencePlacementController placementController)) return;
    if (!placementController.isPlaced()) return;

    if (other.TryGetComponent(out IDefenseEffect defenseEffect))
    {
      defenseEffect.ApplyEffect(this);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.TryGetComponent(out DefencePlacementController placementController)) return;
    if (!placementController.isPlaced()) return;

    if (other.TryGetComponent(out IDefenseEffect defenseEffect))
    {
      defenseEffect.RemoveEffect(this);
    }
  }

  public void SetSpeed(float initSpeed)
  {
    speed = initSpeed;
    baseSpeed = initSpeed;
  }

  public void SetHealth(float initHealth)
  {
    health = initHealth;
    maxHealth = initHealth;
  }

  public void TakeDamage(float amount)
  {
    health -= amount;
    healthBar.UpdateHealthBar(health, maxHealth);
    if (health <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    Destroy(gameObject); // Or trigger a death animation, effects, etc.
  }

  public void SetSpeedMultiplier(float multiplier)
  {
    speed = baseSpeed * multiplier;
  }

  public void ResetSpeedMultiplier()
  {
    speed = baseSpeed;
  }

  public void BlockMovement(bool isBlocked)
  {
    this.isBlocked = isBlocked;
  }
}
