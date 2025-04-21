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
    [SerializeField] public UnityEvent OnLoadGame;
    [SerializeField] public UnityEvent OnDeleteSave;

    public void SetData(GameDataMeta meta)
    {
        saveText.text = meta.Name;

        DateTime parsedTimed = DateTime.Parse(meta.CreationTime);
        DateTime localTime = parsedTimed.ToLocalTime();
        string formattedTime = localTime.ToString("M/d/yyyy h:mm tt");

        creationDateText.text = formattedTime;
    }

    public void LoadGame()
    {
        OnLoadGame.Invoke();
    }

    public void DeleteSave()
    {
        OnDeleteSave.Invoke();
    }
}
