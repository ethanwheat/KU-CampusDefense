using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  public PathNode currentNode; // The waypoint the object is moving toward
  [SerializeField] private float speed;
  [SerializeField] private float health;
  [SerializeField] private HealthBar healthBar; // Reference to the HealthBar script

  private RoundManager roundManager;
  private float baseSpeed;
  private float maxHealth;
  private bool isBlocked = false;
  private int killReward;
  private PathNode previousNode;

  // New fields for distraction
  private PathNode distractionNode;
  private bool isDistracted = false;

  public float Health => health; // read only access

  void Start()
  {
    roundManager = GameObject.Find("RoundManager").GetComponent<RoundManager>();
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
      PathNode nextNode;

      // If distracted, use distraction node once
      if (isDistracted && distractionNode != null)
      {
        nextNode = distractionNode;
        ClearDistraction(); // After using it once
      }
      else
      {
        nextNode = currentNode.GetNextNode();
      }

      if (nextNode != null)
      {
        previousNode = currentNode;
        currentNode = nextNode;
      }
      else  // Enemy has reached Allen Fieldhouse
      {
        if (roundManager != null)
        {
          roundManager.EnemyDefeated();
          roundManager.damageFieldhouse(health);
        }

        Destroy(gameObject);
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
    if (!placementController.isPlaced()) return;

    if (other.TryGetComponent(out IDefenseEffect defenseEffect))
    {
      defenseEffect.ApplyEffect(this);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
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

  public void SetReward(int reward)
  {
    killReward = reward;
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
    Debug.Log("Enemy died.");

    if (roundManager != null)
    {
      roundManager.EnemyDefeated();
      roundManager.addCoins(killReward);
    }
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

  // 🍔 Distraction methods (new)
  public void SetDistraction(PathNode distraction)
  {
    distractionNode = distraction;
    isDistracted = true;
  }

  public void ClearDistraction()
  {
    distractionNode = null;
    isDistracted = false;
  }
}
