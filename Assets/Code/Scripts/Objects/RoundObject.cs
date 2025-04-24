using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private int fans;
    [SerializeField] private int coaches;
    [SerializeField] private int cheerleaders;
    [SerializeField] private int players;
    [SerializeField] private int mascots;
    [SerializeField] private float spawnInterval = 1f; // Time between spawns

    public int Fans => fans;
    public int Coaches => coaches;
    public int Cheerleaders => cheerleaders;
    public int Players => players;
    public int Mascots => mascots;
    public float SpawnInterval => spawnInterval;
}

[CreateAssetMenu(fileName = "NewRound", menuName = "Round Configuration")]
public class RoundObject : ScriptableObject
{
    [SerializeField] private Wave[] waves;  // Array of waves
    [SerializeField] private int roundNumber;
    [SerializeField] private float fieldHouseHealth;
    [SerializeField] private int winPayout;
    [SerializeField] private int killPayout;

    public Wave[] Waves => waves;
    public int RoundNumber => roundNumber;
    public float FieldHouseHealth => fieldHouseHealth;
    public int WinPayout => winPayout;
    public int KillPayout => killPayout;
}
