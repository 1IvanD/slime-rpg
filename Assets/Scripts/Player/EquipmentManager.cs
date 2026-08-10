using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    // slot -> item id
    private Dictionary<string, ItemSO> equipped = new Dictionary<string, ItemSO>();

    private Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        // ensure stats reflect equipment on start
        RecalculateEquipmentBonuses();
    }

    public bool Equip(string itemId)
    {
        var db = ItemsDatabase.Instance;
        if (db == null) { Debug.LogWarning("EquipmentManager: ItemsDatabase not found."); return false; }
        var item = db.GetItem(itemId);
        if (item == null) { Debug.LogWarning($"Equip: Item {itemId} not found in DB"); return false; }
        if (item.itemType != TempestItemType.Equipment)
        {
            Debug.Log($"Equip: Item {item.displayName} is not equipment");
            return false;
        }

        string slot = string.IsNullOrEmpty(item.equipSlot) ? "Accessory" : item.equipSlot;

        // if slot occupied, unequip first
        if (equipped.ContainsKey(slot))
        {
            Unequip(slot);
        }

        equipped[slot] = item;
        RecalculateEquipmentBonuses();
        UIController.GetInstance()?.ShowNotification($"Equipped: {item.displayName} in {slot}");
        return true;
    }

    public bool Unequip(string slot)
    {
        if (!equipped.ContainsKey(slot)) return false;
        var item = equipped[slot];
        equipped.Remove(slot);
        RecalculateEquipmentBonuses();
        UIController.GetInstance()?.ShowNotification($"Unequipped: {item.displayName} from {slot}");
        // return item to inventory (try add)
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.AddItem(item.id, item.displayName, (ItemRarity)item.rarity, ItemCategory.Artifact, 0.1f, 1, item.description, itemValue(item));
        }
        return true;
    }

    private float itemValue(ItemSO item)
    {
        // simple heuristic: rarity * (attack+defense+magic+speed+intellect)
        int rarityMul = Mathf.Max(1, (int)item.rarity + 1);
        int sum = item.attack + item.defense + item.magic + item.speed + item.intellect;
        return Mathf.Max(1f, sum * 10f * rarityMul);
    }

    public Dictionary<string, ItemSO> GetEquipped() => new Dictionary<string, ItemSO>(equipped);

    private void RecalculateEquipmentBonuses()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (player == null) return;

        // reset base stats from player's starting attributes
        player.ResetStatsToBase();

        // apply bonuses
        foreach (var kv in equipped)
        {
            var it = kv.Value;
            if (it == null) continue;
            player.stats.Attack += it.attack;
            player.stats.Defense += it.defense;
            player.stats.MaxHealth += it.magic * 2; // example mapping
            player.stats.Health = Mathf.Min(player.stats.Health, player.stats.MaxHealth);
            // apply other attribute increases
            player.stats.Strength += it.attack; // simplistic
            player.stats.DefenseStat += it.defense;
            player.stats.Magic += it.magic;
            player.stats.Speed += it.speed;
            player.stats.Intelligence += it.intellect;
        }
    }
}
