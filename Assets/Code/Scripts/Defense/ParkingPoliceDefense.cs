using System.Collections;
using UnityEngine;

public class ParkingPoliceDefense : Defense, IDefense
{
    [Header("Ticket Settings")]
    [SerializeField] private float ticketDelay = 1.0f; // time before sending enemy back
    [SerializeField] private float blockDuration = 2.0f; // how long they’re blocked total

    public void ApplyEffect(EnemyMovement enemy)
    {
        if (enemy != null)
        {
            StartCoroutine(TicketAndSendBack(enemy));
        }
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // No ongoing effect to remove
    }

    private IEnumerator TicketAndSendBack(EnemyMovement enemy)
    {
        enemy.BlockMovement(true); // Stop them
        yield return new WaitForSeconds(ticketDelay);

        // Try to send them back
        if (enemy != null && enemy.currentNode != null)
        {
            var prev = GetPreviousNode(enemy);
            if (prev != null)
            {
                enemy.currentNode = prev;
            }
        }

        yield return new WaitForSeconds(blockDuration - ticketDelay);
        if (enemy != null)
        {
            enemy.BlockMovement(false);
        }
    }

    private PathNode GetPreviousNode(EnemyMovement enemy)
    {
        var field = typeof(EnemyMovement).GetField("previousNode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field?.GetValue(enemy) as PathNode;
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}
