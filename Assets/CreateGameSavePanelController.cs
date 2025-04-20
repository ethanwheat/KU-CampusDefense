using System.IO;
using TMPro;
using UnityEngine;

public class CreateGameSavePanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveNameInput;

    public void CreateGameSave()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = saveNameInput.text + "_" + timestamp + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        Debug.Log(filePath);

        GameData gameData = new GameData
        {
            roundNumber = 1,
            dollars = 100,
        };

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
    }
}
