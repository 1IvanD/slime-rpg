using UnityEngine;
using System.Collections.Generic;

public class Faction
{
    public string factionName;
    public string leader;
    public int strength; // 1-100
    public int territory; // количество поселений
    public List<string> allies;
    public List<string> enemies;
    public float resources;
}

public class WarManager : MonoBehaviour
{
    public static WarManager Instance { get; private set; }
    
    private Dictionary<string, Faction> factions = new Dictionary<string, Faction>();
    private List<War> activeWars = new List<War>();

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
    }

    private void Start()
    {
        InitializeFactions();
    }

    private void InitializeFactions()
    {
        // Монстры (Monsters)
        CreateFaction("Monsters", "Риммуру", 40, 3, 200);
        
        // Люди (Humans)
        CreateFaction("Humans", "Король Инграссии", 60, 5, 500);
        
        // Драконы (Dragons)
        CreateFaction("Dragons", "Вельдора", 80, 2, 1000);
        
        // Демоны (Demons)
        CreateFaction("Demons", "Король Демонов", 70, 4, 800);
        
        // Торговцы (Traders)
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
            allies = new List<string>(),
            enemies = new List<string>(),
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

    public void ForceAllianceWithFaction(string faction1, string faction2)
    {
        if (factions.TryGetValue(faction1, out Faction f1) && factions.TryGetValue(faction2, out Faction f2))
        {
            f1.allies.Add(faction2);
            f2.allies.Add(faction1);
            
            // Удалить из врагов, если были
            f1.enemies.Remove(faction2);
            f2.enemies.Remove(faction1);
            
            Debug.Log($"{faction1} and {faction2} are now allies!");
        }
    }

    public void UpdateWars()
    {
        for (int i = activeWars.Count - 1; i >= 0; i--)
        {
            War war = activeWars[i];
            war.progress += Time.deltaTime;
            
            if (war.progress >= war.duration)
            {
                war.isActive = false;
                ResolveWar(war);
                activeWars.RemoveAt(i);
            }
        }
    }

    private void ResolveWar(War war)
    {
        Faction attacker = factions[war.attacker];
        Faction defender = factions[war.defender];
        
        // Определить победителя
        bool attackerWins = attacker.strength > defender.strength;
        
        if (attackerWins)
        {
            attacker.territory++;
            defender.territory--;
            attacker.resources += defender.resources * 0.3f;
            Debug.Log($"{war.attacker} wins the war!");
        }
        else
        {
            defender.resources += attacker.resources * 0.2f;
            Debug.Log($"{war.defender} wins the war!");
        }
    }

    public Faction GetFaction(string factionName)
    {
        return factions.TryGetValue(factionName, out var faction) ? faction : null;
    }

    public Dictionary<string, Faction> GetAllFactions() => factions;
    public List<War> GetActiveWars() => activeWars;

    private void Update()
    {
        UpdateWars();
    }
}
