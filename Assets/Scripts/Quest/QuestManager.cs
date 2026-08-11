using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private Dictionary<string, QuestDef> allQuests = new Dictionary<string, QuestDef>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadQuests();
    }

    private void LoadQuests()
    {
        var defs = Resources.LoadAll<QuestDef>("Quests");
        allQuests.Clear();
        foreach (var q in defs)
        {
            allQuests[q.id] = q;
        }
        Debug.Log($"QuestManager: loaded {allQuests.Count} quests.");
    }

    public QuestDef GetQuest(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        allQuests.TryGetValue(id, out var q);
        return q;
    }

    public bool IsQuestCompleted(string id)
    {
        var q = GetQuest(id);
        if (q == null) return false;
        return q.IsCompleted();
    }

    public bool CanStartQuest(string id)
    {
        var q = GetQuest(id);
        if (q == null) return false;
        if (q.status == QuestDef.QuestStatus.Completed) return false;
        if (string.IsNullOrEmpty(q.prerequisiteQuestId)) return true;
        return IsQuestCompleted(q.prerequisiteQuestId);
    }

    public bool StartQuest(string id)
    {
        var q = GetQuest(id);
        if (q == null) return false;
        if (!CanStartQuest(id)) return false;
        q.status = QuestDef.QuestStatus.Active;
        WorldMapUI.Instance?.ShowNotification($"Quest started: {q.displayName}");
        QuestUI.Instance?.OnQuestUpdated(q);
        return true;
    }

    public bool CompleteObjective(string questId, int objectiveIndex)
    {
        var q = GetQuest(questId);
        if (q == null) return false;
        if (objectiveIndex < 0 || objectiveIndex >= q.objectives.Count) return false;
        q.objectives[objectiveIndex].completed = true;

        if (q.IsCompleted())
        {
            q.status = QuestDef.QuestStatus.Completed;
            WorldMapUI.Instance?.ShowNotification($"Quest completed: {q.displayName}");
            QuestUI.Instance?.OnQuestUpdated(q);

            // Auto-activate quests that list this as prerequisite
            foreach (var other in GetAllQuests())
            {
                if (other.status == QuestDef.QuestStatus.Locked && other.prerequisiteQuestId == q.id)
                {
                    if (CanStartQuest(other.id))
                    {
                        StartQuest(other.id);
                    }
                }
            }

            return true;
        }
        else
        {
            QuestUI.Instance?.OnQuestUpdated(q);
        }
        return false;
    }

    public List<QuestDef> GetActiveQuests()
    {
        var list = new List<QuestDef>();
        foreach (var q in allQuests.Values)
        {
            if (q.status == QuestDef.QuestStatus.Active) list.Add(q);
        }
        return list;
    }

    public List<QuestDef> GetAllQuests()
    {
        return new List<QuestDef>(allQuests.Values);
    }
}
