using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  public PathNode currentNode; // The waypoint the object is moving toward
  [SerializeField] private float speed;
  [SerializeField] private float health;
  [SerializeField] private HealthBar healthBar; // Reference to the HealthBar script

  [Header("Sounds")]
  [SerializeField] private AudioClip enemyKilledSoundEffect;

  private RoundManager roundManager;
  private float baseSpeed;
  private float maxHealth;
  private bool isBlocked = false;
  private bool isDead = false;
  private int killReward;

  public float Health => health; // read only access

  void Start()
  {
    roundManager = RoundManager.instance;
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
      var nextNode = currentNode.GetNextNode();
      if (nextNode != null)
      {
        currentNode = currentNode.GetNextNode();
      }
      else  // enemy has reached Allen fieldhouse
      {

        if (!isDead)
        {
          isDead = true;

          roundManager.EnemyDefeated(this);
          roundManager.DamageFieldhouse(health);
        }

        Destroy(gameObject);
      }
    }
  }

  // defense reactions
  private void OnTriggerEnter(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
    if (!placementController.Placed) return;

    if (other.TryGetComponent(out IDefense defenseEffect))
    {
      defenseEffect.ApplyEffect(this);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
    if (!placementController.Placed) return;

    if (other.TryGetComponent(out IDefense defenseEffect))
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
    if (!isDead)
    {
      isDead = true;

      roundManager.EnemyDefeated(this);
      roundManager.AddCoins(killReward);
    }
    SoundManager.instance.PlaySoundEffect(enemyKilledSoundEffect, transform);
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
