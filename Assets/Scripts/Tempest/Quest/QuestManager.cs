using UnityEngine;
using System.Collections.Generic;

namespace Tempest.Quest
{
    /// <summary>
    /// Quest management system with prerequisites, rewards, and state tracking.
    /// Integrates with dialogue system and UI display.
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        private Dictionary<string, QuestInstance> activeQuests = new Dictionary<string, QuestInstance>();
        private Dictionary<string, QuestDef> questDefinitions = new Dictionary<string, QuestDef>();
        private List<string> completedQuestIds = new List<string>();

        // Events for UI updates
        public delegate void QuestEventHandler(QuestInstance quest);
        public event QuestEventHandler OnQuestAccepted;
        public event QuestEventHandler OnQuestCompleted;
        public event QuestEventHandler OnQuestFailed;
        public event QuestEventHandler OnObjectiveUpdated;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[QuestManager] Initialized");
        }

        #region Quest Lifecycle

        /// <summary>
        /// Accept/start a quest.
        /// Checks prerequisites before accepting.
        /// </summary>
        public bool AcceptQuest(string questId)
        {
            if (!questDefinitions.ContainsKey(questId))
            {
                Debug.LogWarning($"[Quest] Quest definition not found: {questId}");
                return false;
            }

            if (activeQuests.ContainsKey(questId))
            {
                Debug.LogWarning($"[Quest] Quest already active: {questId}");
                return false;
            }

            QuestDef def = questDefinitions[questId];

            // Check prerequisites
            if (!string.IsNullOrEmpty(def.prerequisiteQuestId))
            {
                if (!completedQuestIds.Contains(def.prerequisiteQuestId))
                {
                    Debug.LogWarning($"[Quest] Prerequisite not met for {questId}: requires {def.prerequisiteQuestId}");
                    return false;
                }
            }

            // Create quest instance
            QuestInstance instance = new QuestInstance(def);
            activeQuests[questId] = instance;

            Debug.Log($"[Quest] ✓ Quest accepted: {def.displayName}");
            OnQuestAccepted?.Invoke(instance);

            return true;
        }

        /// <summary>
        /// Complete a quest and grant rewards.
        /// </summary>
        public bool CompleteQuest(string questId)
        {
            if (!activeQuests.ContainsKey(questId))
            {
                Debug.LogWarning($"[Quest] Quest not active: {questId}");
                return false;
            }

            QuestInstance instance = activeQuests[questId];

            // Check if all objectives are complete
            if (!instance.IsComplete())
            {
                Debug.LogWarning($"[Quest] Quest objectives not complete: {questId}");
                return false;
            }

            // Grant rewards
            GrantRewards(instance.definition.rewards);

            // Mark as completed
            completedQuestIds.Add(questId);
            activeQuests.Remove(questId);

            Debug.Log($"[Quest] ✓ Quest completed: {instance.definition.displayName}");
            OnQuestCompleted?.Invoke(instance);

            return true;
        }

        /// <summary>
        /// Fail a quest and remove it from active list.
        /// </summary>
        public bool FailQuest(string questId)
        {
            if (!activeQuests.ContainsKey(questId))
            {
                Debug.LogWarning($"[Quest] Quest not active: {questId}");
                return false;
            }

            QuestInstance instance = activeQuests[questId];
            activeQuests.Remove(questId);

            Debug.Log($"[Quest] ✗ Quest failed: {instance.definition.displayName}");
            OnQuestFailed?.Invoke(instance);

            return true;
        }

        /// <summary>
        /// Update objective progress and check for completion.
        /// </summary>
        public bool UpdateObjective(string questId, int objectiveIndex, bool completed)
        {
            if (!activeQuests.ContainsKey(questId))
            {
                Debug.LogWarning($"[Quest] Quest not active: {questId}");
                return false;
            }

            QuestInstance instance = activeQuests[questId];
            if (objectiveIndex < 0 || objectiveIndex >= instance.objectives.Count)
            {
                Debug.LogWarning($"[Quest] Invalid objective index: {objectiveIndex}");
                return false;
            }

            instance.objectives[objectiveIndex].completed = completed;
            Debug.Log($"[Quest] Objective {objectiveIndex + 1} updated for {questId}");
            OnObjectiveUpdated?.Invoke(instance);

            // Auto-complete if all objectives done
            if (instance.IsComplete())
            {
                Debug.Log($"[Quest] All objectives complete for {questId}. Auto-completing...");
                CompleteQuest(questId);
            }

            return true;
        }

        #endregion

        #region Quest Data Management

        /// <summary>
        /// Register a quest definition (typically from ScriptableObject).
        /// </summary>
        public void RegisterQuestDefinition(QuestDef def)
        {
            if (def == null)
            {
                Debug.LogError("[Quest] Cannot register null quest definition");
                return;
            }

            questDefinitions[def.id] = def;
            Debug.Log($"[Quest] Registered: {def.displayName}");
        }

        /// <summary>
        /// Get active quest instance by ID.
        /// </summary>
        public QuestInstance GetActiveQuest(string questId)
        {
            return activeQuests.ContainsKey(questId) ? activeQuests[questId] : null;
        }

        /// <summary>
        /// Get all active quests.
        /// </summary>
        public List<QuestInstance> GetAllActiveQuests()
        {
            return new List<QuestInstance>(activeQuests.Values);
        }

        /// <summary>
        /// Get completed quest IDs.
        /// </summary>
        public List<string> GetCompletedQuests()
        {
            return new List<string>(completedQuestIds);
        }

        /// <summary>
        /// Check if quest is completed.
        /// </summary>
        public bool IsQuestCompleted(string questId)
        {
            return completedQuestIds.Contains(questId);
        }

        #endregion

        #region Rewards

        /// <summary>
        /// Grant quest rewards to player (XP, gold, items).
        /// </summary>
        private void GrantRewards(QuestReward reward)
        {
            if (reward == null) return;

            Debug.Log($"[Quest] Granting rewards: +{reward.experienceReward} XP, +{reward.goldReward} gold");

            // Award XP
            if (reward.experienceReward > 0)
            {
                var player = FindObjectOfType<Tempest.Player.Player>();
                if (player != null)
                {
                    Tempest.Combat.CombatManager.Instance.GainXP(player.combatEntity, reward.experienceReward);
                }
            }

            // Award gold
            if (reward.goldReward > 0)
            {
                // TODO: Add to inventory/currency system
                Debug.Log($"[Quest] +{reward.goldReward} gold");
            }

            // Award items
            if (reward.itemRewards.Count > 0)
            {
                foreach (var item in reward.itemRewards)
                {
                    Debug.Log($"[Quest] +1x {item.itemId}");
                    // TODO: Add to inventory
                }
            }
        }

        #endregion
    }

    #region Quest Data Structures

    /// <summary>
    /// Quest definition (template). Use ScriptableObject for easy content creation.
    /// </summary>
    [CreateAssetMenu(menuName = "Tempest/Quest/Quest Definition", fileName = "Quest_")]
    public class QuestDef : ScriptableObject
    {
        public string id = "quest_001";
        public string displayName = "New Quest";
        [TextArea(3, 5)]
        public string description = "Quest description...";

        public string prerequisiteQuestId; // Leave empty if no prerequisites
        public string giver; // NPC who gives the quest

        [System.Serializable]
        public class Objective
        {
            public string description = "Objective...";
            public bool completed = false;
        }

        public List<Objective> objectives = new List<Objective>();
        public QuestReward rewards = new QuestReward();
    }

    /// <summary>
    /// Runtime instance of a quest (tracks progress).
    /// </summary>
    public class QuestInstance
    {
        public QuestDef definition;
        public List<QuestDef.Objective> objectives = new List<QuestDef.Objective>();
        public float progress = 0f;

        public QuestInstance(QuestDef def)
        {
            definition = def;
            // Deep copy objectives
            foreach (var obj in def.objectives)
            {
                objectives.Add(new QuestDef.Objective
                {
                    description = obj.description,
                    completed = obj.completed
                });
            }
            UpdateProgress();
        }

        public bool IsComplete()
        {
            foreach (var obj in objectives)
            {
                if (!obj.completed) return false;
            }
            return objectives.Count > 0;
        }

        public void UpdateProgress()
        {
            if (objectives.Count == 0)
            {
                progress = 0f;
                return;
            }

            int completed = 0;
            foreach (var obj in objectives)
            {
                if (obj.completed) completed++;
            }

            progress = (float)completed / objectives.Count;
        }
    }

    /// <summary>
    /// Quest rewards (XP, gold, items).
    /// </summary>
    [System.Serializable]
    public class QuestReward
    {
        public int experienceReward = 100;
        public int goldReward = 50;

        [System.Serializable]
        public class ItemReward
        {
            public string itemId;
            public int quantity = 1;
        }

        public List<ItemReward> itemRewards = new List<ItemReward>();
    }

    #endregion
}
