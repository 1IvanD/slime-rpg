using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Save
{
    /// <summary>
    /// Complete serializable world state for save/load operations.
    /// Contains all game systems' state in one unified structure.
    /// </summary>
    [System.Serializable]
    public class WorldState
    {
        public int version = 1;
        public string timestamp;
        public string sceneName;

        #region Player State

        [System.Serializable]
        public class PlayerState
        {
            public Vector3 position;
            public Quaternion rotation;
            public int level;
            public float health;
            public float maxHealth;
            public float experience;
            public string race;
        }

        public PlayerState playerState;

        #endregion

        #region Quest State

        [System.Serializable]
        public class QuestState
        {
            public string questId;
            public string status;
            public List<bool> objectives = new List<bool>();
        }

        public List<QuestState> quests = new List<QuestState>();

        #endregion

        #region NPC Affinity State

        [System.Serializable]
        public class NPCAffinityState
        {
            public string npcId;
            public int affinity;
            public bool recruited;
        }

        public List<NPCAffinityState> npcAffinity = new List<NPCAffinityState>();

        #endregion

        #region War State

        [System.Serializable]
        public class WarState
        {
            [System.Serializable]
            public class WarInfo
            {
                public string attacker;
                public string defender;
                public float progress;
                public float duration;
            }

            public List<string> activeFactions = new List<string>();
            public List<WarInfo> activeWars = new List<WarInfo>();
        }

        public WarState warState;

        #endregion

        #region Settlement State

        [System.Serializable]
        public class SettlementState
        {
            public string settlementId;
            public bool isAllied;
            public float friendliness;
            public float resources;
        }

        public List<SettlementState> settlements = new List<SettlementState>();

        #endregion

        #region Constructor

        public WorldState()
        {
            playerState = new PlayerState();
            warState = new WarState();
        }

        #endregion
    }
}
