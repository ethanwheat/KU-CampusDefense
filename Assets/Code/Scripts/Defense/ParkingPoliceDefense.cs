using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingPoliceDefense : Defense, IDefense
{
    [Header("Ticket Settings")]
    [SerializeField] private float ticketDelay = 1.0f; // time before sending enemy back
    [SerializeField] private float blockDuration = 2.0f; // how long they’re blocked total
    [SerializeField] private int coinReward = 10;

    private IEnumerator TicketAndSendBack(EnemyMovement enemy)
    {
        enemy.BlockMovement(true); // Stop them
        RoundManager.instance.AddCoins(coinReward);
        RoundSceneCanvasController.instance.UpdateCoinUI();
        yield return new WaitForSeconds(ticketDelay);

        // Try to send them back
        if (enemy.currentNode != null)
        {
            var prev = enemy.PreviousNode;
            if (prev != null)
            {
                enemy.currentNode = prev;
            }
        }

        yield return new WaitForSeconds(blockDuration - ticketDelay);

        enemy.BlockMovement(false);
    }

    public void ApplyEffect(EnemyMovement enemy)
    {
        StartCoroutine(TicketAndSendBack(enemy));
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // No ongoing effect to remove
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}
