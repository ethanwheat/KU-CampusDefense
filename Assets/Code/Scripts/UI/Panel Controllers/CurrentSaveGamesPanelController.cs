using System.Collections.Generic;
using UnityEngine;

public class CurrentSaveGamesPanelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject gameSaveButtonGroupPrefab;

    [Header("Transforms")]
    [SerializeField] private Transform content;

    [Header("UI Controllers")]
    [SerializeField] private PanelFadeController panelFadeController;
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Sounds")]
    [SerializeField] private AudioClip errorSoundEffect;

    // Populate panel with game saves.
    public void ShowPanel()
    {
        ResetPanel();

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

        panelFadeController.Show();
    }

    // Remove previous game saves.
    void ResetPanel()
    {
        foreach (Transform buttonGroup in content)
        {
            Destroy(buttonGroup.gameObject);
        }
    }

    // Load game data and start game.
    private void StartGame(GameDataMeta gameDataMeta)
    {
        if (GameDataManager.instance.LoadGameData(gameDataMeta.Guid))
        {
            MainMenuSceneUIController.instance.StartGame();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Something went wrong!", "The game could not be started.");
        }
    }

    // Delete game save and remove button.
    private void DeleteSave(GameObject buttonGroup, GameDataMeta gameDataMeta)
    {
        if (GameDataManager.instance.DeleteGameData(gameDataMeta.Guid))
        {
            Destroy(buttonGroup);
        }
        else
        {
            SoundManager.instance.PlaySoundEffect(errorSoundEffect, transform, 1f);
            messagePopupPanelController.ShowPanel("Something went wrong!", "The game save could not be deleted.");
        }
    }
}
