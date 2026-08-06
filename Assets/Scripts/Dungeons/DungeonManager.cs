using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DungeonData
{
    public string dungeonName;
    public int level;
    public float difficulty;
    public List<string> enemyTypes;
    public bool isDiscovered;
    public Vector3 position;
    public int maxEnemies;
    public float lootReward;
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }
    
    private Dictionary<string, DungeonData> dungeons = new Dictionary<string, DungeonData>();
    private DungeonData currentDungeon;
    private List<Enemy> spawnedEnemies = new List<Enemy>();

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
        InitializeDungeons();
    }

    private void InitializeDungeons()
    {
        // Лес Чертовых (Forest of Jura)
        CreateDungeon("ForestOfJura", "Лес Чертовых", 1, 0.5f, new List<string> { "Goblin", "Wolf", "Slime" }, new Vector3(10, 0, 10), 5);
        
        // Пещера огня (Volcanic Cavern)
        CreateDungeon("VolcanicCavern", "Вулканическая Пещера", 3, 1.5f, new List<string> { "FireSlime", "Salamander", "LavaGiant" }, new Vector3(-15, 0, 20), 8);
        
        // Затопленный храм (Submerged Temple)
        CreateDungeon("SubmergedTemple", "Затопленный Храм", 2, 1.0f, new List<string> { "Dragonewt", "AquaSlime", "SeaMonster" }, new Vector3(25, 0, -10), 6);
        
        // Королевство гоблинов (Goblin Kingdom)
        CreateDungeon("GoblinKingdom", "Королевство Гоблинов", 4, 2.0f, new List<string> { "GoblinLord", "GoblinArcher", "GoblinMage" }, new Vector3(-20, 0, -15), 10);
        
        // Башня Раймондса (Ramiris's Tower)
        CreateDungeon("RamirisTower", "Башня Раймондса", 5, 2.5f, new List<string> { "TowerGuardian", "MagicalBeast", "Demon" }, new Vector3(0, 0, -30), 12);
        
        // Лабиринт (Labyrinth)
        CreateDungeon("Labyrinth", "Лабиринт", 6, 3.0f, new List<string> { "Minotaur", "Basilisk", "Dragon" }, new Vector3(-40, 0, 0), 15);
    }

    private void CreateDungeon(string id, string name, int level, float difficulty, List<string> enemies, Vector3 pos, int maxEnemies)
    {
        DungeonData dungeon = new DungeonData
        {
            dungeonName = name,
            level = level,
            difficulty = difficulty,
            enemyTypes = enemies,
            isDiscovered = false,
            position = pos,
            maxEnemies = maxEnemies,
            lootReward = 50 * level
        };
        
        dungeons[id] = dungeon;
    }

    public void EnterDungeon(string dungeonId)
    {
        if (dungeons.TryGetValue(dungeonId, out DungeonData dungeon))
        {
            currentDungeon = dungeon;
            dungeon.isDiscovered = true;
            SpawnEnemies();
            Debug.Log($"Entered dungeon: {dungeon.dungeonName}");
        }
    }

    private void SpawnEnemies()
    {
        if (currentDungeon == null) return;
        
        spawnedEnemies.Clear();
        
        for (int i = 0; i < currentDungeon.maxEnemies; i++)
        {
            // Здесь будет спавн врагов
            string enemyType = currentDungeon.enemyTypes[Random.Range(0, currentDungeon.enemyTypes.Count)];
            Vector3 spawnPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + currentDungeon.position;
            
            // Создаем врага (здесь нужен префаб)
            // Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            // enemy.Initialize(enemyType, currentDungeon.level);
            // spawnedEnemies.Add(enemy);
        }
        
        Debug.Log($"Spawned {spawnedEnemies.Count} enemies in {currentDungeon.dungeonName}");
    }

    public void ExitDungeon()
    {
        foreach (Enemy enemy in spawnedEnemies)
        {
            Destroy(enemy.gameObject);
        }
        spawnedEnemies.Clear();
        currentDungeon = null;
        Debug.Log("Exited dungeon");
    }

    public DungeonData GetDungeon(string dungeonId)
    {
        return dungeons.TryGetValue(dungeonId, out var dungeon) ? dungeon : null;
    }

    public Dictionary<string, DungeonData> GetAllDungeons() => dungeons;
    public DungeonData GetCurrentDungeon() => currentDungeon;
}
