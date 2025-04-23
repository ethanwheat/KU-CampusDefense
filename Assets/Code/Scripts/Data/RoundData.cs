using UnityEngine;

[System.Serializable]
public class Wave
{
    public int fans;
    public int coaches;
    public int cheerleaders;
    public int players;
    public int mascots;
    public float spawnInterval = 1f; // Time between spawns
}

[CreateAssetMenu(fileName = "NewRound", menuName = "Round Configuration")]
public class RoundData : ScriptableObject
{
    public Wave[] waves;  // Array of waves
    public int roundNumber;
    public float fieldhouseHealth;
    public int winPayout;
    public int killPayout;

}
