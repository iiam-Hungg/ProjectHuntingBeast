using Cinemachine;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
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
    }
}
