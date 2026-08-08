using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public int level;
    public int exp;
    public int gold;
    public float healthPoints;
    public List<string> inventoryItems;
    public List<string> completedQuests;
    public int playtime;
    public string timestamp;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string savePath;
    private List<SaveData> saveSlots = new List<SaveData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        savePath = Application.persistentDataPath + "/Saves/";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
    }

    public bool SaveGame(int slotNumber)
    {
        try
        {
            SaveData data = new SaveData
            {
                playerName = "Player",
                level = EconomySystem.Instance.GetLevel(),
                exp = (int)EconomySystem.Instance.GetExperiencePoints(),
                gold = (int)EconomySystem.Instance.GetGold(),
                healthPoints = 100,
                inventoryItems = new List<string>(InventorySystem.Instance.GetInventory().Keys),
                completedQuests = new List<string>(),
                playtime = (int)Time.timeSinceLevelLoad,
                timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string filePath = savePath + "save_" + slotNumber + ".json";
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            
            Debug.Log($"✅ Игра сохранена в слот {slotNumber}! Время: {data.timestamp}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Ошибка сохранения: {e.Message}");
            return false;
        }
    }

    public bool LoadGame(int slotNumber)
    {
        try
        {
            string filePath = savePath + "save_" + slotNumber + ".json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                
                EconomySystem.Instance.SetGold(data.gold);
                Debug.Log($"✅ Игра загружена из слота {slotNumber}! Сохранено: {data.timestamp}");
                return true;
            }
            else
            {
                Debug.LogWarning($"⚠️ Слот сохранения {slotNumber} не найден!");
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Ошибка загрузки: {e.Message}");
            return false;
        }
    }

    public List<SaveData> GetAllSaves()
    {
        saveSlots.Clear();
        for (int i = 1; i <= 5; i++)
        {
            string filePath = savePath + "save_" + i + ".json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                saveSlots.Add(data);
            }
        }
        return saveSlots;
    }
}
