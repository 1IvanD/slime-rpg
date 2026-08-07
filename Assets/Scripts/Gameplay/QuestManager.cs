using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, string> quests = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddQuest(string id, string description)
    {
        if (!quests.ContainsKey(id))
        {
            quests[id] = description;
            UIController.GetInstance()?.ShowNotification($"Квест получен: {description}");
        }
    }

    public void CompleteQuest(string id)
    {
        if (quests.ContainsKey(id))
        {
            UIController.GetInstance()?.ShowNotification($"Квест выполнен: {quests[id]}");
            quests.Remove(id);
        }
    }

    public bool HasQuest(string id) => quests.ContainsKey(id);
}
