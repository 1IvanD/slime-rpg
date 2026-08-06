using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnchantmentData
{
    public string enchantmentId;
    public string name;
    public string bonus;
    public float value;
    public int level;
}

[System.Serializable]
public class UpgradeData
{
    public string itemId;
    public int upgradeLevel;
    public float damageBonus;
    public float armorBonus;
    public float costPerUpgrade;
}

public class EquipmentUpgradeSystem : MonoBehaviour
{
    public static EquipmentUpgradeSystem Instance { get; private set; }

    private Dictionary<string, UpgradeData> upgradedItems = new Dictionary<string, UpgradeData>();
    private Dictionary<string, List<EnchantmentData>> itemEnchantments = new Dictionary<string, List<EnchantmentData>>();

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

    public bool UpgradeItem(string itemId, int amount = 1)
    {
        if (!upgradedItems.ContainsKey(itemId))
        {
            upgradedItems[itemId] = new UpgradeData
            {
                itemId = itemId,
                upgradeLevel = 0,
                damageBonus = 0,
                armorBonus = 0,
                costPerUpgrade = 100
            };
        }

        UpgradeData upgrade = upgradedItems[itemId];
        float totalCost = upgrade.costPerUpgrade * amount;

        if (EconomySystem.Instance.SpendGold(totalCost))
        {
            upgrade.upgradeLevel += amount;
            upgrade.damageBonus += amount * 10;
            upgrade.armorBonus += amount * 5;
            upgrade.costPerUpgrade *= 1.1f;
            
            Debug.Log($"Предмет улучшен: {itemId} до уровня +{upgrade.upgradeLevel}");
            return true;
        }
        
        Debug.Log("Недостаточно золота для улучшения!");
        return false;
    }

    public bool AddEnchantment(string itemId, string enchantmentId, string name, string bonus, float value)
    {
        if (!itemEnchantments.ContainsKey(itemId))
        {
            itemEnchantments[itemId] = new List<EnchantmentData>();
        }

        // Максимум 3 чара на предмет
        if (itemEnchantments[itemId].Count >= 3)
        {
            Debug.Log("Максимум очарований на предмет!");
            return false;
        }

        EnchantmentData enchantment = new EnchantmentData
        {
            enchantmentId = enchantmentId,
            name = name,
            bonus = bonus,
            value = value,
            level = 1
        };

        itemEnchantments[itemId].Add(enchantment);
        Debug.Log($"Добавлено очарование: {name} к предмету {itemId}");
        return true;
    }

    public bool SynthesizeItem(string material1Id, string material2Id, string material3Id, out string newItemId)
    {
        newItemId = "synthesized_" + Random.Range(1000, 9999);
        
        // Удалить материалы из инвентаря
        InventorySystem.Instance.RemoveItem(material1Id, 1);
        InventorySystem.Instance.RemoveItem(material2Id, 1);
        InventorySystem.Instance.RemoveItem(material3Id, 1);
        
        // Добавить новый предмет
        InventorySystem.Instance.AddItem(newItemId, "Синтезированный предмет", ItemRarity.Rare, 
            ItemCategory.Artifact, 1f, 1, "Редкий синтезированный предмет", 500);
        
        Debug.Log($"Предмет синтезирован: {newItemId}");
        return true;
    }

    public UpgradeData GetUpgradeData(string itemId) => upgradedItems.TryGetValue(itemId, out var data) ? data : null;
    public List<EnchantmentData> GetEnchantments(string itemId) => itemEnchantments.TryGetValue(itemId, out var list) ? list : null;
}
