#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class GenerateStarterItemsUtility
{
    [MenuItem("Tools/Tempest/Generate Starter Items (Resources/Items)")]
    public static void GenerateItems()
    {
        string dir = "Assets/Resources/Items";
        if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder(dir)) AssetDatabase.CreateFolder("Assets/Resources", "Items");

        ItemSO hip = CreateItem("hipokute", "Hipokute", "A common medicinal herb.");
        ItemSO extract = CreateItem("hipokute_extract", "Hipokute Extract", "Concentrated extract.");
        ItemSO potion = CreateItem("potion_health", "Potion", "Heals 50 HP."); potion.itemType = TempestItemType.Consumable;
        ItemSO fullPotion = CreateItem("potion_full", "Full Potion", "Heals 300 HP."); fullPotion.itemType = TempestItemType.Consumable; fullPotion.rarity = ItemRarity.Rare;
        ItemSO sticky = CreateItem("sticky_thread", "Sticky Thread", "Thread from spider silk.");
        ItemSO magSword = CreateItem("magisteel_sword", "Magisteel Sword", "A sword forged with magisteel core."); magSword.itemType = TempestItemType.Equipment; magSword.attack = 20; magSword.equipSlot = "Weapon"; magSword.rarity = ItemRarity.Epic;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Generated starter items under Assets/Resources/Items. Use ItemsDatabase (auto-loads from Resources/Items).");
    }

    private static ItemSO CreateItem(string id, string name, string desc)
    {
        var it = ScriptableObject.CreateInstance<ItemSO>();
        it.id = id;
        it.displayName = name;
        it.description = desc;
        string path = Path.Combine("Assets/Resources/Items", id + ".asset");
        AssetDatabase.CreateAsset(it, path);
        return it;
    }
}
#endif
