using UnityEngine;

public enum TempestItemType { Resource, Consumable, Equipment, Artifact }

[CreateAssetMenu(menuName = "Tempest/Data/Item", fileName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public TempestItemType itemType = TempestItemType.Resource;
    public Sprite icon;

    [Header("Stats (for equipment)")]
    public int attack = 0;
    public int defense = 0;
    public int magic = 0;
    public int speed = 0;
    public int intellect = 0;

    public int maxStack = 99;
    public ItemRarity rarity = ItemRarity.Common;
    public string equipSlot; // e.g., "Weapon", "Chest", "Accessory"

    [Header("Physical & Economic")]
    public float weight = 0.1f;
    public float value = 10f;

    [Header("Consumable Effects")]
    public EffectType effectType = EffectType.None;
    public float healAmount = 0f;
    public float buffDuration = 0f; // placeholder for future
    public float buffStrength = 0f; // placeholder for future
}

public enum EffectType { None, Heal, Buff }
