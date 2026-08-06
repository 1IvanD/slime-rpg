using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DemonLord
{
    public string name;
    public string title;
    public int power; // 1-100
    public List<string> abilities;
    public DemonRank rank;
    public bool isDefeated;
    public Vector3 position;
    public float health;
    public float maxHealth;
}

public enum DemonRank
{
    LowerDemon,
    MidDemon,
    UpperDemon,
    ArcDemon,
    PrimordialDemon,
    DemonLord
}

public class DemonManager : MonoBehaviour
{
    public static DemonManager Instance { get; private set; }
    
    private Dictionary<string, DemonLord> demonLords = new Dictionary<string, DemonLord>();
    private List<DemonLord> defeatedDemons = new List<DemonLord>();
    private DemonLord currentTargetDemon;

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
        InitializeDemonLords();
    }

    private void InitializeDemonLords()
    {
        // Нижние демоны (Lower Demons)
        CreateDemonLord("LowerDemon1", "Мелкий Демон", "Малыш Демонической Иерархии", DemonRank.LowerDemon, 10, 15,
            new List<string> { "Scratch", "Bite", "Flee" }, new Vector3(20, 0, 30));
        
        // Средние демоны (Mid-tier Demons)
        CreateDemonLord("MidDemon1", "Жгучий Демон", "Повелитель Пламени", DemonRank.MidDemon, 25, 50,
            new List<string> { "Fireball", "Inferno", "Teleport" }, new Vector3(10, 0, 40));
        
        CreateDemonLord("MidDemon2", "Теневой Демон", "Король Теней", DemonRank.MidDemon, 28, 60,
            new List<string> { "ShadowStrike", "Darkness", "Curse" }, new Vector3(-10, 0, 35));
        
        // Верхние демоны (Upper Demons)
        CreateDemonLord("UpperDemon1", "Небесный Демон", "Властелин Небес", DemonRank.UpperDemon, 40, 100,
            new List<string> { "Lightning", "FlightAttack", "StormSummon" }, new Vector3(0, 10, 50));
        
        CreateDemonLord("UpperDemon2", "Кровавый Демон", "Вампир Древних Времен", DemonRank.UpperDemon, 42, 110,
            new List<string> { "BloodDrain", "Regeneration", "Charm" }, new Vector3(-20, 0, 45));
        
        // Арк-демоны (Arc Demons)
        CreateDemonLord("ArcDemon1", "Измеритель Судеб", "Страж Врат Ада", DemonRank.ArcDemon, 60, 200,
            new List<string> { "FateManipulation", "Hellfire", "DimensionalRift" }, new Vector3(50, 5, 60));
        
        CreateDemonLord("ArcDemon2", "Король Иллюзий", "Повелитель Снов", DemonRank.ArcDemon, 62, 220,
            new List<string> { "IllusionMastery", "MindControl", "RealityWarp" }, new Vector3(-50, 0, 70));
        
        // Первородные демоны (Primordial Demons)
        CreateDemonLord("PrimordialDemon1", "Вельзебуб", "Властелин Разложения", DemonRank.PrimordialDemon, 80, 300,
            new List<string> { "Corruption", "Swarm", "PlagueSpreading" }, new Vector3(30, 0, 80));
        
        CreateDemonLord("PrimordialDemon2", "Аспирел", "Королева Ночи", DemonRank.PrimordialDemon, 82, 320,
            new List<string> { "NightMastery", "StarFall", "CosmicWeapon" }, new Vector3(-60, 0, 90));
        
        CreateDemonLord("PrimordialDemon3", "Леон", "Первовладыка Силы", DemonRank.PrimordialDemon, 85, 350,
            new List<string> { "TruePower", "GolemCreation", "EarthManipulation" }, new Vector3(0, 0, 100));
        
        // Лорды демонов (Demon Lords)
        CreateDemonLord("DemonLord1", "Гай Кримсон", "Повелитель Справедливости", DemonRank.DemonLord, 95, 500,
            new List<string> { "Void", "Disintegration", "OmniscientEyes" }, new Vector3(-80, 10, 120));
        
        CreateDemonLord("DemonLord2", "Дестра", "Властелин Конца", DemonRank.DemonLord, 92, 480,
            new List<string> { "Apocalypse", "Annihilation", "TimeFracture" }, new Vector3(100, 5, 130));
        
        // Верховный лорд (Supreme Demon Lord)
        CreateDemonLord("SupremeLord", "Люцифер", "Король всех Демонов", DemonRank.DemonLord, 100, 1000,
            new List<string> { "AbsoluteEvil", "UltimateWill", "InfinityPower", "CreationDestruction" }, 
            new Vector3(0, 20, 200));
    }

    private void CreateDemonLord(string id, string name, string title, DemonRank rank, int power, float maxHealth,
        List<string> abilities, Vector3 position)
    {
        DemonLord demon = new DemonLord
        {
            name = name,
            title = title,
            power = power,
            abilities = abilities,
            rank = rank,
            isDefeated = false,
            position = position,
            health = maxHealth,
            maxHealth = maxHealth
        };
        
        demonLords[id] = demon;
    }

    public void SetTargetDemon(string demonId)
    {
        if (demonLords.TryGetValue(demonId, out DemonLord demon))
        {
            currentTargetDemon = demon;
            Debug.Log($"Targeting demon: {demon.name} ({demon.title})");
        }
    }

    public void DamageDemon(float damage)
    {
        if (currentTargetDemon != null)
        {
            currentTargetDemon.health -= damage;
            if (currentTargetDemon.health <= 0)
            {
                DefeatDemon();
            }
        }
    }

    private void DefeatDemon()
    {
        if (currentTargetDemon == null) return;
        
        currentTargetDemon.isDefeated = true;
        defeatedDemons.Add(currentTargetDemon);
        Debug.Log($"Defeated demon: {currentTargetDemon.name}!");
        
        currentTargetDemon = null;
    }

    public DemonLord GetDemon(string demonId)
    {
        return demonLords.TryGetValue(demonId, out var demon) ? demon : null;
    }

    public DemonLord GetCurrentTargetDemon() => currentTargetDemon;
    public Dictionary<string, DemonLord> GetAllDemons() => demonLords;
    public List<DemonLord> GetDefeatedDemons() => defeatedDemons;
}
