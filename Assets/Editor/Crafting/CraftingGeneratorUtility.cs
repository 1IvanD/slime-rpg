#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class CraftingGeneratorUtility
{
    private const string baseFolder = "Assets/Data/Crafting";

    [MenuItem("Tools/Tempest/Generate Key Crafting Items & Recipes (Season1)")]
    public static void GenerateKeyCrafting()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder(baseFolder))
            AssetDatabase.CreateFolder("Assets/Data", "Crafting");

        string itemsFolder = Path.Combine(baseFolder, "Items");
        string recipesFolder = Path.Combine(baseFolder, "Recipes");

        if (!AssetDatabase.IsValidFolder(itemsFolder)) AssetDatabase.CreateFolder(baseFolder, "Items");
        if (!AssetDatabase.IsValidFolder(recipesFolder)) AssetDatabase.CreateFolder(baseFolder, "Recipes");

        // Items
        ItemSO hip = CreateItem("Item_Hipokute", "Hipokute", "A common medicinal herb.");
        ItemSO extract = CreateItem("Item_Hipokute_Extract", "Hipokute Extract", "Concentrated medicinal extract.");
        ItemSO potion = CreateItem("Item_Potion", "Potion", "Standard healing potion.");
        ItemSO fullPotion = CreateItem("Item_FullPotion", "Full Potion", "Powerful healing potion.");

        ItemSO sticky = CreateItem("Item_StickyThread", "Sticky Thread", "Thread harvested from spider silk.");
        ItemSO steel = CreateItem("Item_SteelThread", "Steel Thread", "Reinforced metal thread.");
        ItemSO stickySteel = CreateItem("Item_StickySteelThread", "Sticky-Steel Thread", "Composite thread combining stickiness and strength.");

        ItemSO magOre = CreateItem("Item_MagicalOre", "Magical Ore", "Raw magical ore (can be refined to Magisteel).", ItemType.Material);
        ItemSO magIngot = CreateItem("Item_MagisteelIngot", "Magisteel Ingot", "Refined magisteel ingot.", ItemType.Material);
        ItemSO magSword = CreateItem("Item_MagisteelSword", "Magisteel Sword", "A sword forged with a magisteel core.", ItemType.Equipment);

        // Recipes
        // Hipokute -> Extract (alchemy)
        RecipeSO r1 = CreateRecipe("Recipe_Hipokute_to_Extract", "Hipokute -> Extract", "Process Hipokute into a medicinal extract.", 6f, RecipeSO.StationType.Alchemy);
        r1.inputs = new Ingredient[] { new Ingredient { itemId = hip.id, amount = 3 } };
        r1.outputs = new Ingredient[] { new Ingredient { itemId = extract.id, amount = 1 } };
        r1.allowBulk = true;

        // Hipokute -> Potion
        RecipeSO r2 = CreateRecipe("Recipe_Extract_to_Potion", "Extract -> Potion", "Brew a potion from extract.", 10f, RecipeSO.StationType.Alchemy);
        r2.inputs = new Ingredient[] { new Ingredient { itemId = extract.id, amount = 1 } };
        r2.outputs = new Ingredient[] { new Ingredient { itemId = potion.id, amount = 1 } };

        // Hipokute -> Full Potion (advanced)
        RecipeSO r3 = CreateRecipe("Recipe_Extract_to_FullPotion", "Extract -> Full Potion", "Produce a powerful full potion.", 25f, RecipeSO.StationType.Alchemy);
        r3.inputs = new Ingredient[] { new Ingredient { itemId = extract.id, amount = 4 } };
        r3.outputs = new Ingredient[] { new Ingredient { itemId = fullPotion.id, amount = 1 } };

        // Sticky Thread (from spider silk) - assume resource gather -> create item
        // Here recipe uses no inputs (gathered) but we make a simple conversion for demonstration
        RecipeSO r4 = CreateRecipe("Recipe_Make_StickyThread", "Make Sticky Thread", "Process spider silk into sticky thread.", 4f, RecipeSO.StationType.General);
        r4.inputs = new Ingredient[] { new Ingredient { itemId = "Item_SpiderSilk", amount = 2 } };
        r4.outputs = new Ingredient[] { new Ingredient { itemId = sticky.id, amount = 1 } };

        // Sticky -> Steel Thread (combine with iron/metal)
        RecipeSO r5 = CreateRecipe("Recipe_Sticky_to_SteelThread", "Sticky -> Steel Thread", "Combine sticky thread with metal to make steel thread.", 12f, RecipeSO.StationType.Blacksmith);
        r5.inputs = new Ingredient[] { new Ingredient { itemId = sticky.id, amount = 2 }, new Ingredient { itemId = "Item_IronIngot", amount = 1 } };
        r5.outputs = new Ingredient[] { new Ingredient { itemId = steel.id, amount = 1 } };

        // Sticky + Steel -> Sticky-Steel Thread
        RecipeSO r6 = CreateRecipe("Recipe_Combine_StickySteel", "Combine Sticky & Steel", "Combine threads into composite.", 6f, RecipeSO.StationType.General);
        r6.inputs = new Ingredient[] { new Ingredient { itemId = sticky.id, amount = 1 }, new Ingredient { itemId = steel.id, amount = 1 } };
        r6.outputs = new Ingredient[] { new Ingredient { itemId = stickySteel.id, amount = 1 } };

        // Magical ore -> Magisteel Ingot (smelting/refining)
        RecipeSO r7 = CreateRecipe("Recipe_MagOre_to_MagIngot", "Magical Ore -> Magisteel Ingot", "Refine magical ore into magisteel.", 40f, RecipeSO.StationType.Blacksmith);
        r7.inputs = new Ingredient[] { new Ingredient { itemId = magOre.id, amount = 3 } };
        r7.outputs = new Ingredient[] { new Ingredient { itemId = magIngot.id, amount = 1 } };

        // Magisteel Ingot -> Magisteel Sword
        RecipeSO r8 = CreateRecipe("Recipe_MagIngot_to_MagSword", "Magisteel Ingot -> Magisteel Sword", "Forge a magisteel sword.", 30f, RecipeSO.StationType.Blacksmith);
        r8.inputs = new Ingredient[] { new Ingredient { itemId = magIngot.id, amount = 2 }, new Ingredient { itemId = "Item_SteelIngot", amount = 1 } };
        r8.outputs = new Ingredient[] { new Ingredient { itemId = magSword.id, amount = 1 } };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Tempest: Generated key crafting Items and Recipes under Assets/Data/Crafting.\nYou can edit created assets in the inspector and use AlchemyStation/BlacksmithStation to enqueue recipes for testing.");
    }

    private static ItemSO CreateItem(string id, string name, string desc, ItemType type = ItemType.Resource)
    {
        var it = ScriptableObject.CreateInstance<ItemSO>();
        it.id = id;
        it.displayName = name;
        it.description = desc;
        it.itemType = type;
        it.maxStack = 99;
        string path = Path.Combine(baseFolder, "Items", id + ".asset");
        AssetDatabase.CreateAsset(it, path);
        return it;
    }

    private static RecipeSO CreateRecipe(string id, string name, string desc, float time, RecipeSO.StationType station)
    {
        var r = ScriptableObject.CreateInstance<RecipeSO>();
        r.id = id;
        r.displayName = name;
        r.description = desc;
        r.craftTime = time;
        r.station = station;
        string path = Path.Combine(baseFolder, "Recipes", id + ".asset");
        AssetDatabase.CreateAsset(r, path);
        return r;
    }
}
#endif
