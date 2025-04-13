using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    roundManager = GameObject.Find("RoundManager").GetComponent<RoundManager>();
  }

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
  private int killReward;

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
      else  // enemy has reached Allen fieldhouse
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

  // defense reactions
  private void OnTriggerEnter(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
    if (!placementController.isPlaced()) return;

    if (other.TryGetComponent(out IDefense defenseEffect))
    {
      defenseEffect.ApplyEffect(this);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.TryGetComponent(out DefensePlacementController placementController)) return;
    if (!placementController.isPlaced()) return;

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
    //Debug.Log("Enemy took damage. Current Health: " + health);
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
    SoundManager.instance.playSoundEffect(enemyKilledSoundEffect, transform, .5f);
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
