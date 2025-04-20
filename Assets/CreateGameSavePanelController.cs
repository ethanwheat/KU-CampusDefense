using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CreateGameSavePanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameSaveNameInput;

    public void CreateGameSave()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = gameSaveNameInput.text + "_" + timestamp + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        GameData gameData = new GameData
        {
            roundNumber = 1,
            dollars = 100,
            purchasableData = new List<PurchasableData>
            {
                new PurchasableData
                {
                    objectName = "Barrier",
                    bought = true
                }
            },
        };

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
    }
}
