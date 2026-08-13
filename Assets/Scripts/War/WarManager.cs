using System.Collections.Generic;
using UnityEngine;

public partial class WarManager : MonoBehaviour
{
    // Public list of ArmyDef assets used by CampaignManager/Enemy spawners
    public List<ArmyDef> armies = new List<ArmyDef>();

    // Simple tick settings for simulation
    public float tickInterval = 5f; // seconds
    private float timer = 0f;

    // Faction-level data and active wars (merged from older World/WarManager implementation)
    public class Faction
    {
        public string factionName;
        public string leader;
        public int strength; // 1-100
        public int territory; // number of settlements
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

    private Dictionary<string, Faction> factions = new Dictionary<string, Faction>();
    private List<War> activeWars = new List<War>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        timer = tickInterval;
        InitializeFactions();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // future: run periodic simulation steps
            timer = tickInterval;
        }
    }

    private void InitializeFactions()
    {
        // Example factions; these can be replaced by data-driven setup later
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
        if (!factions[attackerFaction].enemies.Contains(defenderFaction)) factions[attackerFaction].enemies.Add(defenderFaction);
        if (!factions[defenderFaction].enemies.Contains(attackerFaction)) factions[defenderFaction].enemies.Add(attackerFaction);

        Debug.Log($"War started: {attackerFaction} vs {defenderFaction}");
    }

    public void ForceAllianceWithFaction(string faction1, string faction2)
    {
        if (factions.TryGetValue(faction1, out Faction f1) && factions.TryGetValue(faction2, out Faction f2))
        {
            if (!f1.allies.Contains(faction2)) f1.allies.Add(faction2);
            if (!f2.allies.Contains(faction1)) f2.allies.Add(faction1);

            // Remove from enemies if present
            f1.enemies.Remove(faction2);
            f2.enemies.Remove(faction1);
        }
    }

    public List<War> GetActiveWars()
    {
        return activeWars;
    }
}
