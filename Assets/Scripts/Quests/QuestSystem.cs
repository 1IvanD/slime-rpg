using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestData
{
    public string questId;
    public string questName;
    public string description;
    public QuestType questType;
    public QuestStatus status;
    public int reward_gold;
    public int reward_exp;
    public float progress;
    public float progressMax;
    public string objective;
}

public enum QuestType
{
    Main,      // Основные квесты
    Side,      // Побочные квесты
    Daily      // Ежедневные квесты
}

public enum QuestStatus
{
    Available,
    Active,
    Completed,
    Failed
}

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance { get; private set; }

    private Dictionary<string, QuestData> allQuests = new Dictionary<string, QuestData>();
    private List<QuestData> activeQuests = new List<QuestData>();
    private List<QuestData> completedQuests = new List<QuestData>();
    private QuestData currentMainQuest;

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
        InitializeQuests();
    }

    private void InitializeQuests()
    {
        // Основные квесты (Main)
        CreateQuest("main_1", "Встреча с Риммуру", "Найди и встреться с Риммуру в его деревне", 
            QuestType.Main, 500, 200, 0, 1, "Дойти до деревни Риммуру");
        
        CreateQuest("main_2", "Первая битва", "Одолей лордов гоблинов", 
            QuestType.Main, 1000, 500, 0, 1, "Победить 5 гоблинов");
        
        CreateQuest("main_3", "Король демонов", "Сразись с Люцифером", 
            QuestType.Main, 5000, 2000, 0, 1, "Победить Люцифера");
        
        // Побочные квесты (Side)
        CreateQuest("side_1", "Охотник на зелья", "Собери 10 редких трав", 
            QuestType.Side, 200, 100, 0, 10, "Собрано трав: 0/10");
        
        CreateQuest("side_2", "Добытчик золота", "Заработай 500 золота", 
            QuestType.Side, 100, 50, 0, 500, "Золото: 0/500");
        
        CreateQuest("side_3", "Покоритель подземелий", "Пройди 5 подземелий", 
            QuestType.Side, 300, 150, 0, 5, "Подземелий пройдено: 0/5");
        
        // Ежедневные квесты (Daily)
        CreateQuest("daily_1", "Ежедневное убийство", "Победи 10 врагов", 
            QuestType.Daily, 100, 50, 0, 10, "Враги побеждены: 0/10");
        
        CreateQuest("daily_2", "Сборщик ресурсов", "Собери 5 ресурсов", 
            QuestType.Daily, 50, 25, 0, 5, "Ресурсов собрано: 0/5");
        
        CreateQuest("daily_3", "Торговец", "Торгуй с 3 поселениями", 
            QuestType.Daily, 150, 75, 0, 3, "Торговли: 0/3");
    }

    private void CreateQuest(string id, string name, string desc, QuestType type, 
        int gold, int exp, float progress, float maxProgress, string objective)
    {
        QuestData quest = new QuestData
        {
            questId = id,
            questName = name,
            description = desc,
            questType = type,
            status = QuestStatus.Available,
            reward_gold = gold,
            reward_exp = exp,
            progress = progress,
            progressMax = maxProgress,
            objective = objective
        };
        
        allQuests[id] = quest;
    }

    public void AcceptQuest(string questId)
    {
        if (allQuests.TryGetValue(questId, out QuestData quest))
        {
            if (quest.status == QuestStatus.Available)
            {
                quest.status = QuestStatus.Active;
                activeQuests.Add(quest);
                
                if (quest.questType == QuestType.Main)
                {
                    currentMainQuest = quest;
                }
                
                Debug.Log($"Квест принят: {quest.questName}");
            }
        }
    }

    public void UpdateQuestProgress(string questId, float amount)
    {
        if (allQuests.TryGetValue(questId, out QuestData quest))
        {
            quest.progress += amount;
            
            if (quest.progress >= quest.progressMax)
            {
                CompleteQuest(questId);
            }
        }
    }

    public void CompleteQuest(string questId)
    {
        if (allQuests.TryGetValue(questId, out QuestData quest))
        {
            if (quest.status == QuestStatus.Active)
            {
                quest.status = QuestStatus.Completed;
                quest.progress = quest.progressMax;
                activeQuests.Remove(quest);
                completedQuests.Add(quest);
                
                // Выдать награды
                EconomySystem.Instance.AddGold(quest.reward_gold);
                EconomySystem.Instance.AddExperiencePoints(quest.reward_exp);
                
                Debug.Log($"Квест завершён: {quest.questName}! +{quest.reward_gold} золота, +{quest.reward_exp} опыта");
            }
        }
    }

    public void FailQuest(string questId)
    {
        if (allQuests.TryGetValue(questId, out QuestData quest))
        {
            if (quest.status == QuestStatus.Active)
            {
                quest.status = QuestStatus.Failed;
                activeQuests.Remove(quest);
                Debug.Log($"Квест провален: {quest.questName}");
            }
        }
    }

    public QuestData GetQuest(string questId) => allQuests.TryGetValue(questId, out var quest) ? quest : null;
    public List<QuestData> GetActiveQuests() => activeQuests;
    public List<QuestData> GetCompletedQuests() => completedQuests;
    public QuestData GetCurrentMainQuest() => currentMainQuest;
    public Dictionary<string, QuestData> GetAllQuests() => allQuests;
}
