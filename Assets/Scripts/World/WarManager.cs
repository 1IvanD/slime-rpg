using UnityEngine;
using System.Collections.Generic;

public class WorldFactionManager : MonoBehaviour
{
    // Renamed from WarManager to avoid class name collision with War/WarManager
    public static WorldFactionManager Instance { get; private set; }

    private Dictionary<string, Faction> factions = new Dictionary<string, Faction>();
    private List<War> activeWars = new List<War>();

    [System.Serializable]
    public class Faction
    {
        public string factionName;
        public string leader;
        public int strength; // 1-100
        public int territory; // количество поселений
        public List<string> allies = new List<string>();
        public List<string> enemies = new List<string>();
        public float resources;
    }

    [System.Serializable]
    public class War
    {
        public string attacker;
        public string defender;
        public float duration;
        public float progress;
        public bool isActive;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFactions();
    }

    private void InitializeFactions()
    {
        // Example factions
        CreateFaction("Monsters", "Риммуру", 40, 3, 200);
        CreateFaction("Humans", "Король Инграссии", 60, 5, 500);
        CreateFaction("Dragons", "Вельдора", 80, 2, 1000);
        CreateFaction("Demons", "Король Демонов", 70, 4, 800);
        CreateFaction("Traders", "Торговый Гильдия", 30, 3, 300);
    }

    private void CreateFaction(string name, string leader, int strength, int territory, float resources)
    {
        Faction faction = new Faction
        {
            factionName = name,
            leader = leader,
            strength = strength,
            territory = territory,
            resources = resources
        };

        factions[name] = faction;
    }

    public void StartWar(string attackerFaction, string defenderFaction, float duration = 30f)
    {
        if (!factions.ContainsKey(attackerFaction) || !factions.ContainsKey(defenderFaction))
            return;

        War war = new War
        {
            attacker = attackerFaction,
            defender = defenderFaction,
            duration = duration,
            progress = 0,
            isActive = true
        };

        activeWars.Add(war);
        factions[attackerFaction].enemies.Add(defenderFaction);
        factions[defenderFaction].enemies.Add(attackerFaction);

        Debug.Log($"War started: {attackerFaction} vs {defenderFaction}");
    }

    public List<War> GetActiveWars()
    {
        return activeWars;
    }
}
