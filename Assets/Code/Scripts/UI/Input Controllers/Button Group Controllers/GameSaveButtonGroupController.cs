using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameSaveButtonGroupController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI saveText;
    [SerializeField] private TextMeshProUGUI lastModifiedText;

    [Header("Unity Events")]
    public UnityEvent OnLoadGame;
    public UnityEvent OnDeleteSave;

    public void SetData(GameDataMeta meta)
    {
        saveText.text = meta.Name;

        if (DateTime.TryParse(meta.LastModified, out DateTime parsedTime))
        {
            lastModifiedText.text = parsedTime.ToLocalTime().ToString("M/d/yyyy h:mm tt");
        }
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
