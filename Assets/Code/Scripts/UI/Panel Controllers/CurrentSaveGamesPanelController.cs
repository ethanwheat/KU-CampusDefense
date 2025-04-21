using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentSaveGamesPanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject gameSaveButtonGroupPrefab;

    [Header("Transforms")]
    [SerializeField] private Transform content;

    [Header("UI Controllers")]
    [SerializeField] private MainMenuUIController mainMenuUIController;

    void OnEnable()
    {
        List<GameSave> gameSaves = GameSaveController.GetGameSaveList().GameSaves;

        foreach (var gameSave in gameSaves)
        {
            GameObject buttonGroup = Instantiate(gameSaveButtonGroupPrefab, content);
            GameSaveButtonGroupController gameSaveButtonGroupController = buttonGroup.GetComponent<GameSaveButtonGroupController>();
            gameSaveButtonGroupController.SetData(gameSave);

            GameData gameData = GameDataController.GetGameData(gameSave.Id);
            gameSaveButtonGroupController.onLoadGame.AddListener(() => mainMenuUIController.StartGame(gameData));
            gameSaveButtonGroupController.onDeleteSave.AddListener(() => DeleteSave(buttonGroup, gameSave));
        }
    }

    void OnDisable()
    {
        foreach (Transform buttonGroup in content)
        {
            Destroy(buttonGroup.gameObject);
        }
    }

    private void DeleteSave(GameObject buttonGroup, GameSave gameSave)
    {
        GameSaveController.DeleteGameSave(gameSave.Id);
        Destroy(buttonGroup);
    }
}
