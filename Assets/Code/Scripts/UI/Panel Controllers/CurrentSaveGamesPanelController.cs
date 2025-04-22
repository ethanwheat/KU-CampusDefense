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
        GameDataManager gameDataManager = GameDataManager.instance;
        List<GameDataMeta> gameDataMeta = gameDataManager.GetAllGameMetaData();

        foreach (var meta in gameDataMeta)
        {
            GameObject buttonGroup = Instantiate(gameSaveButtonGroupPrefab, content);
            GameSaveButtonGroupController gameSaveButtonGroupController = buttonGroup.GetComponent<GameSaveButtonGroupController>();
            gameSaveButtonGroupController.SetData(meta);
            gameSaveButtonGroupController.OnLoadGame.AddListener(() => StartGame(meta));
            gameSaveButtonGroupController.OnDeleteSave.AddListener(() => DeleteSave(buttonGroup, meta));
        }
    }

    void OnDisable()
    {
        foreach (Transform buttonGroup in content)
        {
            Destroy(buttonGroup.gameObject);
        }
    }

    private void StartGame(GameDataMeta gameDataMeta)
    {
        if (GameDataManager.instance.LoadGameData(gameDataMeta.Guid))
        {
            mainMenuUIController.StartGame();
        }
        else
        {
            // Show message popup.
        }
    }

    private void DeleteSave(GameObject buttonGroup, GameDataMeta gameDataMeta)
    {
        if (GameDataManager.instance.DeleteGameData(gameDataMeta.Guid))
        {
            Destroy(buttonGroup);
        }
        else
        {
            // Show message popup.
        }
    }
}
