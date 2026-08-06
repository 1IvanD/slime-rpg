using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SettlementData
{
    public string settlementName;
    public string governorName;
    public int population;
    public float resources;
    public SettlementType type;
    public Vector3 position;
    public bool isDiscovered;
    public List<string> inhabitants;
    public bool isAllied;
    public float friendliness; // 0-100
}

public enum SettlementType
{
    GoblinVillage,
    HumanTown,
    FantasyCity,
    MonsterOutpost,
    TraderHub
}

public class SettlementManager : MonoBehaviour
{
    public static SettlementManager Instance { get; private set; }
    
    private Dictionary<string, SettlementData> settlements = new Dictionary<string, SettlementData>();
    private SettlementData currentSettlement;

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
        InitializeSettlements();
    }

    private void InitializeSettlements()
    {
        // Деревня гоблинов Риммуру
        CreateSettlement("RimmuruVillage", "Деревня Риммуру", "Риммуру", SettlementType.GoblinVillage, 
            new Vector3(5, 0, 5), 50, new List<string> { "Goblin", "Hobgoblin" });
        
        // Город Ингрессия
        CreateSettlement("IngrassiaCity", "Город Ингрессия", "Король Инграссии", SettlementType.HumanTown,
            new Vector3(30, 0, 0), 500, new List<string> { "Human Merchant", "Knight" });
        
        // Деревня драконов
        CreateSettlement("DragonSettlement", "Поселение Драконов", "Вельдора", SettlementType.FantasyCity,
            new Vector3(-30, 0, 20), 100, new List<string> { "Dragon", "Wyvern" });
        
        // Торговый хаб
        CreateSettlement("TradePost", "Торговый Пост", "Трейдер Боб", SettlementType.TraderHub,
            new Vector3(0, 0, 25), 200, new List<string> { "Merchant", "Trader" });
        
        // Форпост монстров
        CreateSettlement("MonsterOutpost", "Форпост Монстров", "Командир Монстров", SettlementType.MonsterOutpost,
            new Vector3(-25, 0, -20), 150, new List<string> { "Monster", "Beast" });
    }

    private void CreateSettlement(string id, string name, string governor, SettlementType type, 
        Vector3 pos, int population, List<string> inhabitants)
    {
        SettlementData settlement = new SettlementData
        {
            settlementName = name,
            governorName = governor,
            population = population,
            resources = 100,
            type = type,
            position = pos,
            isDiscovered = false,
            inhabitants = inhabitants,
            isAllied = false,
            friendliness = 50
        };
        
        settlements[id] = settlement;
    }

    public void VisitSettlement(string settlementId)
    {
        if (settlements.TryGetValue(settlementId, out SettlementData settlement))
        {
            currentSettlement = settlement;
            settlement.isDiscovered = true;
            Debug.Log($"Visited settlement: {settlement.settlementName}, Governor: {settlement.governorName}");
        }
    }

    public void NegotiateWithSettlement(bool isAggressive = false)
    {
        if (currentSettlement == null) return;
        
        if (isAggressive)
        {
            currentSettlement.friendliness -= 20;
            Debug.Log($"Aggressive action taken. Friendliness now: {currentSettlement.friendliness}");
        }
        else
        {
            currentSettlement.friendliness += 10;
            Debug.Log($"Peaceful negotiation. Friendliness now: {currentSettlement.friendliness}");
            
            if (currentSettlement.friendliness >= 70)
            {
                BecomeAllied(currentSettlement);
            }
        }
    }

    public void TradeWithSettlement(int goldAmount)
    {
        if (currentSettlement == null) return;
        
        currentSettlement.resources += goldAmount * 0.5f;
        currentSettlement.friendliness += 5;
        Debug.Log($"Traded {goldAmount} gold with {currentSettlement.settlementName}");
    }

    private void BecomeAllied(SettlementData settlement)
    {
        settlement.isAllied = true;
        Debug.Log($"{settlement.settlementName} is now allied!");
    }

    public void DeclareWar(string settlementId)
    {
        if (settlements.TryGetValue(settlementId, out SettlementData settlement))
        {
            settlement.isAllied = false;
            settlement.friendliness = 0;
            // Запустить боевую систему
            Debug.Log($"War declared against {settlement.settlementName}!");
        }
    }

    public SettlementData GetSettlement(string settlementId)
    {
        return settlements.TryGetValue(settlementId, out var settlement) ? settlement : null;
    }

    public Dictionary<string, SettlementData> GetAllSettlements() => settlements;
    public SettlementData GetCurrentSettlement() => currentSettlement;
}
