using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Quest/Quest", fileName = "QuestDef")]
public class QuestDef : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public enum QuestStatus { Locked, Active, Completed }
    [HideInInspector] public QuestStatus status = QuestStatus.Locked;

    [System.Serializable]
    public class Objective
    {
        public string description;
        public bool completed = false;
    }

    public List<Objective> objectives = new List<Objective>();

    [Tooltip("Optional id of quest required to be completed before this quest can start")]
    public string prerequisiteQuestId;

    [Tooltip("Optional MapNode id associated with the quest (used by QuestUI Go button)")]
    public string associatedNodeId;

    // helper
    public bool IsCompleted()
    {
        foreach (var o in objectives)
        {
            if (!o.completed) return false;
        }
        return true;
    }
}
