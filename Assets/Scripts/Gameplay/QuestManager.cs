using System.Collections.Generic;
using UnityEngine;

// Legacy/compat QuestManager kept for historical scenes/tools. Renamed to avoid collision with runtime QuestManager.
public class QuestManager_Legacy : MonoBehaviour
{
    public static QuestManager_Legacy Instance;

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
