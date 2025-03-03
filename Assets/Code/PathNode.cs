using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode[] nextNodes; // possible next waypoints 

    public PathNode GetNextNode()
    {
      if (nextNodes.Length == 0) return null;
      return nextNodes[Random.Range(0, nextNodes.Length)]; // choose randomly
    }
}
