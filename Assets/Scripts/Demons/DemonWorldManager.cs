using UnityEngine;
using System.Collections.Generic;

public class DemonWorldManager : MonoBehaviour
{
    public static DemonWorldManager Instance { get; private set; }

    [System.Serializable]
    public class DemonWorldRegion
    {
        public string regionName;
        public DemonRank minRank;
        public DemonRank maxRank;
        public Vector3 position;
        public float dangerLevel; // 1-10
        public List<string> inhabitants;
    }

    private Dictionary<string, DemonWorldRegion> demonWorldRegions = new Dictionary<string, DemonWorldRegion>();
    private Player currentPlayer;

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
        InitializeDemonWorld();
    }

    private void InitializeDemonWorld()
    {
        // Внешний слой ада (Outer Demon Realm)
        CreateDemonRegion("OuterRealm", "Внешний Слой Ада", DemonRank.LowerDemon, DemonRank.MidDemon,
            new Vector3(150, 0, 150), 3, new List<string> { "LowerDemon1", "MidDemon1" });

        // Ядовитое болото (Poison Swamp)
        CreateDemonRegion("PoisonSwamp", "Ядовитое Болото", DemonRank.MidDemon, DemonRank.UpperDemon,
            new Vector3(180, 0, 180), 5, new List<string> { "MidDemon1", "MidDemon2", "UpperDemon1" });

        // Огненный Ад (Burning Inferno)
        CreateDemonRegion("BurningInferno", "Огненный Ад", DemonRank.UpperDemon, DemonRank.ArcDemon,
            new Vector3(200, 20, 200), 7, new List<string> { "UpperDemon1", "UpperDemon2", "ArcDemon1" });

        // Врата Подземелья (Gate to Abyss)
        CreateDemonRegion("GateOfAbyss", "Врата Бездны", DemonRank.ArcDemon, DemonRank.PrimordialDemon,
            new Vector3(250, 30, 250), 8, new List<string> { "ArcDemon1", "ArcDemon2", "PrimordialDemon1" });

        // Святилище Первородных (Primordial Sanctuary)
        CreateDemonRegion("PrimordialSanctuary", "Святилище Первородных", DemonRank.PrimordialDemon, DemonRank.PrimordialDemon,
            new Vector3(300, 50, 300), 9, new List<string> { "PrimordialDemon1", "PrimordialDemon2", "PrimordialDemon3" });

        // Престол Люцифера (Lucifer's Throne)
        CreateDemonRegion("LuciferThrone", "Престол Люцифера", DemonRank.DemonLord, DemonRank.DemonLord,
            new Vector3(400, 100, 400), 10, new List<string> { "SupremeLord" });
    }

    private void CreateDemonRegion(string id, string name, DemonRank minRank, DemonRank maxRank,
        Vector3 position, float dangerLevel, List<string> inhabitants)
    {
        DemonWorldRegion region = new DemonWorldRegion
        {
            regionName = name,
            minRank = minRank,
            maxRank = maxRank,
            position = position,
            dangerLevel = dangerLevel,
            inhabitants = inhabitants
        };

        demonWorldRegions[id] = region;
    }

    public void EnterDemonRealm(string regionId)
    {
        if (demonWorldRegions.TryGetValue(regionId, out DemonWorldRegion region))
        {
            currentPlayer = FindObjectOfType<Player>();
            if (currentPlayer != null)
            {
                currentPlayer.transform.position = region.position + Vector3.up * 5;
                Debug.Log($"Entered demon realm: {region.regionName} (Danger Level: {region.dangerLevel})");
            }
        }
    }

    public DemonWorldRegion GetRegion(string regionId)
    {
        return demonWorldRegions.TryGetValue(regionId, out var region) ? region : null;
    }

    public Dictionary<string, DemonWorldRegion> GetAllDemonRegions() => demonWorldRegions;
}
