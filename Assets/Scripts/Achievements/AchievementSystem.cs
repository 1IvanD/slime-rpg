using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Achievement
{
    public string achievementId;
    public string title;
    public string description;
    public bool isUnlocked;
    public int rewardGold;
    public Sprite icon; // Для будущего использования
}

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance { get; private set; }

    private Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
    private int totalAchievementsCount = 0;
    private int unlockedCount = 0;

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
        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        // Достижения боевой системы
        CreateAchievement("ach_first_kill", "Первая кровь", "Победи первого врага", 100);
        CreateAchievement("ach_combo_x10", "Комбо х10", "Достигни комбо х10", 250);
        CreateAchievement("ach_defeat_boss", "Убийца босса", "Победи босса подземелья", 500);

        // Достижения исследования
        CreateAchievement("ach_explore_5", "Исследователь", "Открой 5 локаций", 200);
        CreateAchievement("ach_explore_all", "Картограф", "Открой все локации", 1000);

        // Достижения инвентаря
        CreateAchievement("ach_collector", "Коллекционер", "Собери 20 разных предметов", 300);
        CreateAchievement("ach_rich", "Богач", "Накопи 5000 золота", 400);

        // Достижения магии
        CreateAchievement("ach_mage", "Маг-новичок", "Выучи 5 заклинаний", 350);
        CreateAchievement("ach_archmage", "Архимаг", "Выучи 15 заклинаний", 800);

        // Достижения умений
        CreateAchievement("ach_fisherman", "Рыбак", "Поймай 10 рыб", 200);
        CreateAchievement("ach_master_crafter", "Мастер-ремесленник", "Повысь все умения до уровня 5", 1000);

        totalAchievementsCount = achievements.Count;
    }

    private void CreateAchievement(string id, string title, string desc, int reward)
    {
        Achievement achievement = new Achievement
        {
            achievementId = id,
            title = title,
            description = desc,
            isUnlocked = false,
            rewardGold = reward
        };

        achievements[id] = achievement;
    }

    public void UnlockAchievement(string achievementId)
    {
        if (achievements.TryGetValue(achievementId, out Achievement achievement))
        {
            if (!achievement.isUnlocked)
            {
                achievement.isUnlocked = true;
                unlockedCount++;
                EconomySystem.Instance.AddGold(achievement.rewardGold);
                Debug.Log($"Достижение разблокировано: {achievement.title}! +{achievement.rewardGold} золота");
            }
        }
    }

    public Achievement GetAchievement(string achievementId) => achievements.TryGetValue(achievementId, out var ach) ? ach : null;
    public Dictionary<string, Achievement> GetAllAchievements() => achievements;
    public int GetUnlockedCount() => unlockedCount;
    public int GetTotalCount() => totalAchievementsCount;
    public float GetProgress() => totalAchievementsCount > 0 ? (float)unlockedCount / totalAchievementsCount * 100 : 0;
}
