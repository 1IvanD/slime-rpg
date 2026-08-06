using UnityEngine;
using System.Collections.Generic;

public class AutoFarmSystem : MonoBehaviour
{
    public static AutoFarmSystem Instance { get; private set; }

    private bool isAutoFarmActive = false;
    private float autoFarmTimer = 0f;
    private float autoFarmInterval = 5f; // Каждые 5 секунд
    private float offlineGoldPerSecond = 10f;
    private float offlineTimeMultiplier = 1.5f;
    private float lastSessionTime = 0f;

    [System.Serializable]
    public class AutoFarmTask
    {
        public string taskName;
        public float goldPerHour;
        public bool isActive;
    }

    private List<AutoFarmTask> autoFarmTasks = new List<AutoFarmTask>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeAutoFarmTasks();
        CalculateOfflineGains();
    }

    private void InitializeAutoFarmTasks()
    {
        autoFarmTasks.Add(new AutoFarmTask { taskName = "Фарм враго́в", goldPerHour = 500, isActive = false });
        autoFarmTasks.Add(new AutoFarmTask { taskName = "Добыча ресурсов", goldPerHour = 300, isActive = false });
        autoFarmTasks.Add(new AutoFarmTask { taskName = "Торговля", goldPerHour = 1000, isActive = false });
        autoFarmTasks.Add(new AutoFarmTask { taskName = "Крафт предметов", goldPerHour = 700, isActive = false });
    }

    private void CalculateOfflineGains()
    {
        lastSessionTime = PlayerPrefs.GetFloat("LastSessionTime", 0);
        float currentTime = Time.realtimeSinceStartup;
        float offlineTime = currentTime - lastSessionTime;

        if (offlineTime > 60) // Если был оффлайн более 1 минуты
        {
            float offlineGains = (offlineTime / 3600f) * offlineGoldPerSecond * offlineTimeMultiplier;
            EconomySystem.Instance.AddGold(offlineGains);
            Debug.Log($"💤 АФК награда: +{offlineGains} золота (был оффлайн {offlineTime} сек)");
        }
    }

    private void Update()
    {
        if (!isAutoFarmActive) return;

        autoFarmTimer += Time.deltaTime;
        if (autoFarmTimer >= autoFarmInterval)
        {
            PerformAutoFarm();
            autoFarmTimer = 0f;
        }
    }

    private void PerformAutoFarm()
    {
        float totalGold = 0f;
        foreach (AutoFarmTask task in autoFarmTasks)
        {
            if (task.isActive)
            {
                float goldThisInterval = (task.goldPerHour / 3600f) * autoFarmInterval;
                totalGold += goldThisInterval;
            }
        }

        if (totalGold > 0)
        {
            EconomySystem.Instance.AddGold(totalGold);
            Debug.Log($"🤖 АФК награда: +{totalGold:F1} золота");
        }
    }

    public void ToggleAutoFarm()
    {
        isAutoFarmActive = !isAutoFarmActive;
        Debug.Log($"🤖 АФК {(isAutoFarmActive ? "включен" : "отключен")}");
    }

    public void ToggleAutoFarmTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < autoFarmTasks.Count)
        {
            autoFarmTasks[taskIndex].isActive = !autoFarmTasks[taskIndex].isActive;
            Debug.Log($"✅ {autoFarmTasks[taskIndex].taskName} {(autoFarmTasks[taskIndex].isActive ? "включена" : "отключена")}");
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetFloat("LastSessionTime", Time.realtimeSinceStartup);
        }
        else
        {
            CalculateOfflineGains();
        }
    }

    public List<AutoFarmTask> GetAutoFarmTasks() => autoFarmTasks;
    public bool IsAutoFarmActive() => isAutoFarmActive;
}
