using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public static LootSpawner Instance { get; private set; }

    public string defaultDropItemId = "herb_rare";
    public int defaultDropAmount = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnLoot(Vector3 atPosition)
    {
        SpawnLoot(atPosition, null);
    }

    public void SpawnLoot(Vector3 atPosition, LootTableSO table)
    {
        if (table != null && table.entries != null && table.entries.Count > 0)
        {
            foreach (var e in table.entries)
            {
                if (Random.value <= e.chance)
                {
                    int qty = Mathf.Max(1, Random.Range(e.minAmount, e.maxAmount + 1));
                    TryGiveItemToPlayer(e.itemId, qty);
                    Debug.Log($"LootSpawner: Dropped {qty}x {e.itemId}");
                }
            }
            return;
        }

        // fallback
        TryGiveItemToPlayer(defaultDropItemId, defaultDropAmount);
        Debug.Log($"LootSpawner: dropped default {defaultDropItemId} at {atPosition}");
    }

    private void TryGiveItemToPlayer(string itemId, int qty)
    {
        var player = FindObjectOfType<Player>();
        var db = ItemsDatabase.Instance;
        if (db != null)
        {
            var so = db.GetItem(itemId);
            if (so != null)
            {
                // map TempestItemType -> ItemCategory
                ItemCategory cat = MapTypeToCategory(so.itemType, so);
                float weight = so.weight;
                float value = so.value;
                InventorySystem.Instance?.AddItem(so.id, so.displayName, so.rarity, cat, weight, qty, so.description, value);
                UIController.GetInstance()?.ShowNotification($"Получено: {so.displayName} x{qty}");
                return;
            }
        }

        // fallback to string id usage
        InventorySystem.Instance?.AddItem(itemId, itemId, ItemRarity.Common, ItemCategory.Resource, 0.1f, qty, "Looted item", 0);
    }

    private ItemCategory MapTypeToCategory(TempestItemType t, ItemSO so)
    {
        switch (t)
        {
            case TempestItemType.Consumable: return ItemCategory.Consumable;
            case TempestItemType.Equipment:
                // try to guess weapon vs armor based on equipSlot
                if (!string.IsNullOrEmpty(so.equipSlot))
                {
                    var s = so.equipSlot.ToLower();
                    if (s.Contains("weapon") || s.Contains("weapon")) return ItemCategory.Weapon;
                    if (s.Contains("chest") || s.Contains("armor") || s.Contains("body")) return ItemCategory.Armor;
                }
                return ItemCategory.Artifact;
            case TempestItemType.Artifact: return ItemCategory.Artifact;
            case TempestItemType.Resource:
            default: return ItemCategory.Resource;
        }
    }
}
