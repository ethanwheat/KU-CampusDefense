using System.Collections;
using UnityEngine;

public class BaconatorStandDefense : Defense, IDefense
{
    [Header("Baconator Settings")]
    [SerializeField] private float distractionDuration = 3f; // How long enemies are distracted
    [SerializeField] private PathNode distractionNode; // Where enemies should go

    public void ApplyEffect(EnemyMovement enemy)
    {
        enemy.SetDistraction(distractionNode);
        StartCoroutine(ClearDistractionAfterDelay(enemy));
    }

    public void RemoveEffect(EnemyMovement enemy)
    {
        // Nothing needed; enemies will clear distraction themselves after reaching the node
    }

    private IEnumerator ClearDistractionAfterDelay(EnemyMovement enemy)
    {
        yield return new WaitForSeconds(distractionDuration);

        enemy.ClearDistraction();
    }

    public override void OnDefenseDestroy()
    {
        base.OnDefenseDestroy();
    }
}
