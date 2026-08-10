using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemId;
    public string itemName;
    public ItemRarity rarity;
    public ItemCategory category;
    public float weight;
    public int quantity;
    public string description;
    public float value;
}

public enum ItemRarity
{
    Common,      // Обычное
    Uncommon,    // Необычное
    Rare,        // Редкое
    Epic,        // Эпик
    Legendary    // Легендарное
}

public enum ItemCategory
{
    Weapon,
    Armor,
    Potion,
    Resource,
    Artifact,
    Consumable
}

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    private Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();
    private float maxWeight = 100f;
    private float currentWeight = 0f;
    private int maxSlots = 50;

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
        InitializeStartingItems();
    }

    private void InitializeStartingItems()
    {
        // Начальные предметы
        AddItem("potion_health", "Зелье здоровья", ItemRarity.Common, ItemCategory.Potion, 0.5f, 3, "Восстанавливает 50 HP", 50);
        AddItem("sword_iron", "Железный меч", ItemRarity.Common, ItemCategory.Weapon, 2f, 1, "Базовое оружие", 100);
        AddItem("herb_rare", "Редкая трава", ItemRarity.Rare, ItemCategory.Resource, 0.1f, 5, "Для крафта", 200);
    }

    public bool AddItem(string id, string name, ItemRarity rarity, ItemCategory category, 
        float weight, int quantity, string desc, float value)
    {
        if (currentWeight + (weight * quantity) > maxWeight)
        {
            Debug.Log("Инвентарь переполнен!");
            return false;
        }

        if (inventory.TryGetValue(id, out InventoryItem existingItem))
        {
            existingItem.quantity += quantity;
        }
        else
        {
            InventoryItem newItem = new InventoryItem
            {
                itemId = id,
                itemName = name,
                rarity = rarity,
                category = category,
                weight = weight,
                quantity = quantity,
                description = desc,
                value = value
            };
            inventory[id] = newItem;
        }

        currentWeight += weight * quantity;
        Debug.Log($"Предмет добавлен: {name} x{quantity}");
        return true;
    }

    public bool RemoveItem(string itemId, int quantity)
    {
        if (inventory.TryGetValue(itemId, out InventoryItem item))
        {
            if (item.quantity >= quantity)
            {
                currentWeight -= item.weight * quantity;
                item.quantity -= quantity;
                
                if (item.quantity <= 0)
                {
                    inventory.Remove(itemId);
                }
                
                Debug.Log($"Предмет удален: {item.itemName} x{quantity}");
                return true;
            }
        }
        return false;
    }

    public bool UseItem(string itemId)
    {
        if (inventory.TryGetValue(itemId, out InventoryItem item))
        {
            Debug.Log($"Используется: {item.itemName}");
            return RemoveItem(itemId, 1);
        }
        return false;
    }

    public bool SellItem(string itemId, int quantity)
    {
        if (inventory.TryGetValue(itemId, out InventoryItem item))
        {
            float sellPrice = item.value * quantity;
            EconomySystem.Instance.AddGold(sellPrice);
            RemoveItem(itemId, quantity);
            Debug.Log($"Продано: {item.itemName} x{quantity} за {sellPrice} золота");
            return true;
        }
        return false;
    }

    public Dictionary<string, InventoryItem> GetInventory() => inventory;
    public float GetCurrentWeight() => currentWeight;
    public float GetMaxWeight() => maxWeight;
    public int GetItemCount() => inventory.Count;
    public InventoryItem GetItem(string itemId) => inventory.TryGetValue(itemId, out var item) ? item : null;

    // Helper: check existence of an item
    public bool HasItem(string itemId, int requiredAmount = 1)
    {
        if (string.IsNullOrEmpty(itemId)) return false;
        if (!inventory.TryGetValue(itemId, out var item)) return false;
        return item.quantity >= requiredAmount;
    }
}
