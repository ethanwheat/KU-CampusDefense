using UnityEngine;
using System.Collections.Generic;

public class RoadBlockDefense : Defense, IDefense
{
    [Header("Fork Reference")]
    [SerializeField] private PathNode forkSouth;

    private PathNode removedNode = null;

    public override void Start()
    {
        base.Start();
        RemoveSecondOptionFromSouthFork();
    }

    private void RemoveSecondOptionFromSouthFork()
    {
        if (forkSouth == null || forkSouth.nextNodes == null || forkSouth.nextNodes.Length < 2)
        {
            Debug.LogWarning("Cannot remove second option from forkSouth. Not enough paths.");
            return;
        }

        // Save the node to be able to restore later
        removedNode = forkSouth.nextNodes[1];

        // Create a new array excluding the second option
        List<PathNode> updated = new List<PathNode>(forkSouth.nextNodes);
        updated.RemoveAt(1);
        forkSouth.nextNodes = updated.ToArray();
    }

    public override void OnDefenseDestroy()
    {
        // Restore the second option
        if (removedNode != null && forkSouth != null)
        {
            List<PathNode> updated = new List<PathNode>(forkSouth.nextNodes);

            // Reinsert at index 1 if it's still valid
            if (!updated.Contains(removedNode))
            {
                if (updated.Count >= 1)
                    updated.Insert(1, removedNode);
                else
                    updated.Add(removedNode);

                forkSouth.nextNodes = updated.ToArray();
            }
        }

        base.OnDefenseDestroy();
    }

    public void ApplyEffect(EnemyMovement enemy) { }
    public void RemoveEffect(EnemyMovement enemy) { }
}

