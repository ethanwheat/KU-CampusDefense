using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameSaveButtonGroupController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI saveText;
    [SerializeField] private TextMeshProUGUI creationDateText;

    [Header("Unity Events")]
    [SerializeField] public UnityEvent onLoadGame;
    [SerializeField] public UnityEvent onDeleteSave;

    public void SetData(GameSave gameSave)
    {
        saveText.text = gameSave.name;

        DateTime parsedTimed = DateTime.Parse(gameSave.creationTime);
        DateTime localTime = parsedTimed.ToLocalTime();
        string formattedTime = localTime.ToString("M/d/yyyy h:mm tt");

        creationDateText.text = formattedTime;
    }

    public void LoadGame()
    {
        onLoadGame.Invoke();
    }

    public void DeleteSave()
    {
        onDeleteSave.Invoke();
    }
}
