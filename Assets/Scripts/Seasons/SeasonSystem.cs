using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SeasonEvent
{
    public string eventName;
    public Season season;
    public float rewardMultiplier;
    public string description;
}

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class SeasonSystem : MonoBehaviour
{
    public static SeasonSystem Instance { get; private set; }

    private Season currentSeason = Season.Spring;
    private float seasonProgress = 0f; // 0-100
    private float daysPerSeason = 30f;
    private float currentDay = 0f;
    private Dictionary<Season, SeasonEvent> seasonEvents = new Dictionary<Season, SeasonEvent>();

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
        InitializeSeasonEvents();
    }

    private void InitializeSeasonEvents()
    {
        seasonEvents[Season.Spring] = new SeasonEvent
        {
            eventName = "Весна",
            season = Season.Spring,
            rewardMultiplier = 1.2f,
            description = "Время новых начинаний! +20% к награда́м"
        };

        seasonEvents[Season.Summer] = new SeasonEvent
        {
            eventName = "Лето",
            season = Season.Summer,
            rewardMultiplier = 1.0f,
            description = "Обычное время. Нет бонусов"
        };

        seasonEvents[Season.Fall] = new SeasonEvent
        {
            eventName = "Осень",
            season = Season.Fall,
            rewardMultiplier = 1.3f,
            description = "Урожайное время! +30% к добыче ресурсов"
        };

        seasonEvents[Season.Winter] = new SeasonEvent
        {
            eventName = "Зима",
            season = Season.Winter,
            rewardMultiplier = 0.8f,
            description = "Суровое время. -20% к награда́м"
        };
    }

    private void Update()
    {
        // Прогресс времени (для дема меняется быстрее)
        currentDay += Time.deltaTime / 10f;
        seasonProgress = (currentDay % daysPerSeason) / daysPerSeason * 100f;

        if (currentDay >= daysPerSeason)
        {
            AdvanceSeason();
            currentDay = 0f;
        }
    }

    private void AdvanceSeason()
    {
        int nextSeason = (int)currentSeason + 1;
        currentSeason = (Season)(nextSeason % 4);
        Debug.Log($"🌱 Сезон изменился на: {currentSeason}");
        Debug.Log($"📊 Бонус: {seasonEvents[currentSeason].rewardMultiplier}x");
    }

    public Season GetCurrentSeason() => currentSeason;
    public float GetSeasonProgress() => seasonProgress;
    public SeasonEvent GetCurrentSeasonEvent() => seasonEvents.TryGetValue(currentSeason, out var evt) ? evt : null;
    public float GetRewardMultiplier() => seasonEvents[currentSeason].rewardMultiplier;
}
