using Cinemachine;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SaveController : MonoBehaviour
{
    private string saveFolder;

    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Transform loadListContainer;
    [SerializeField] private GameObject loadListItemPrefab;

    private void Start()
    {
        saveFolder = Path.Combine(Application.persistentDataPath, "Saves");

        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);


        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGame);

        if (loadButton != null)
            loadButton.onClick.AddListener(ShowLoadList);
        string testPath = Path.Combine(Application.persistentDataPath, "testSave.txt");
        File.WriteAllText(testPath, "Test file created.");
        Debug.Log("Test file written at: " + testPath);
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        CinemachineConfiner confiner = FindFirstObjectByType<CinemachineConfiner>();
        if (confiner == null || confiner.m_BoundingShape2D == null)
        {
            Debug.LogError("Cinemachine Confiner or Bounding Shape not found!");
            return;
        }

        SaveData saveData = new SaveData
        {
            playerPos = player.transform.position,
            mapBound = confiner.m_BoundingShape2D.gameObject.name
        };

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string savePath = Path.Combine(saveFolder, $"save_{timestamp}.json");

        File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
        Debug.Log($"Game saved: {savePath}");
    }

    public void ShowLoadList()
    {
        foreach (Transform child in loadListContainer)
            Destroy(child.gameObject);

        string[] saveFiles = Directory.GetFiles(saveFolder, "save_*.json");
        foreach (string saveFile in saveFiles)
        {
            GameObject listItem = Instantiate(loadListItemPrefab, loadListContainer);
            listItem.GetComponentInChildren<Text>().text = Path.GetFileName(saveFile); 

            Button btn = listItem.GetComponent<Button>();
            btn.onClick.AddListener(() => LoadGame(saveFile));
        }
    }

    public void LoadGame(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Save file not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        player.transform.position = saveData.playerPos;

        CinemachineConfiner confiner = FindFirstObjectByType<CinemachineConfiner>();
        if (confiner == null)
        {
            Debug.LogError("Cinemachine Confiner not found!");
            return;
        }

        GameObject mapObject = GameObject.Find(saveData.mapBound);
        if (mapObject == null)
        {
            Debug.LogError($"Map Bound object '{saveData.mapBound}' not found!");
            return;
        }

        Collider2D collider = mapObject.GetComponent<PolygonCollider2D>();
        if (collider == null)
        {
            Debug.LogError($"PolygonCollider2D missing on {saveData.mapBound}");
            return;
        }

        confiner.m_BoundingShape2D = collider;
        Debug.Log($"Game loaded from: {filePath}");
    }
    /*private string saveLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveFile.json");
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBound =Object.FindFirstObjectByType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation)) {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPos;
            Object.FindFirstObjectByType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBound).GetComponent<PolygonCollider2D>();
        }
        else
        {
            SaveGame();
        }
    }*/
}
