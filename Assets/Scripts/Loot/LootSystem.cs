using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LootTable
{
    public string lootId;
    public List<LootItem> items = new List<LootItem>();
}

[System.Serializable]
public class LootItem
{
    public string itemId;
    public string itemName;
    public ItemRarity rarity;
    public float dropChance; // 0-100
    public int minQuantity;
    public int maxQuantity;
}

public class LootSystem : MonoBehaviour
{
    public static LootSystem Instance { get; private set; }

    private Dictionary<string, LootTable> lootTables = new Dictionary<string, LootTable>();

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
        InitializeLootTables();
    }

    private void InitializeLootTables()
    {
        // Лут от врагов
        CreateLootTable("enemy_common", new List<LootItem>
        {
            new LootItem { itemId = "coin_gold", itemName = "Золотая монета", rarity = ItemRarity.Common, dropChance = 80, minQuantity = 1, maxQuantity = 5 },
            new LootItem { itemId = "herb_common", itemName = "Обычная трава", rarity = ItemRarity.Common, dropChance = 50, minQuantity = 1, maxQuantity = 2 },
            new LootItem { itemId = "potion_health", itemName = "Зелье здоровья", rarity = ItemRarity.Uncommon, dropChance = 30, minQuantity = 1, maxQuantity = 1 }
        });

        // Лут от боссов
        CreateLootTable("boss_rare", new List<LootItem>
        {
            new LootItem { itemId = "artifact_ring", itemName = "Кольцо Силы", rarity = ItemRarity.Rare, dropChance = 70, minQuantity = 1, maxQuantity = 1 },
            new LootItem { itemId = "sword_rare", itemName = "Редкий меч", rarity = ItemRarity.Rare, dropChance = 60, minQuantity = 1, maxQuantity = 1 },
            new LootItem { itemId = "gem_crystal", itemName = "Кристалл", rarity = ItemRarity.Epic, dropChance = 20, minQuantity = 1, maxQuantity = 2 }
        });

        // Лут от демонов
        CreateLootTable("demon_epic", new List<LootItem>
        {
            new LootItem { itemId = "soul_fragment", itemName = "Фрагмент Души", rarity = ItemRarity.Epic, dropChance = 80, minQuantity = 1, maxQuantity = 3 },
            new LootItem { itemId = "weapon_demonic", itemName = "Демоническое оружие", rarity = ItemRarity.Epic, dropChance = 50, minQuantity = 1, maxQuantity = 1 },
            new LootItem { itemId = "armor_demonic", itemName = "Демоническая броня", rarity = ItemRarity.Legendary, dropChance = 10, minQuantity = 1, maxQuantity = 1 }
        });
    }

    private void CreateLootTable(string id, List<LootItem> items)
    {
        LootTable table = new LootTable { lootId = id, items = items };
        lootTables[id] = table;
    }

    public void DropLoot(string lootTableId, Vector3 position)
    {
        if (!lootTables.TryGetValue(lootTableId, out LootTable table))
            return;

        foreach (LootItem item in table.items)
        {
            if (Random.value * 100 <= item.dropChance)
            {
                int quantity = Random.Range(item.minQuantity, item.maxQuantity + 1);
                InventorySystem.Instance.AddItem(item.itemId, item.itemName, item.rarity, 
                    ItemCategory.Resource, 0.5f, quantity, "Добытый лут", 50);
                Debug.Log($"Выпал лут: {item.itemName} x{quantity}");
            }
        }
    }

    public LootTable GetLootTable(string lootTableId) => lootTables.TryGetValue(lootTableId, out var table) ? table : null;
}
