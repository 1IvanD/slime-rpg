using System.Collections.Generic;
using UnityEngine;

// Simple data-driven in-code database for items and recipes.
// This avoids needing to create many ScriptableObjects; you can extend later.
public class ItemsDatabase : MonoBehaviour
{
    public static ItemsDatabase Instance { get; private set; }

    private Dictionary<string, ItemDef> items = new Dictionary<string, ItemDef>();
    private Dictionary<string, RecipeDef> recipes = new Dictionary<string, RecipeDef>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeItems();
        InitializeRecipes();
    }

    private void InitializeItems()
    {
        // Plants
        AddItem(new ItemDef("hipokute", "Хипокуте", "Лекарственная трава.", 1, 2, 0.1f, 5));
        AddItem(new ItemDef("hipokute_extract", "Экстракт Хипокуте", "Концентрированный экстракт.", 2, 2, 0.05f, 20));

        // Potions
        AddItem(new ItemDef("potion_basic", "Зелье", "Лечебное зелье.", 1, 3, 0.2f, 25));
        AddItem(new ItemDef("potion_full", "Полное зелье", "Мощное лечебное зелье.", 3, 3, 0.4f, 150));
        AddItem(new ItemDef("potion_improved", "Улучшенное зелье", "Улучшенная формула.", 2, 3, 0.3f, 80));

        // Ores and metals
        AddItem(new ItemDef("magic_ore", "Магическая руда", "Руда насыщенная магией.", 2, 4, 2f, 200));
        AddItem(new ItemDef("magisteel", "Магистил", "Рафинированная магическая руда.", 4, 4, 1.5f, 800));
        AddItem(new ItemDef("demon_steel", "Демоническая сталь", "Сталь с демонической энергией.", 4, 4, 2f, 1200));

        // Threads
        AddItem(new ItemDef("sticky_thread", "Sticky Thread", "Клейкая нить из паутины.", 2, 5, 0.05f, 10));
        AddItem(new ItemDef("steel_thread", "Steel Thread", "Прочная нить из стали.", 3, 5, 0.1f, 60));
        AddItem(new ItemDef("sticky_steel_thread", "Sticky-Steel Thread", "Комбинированная нить.", 4, 5, 0.12f, 250));

        // Monster materials
        AddItem(new ItemDef("dirvulf_fang", "Клык Дирвульфа", "Материал монстра.", 1, 6, 0.2f, 30));
        AddItem(new ItemDef("dirvulf_skin", "Шкура Дирвульфа", "Кожаный материал.", 2, 6, 3f, 150));

        // Special
        AddItem(new ItemDef("magikul", "Магикулы", "Редкий магический материал.", 5, 7, 0.5f, 2000));
        AddItem(new ItemDef("crystal_ball", "Хрустальный шар", "Инструмент для магии.", 4, 7, 1f, 500));
        AddItem(new ItemDef("antimagic_mask", "Антимагическая маска Сидзу", "Уникальный предмет.", 5, 7, 1f, 2000));

        // Tools / forge
        AddItem(new ItemDef("anvil", "Наковальня", "Кузнечное оборудование.", 3, 8, 50f, 500));
        AddItem(new ItemDef("hammer", "Молот", "Кузнечный молот.", 3, 8, 5f, 200));

        // Weapons sample
        AddItem(new ItemDef("katana_hakuro", "Катана Хакуро", "Уникальная катана.", 5, 9, 3f, 2500));
        AddItem(new ItemDef("rimuru_sword", "Меч Римуру", "Особый меч.", 5, 9, 3.5f, 3000));

        // Misc
        AddItem(new ItemDef("honey", "Мёд", "Полезный пищевой ресурс.", 1, 11, 0.5f, 15));
    }

    private void InitializeRecipes()
    {
        // Hipokute -> Extract
        var r1 = new RecipeDef("hip_extract", "Экстракт Хипокуте", "hipokute_extract");
        r1.ingredients.Add(new IngredientDef("hipokute", 3));
        r1.stomachCraftable = true;
        AddRecipe(r1);

        // Extract -> Potion
        var r2 = new RecipeDef("hip_potion", "Зелье из Хипокуте", "potion_basic");
        r2.ingredients.Add(new IngredientDef("hipokute_extract", 2));
        r2.stomachCraftable = true;
        AddRecipe(r2);

        // Potion -> Full Potion (example)
        var r3 = new RecipeDef("potion_full", "Полное зелье", "potion_full");
        r3.ingredients.Add(new IngredientDef("potion_basic", 3));
        r3.ingredients.Add(new IngredientDef("magikul", 1));
        r3.stomachCraftable = true;
        AddRecipe(r3);

        // Magic Ore -> Magisteel
        var r4 = new RecipeDef("magisteel_smelting", "Рафинирование Магической руды", "magisteel");
        r4.ingredients.Add(new IngredientDef("magic_ore", 5));
        r4.requiredStation = "Furnace";
        AddRecipe(r4);

        // Sticky thread -> Steel thread
        var r5 = new RecipeDef("steel_thread", "Steel Thread", "steel_thread");
        r5.ingredients.Add(new IngredientDef("sticky_thread", 2));
        r5.ingredients.Add(new IngredientDef("magic_ore", 1));
        r5.requiredStation = "Loom";
        AddRecipe(r5);

        // Sticky + Steel -> Sticky-Steel
        var r6 = new RecipeDef("sticky_steel", "Sticky-Steel Thread", "sticky_steel_thread");
        r6.ingredients.Add(new IngredientDef("sticky_thread", 1));
        r6.ingredients.Add(new IngredientDef("steel_thread", 1));
        r6.requiredStation = "Loom";
        AddRecipe(r6);

        // Monster skin -> leather (example)
        var r7 = new RecipeDef("leather_from_skin", "Кожа из шкуры", "dirvulf_skin");
        r7.ingredients.Add(new IngredientDef("dirvulf_skin", 1));
        r7.requiredStation = "Tannery";
        AddRecipe(r7);
    }

    public void AddItem(ItemDef it)
    {
        if (it == null || string.IsNullOrEmpty(it.id)) return;
        items[it.id] = it;
    }

    public ItemDef GetItem(string id)
    {
        if (items.TryGetValue(id, out var it)) return it;
        return null;
    }

    public void AddRecipe(RecipeDef r)
    {
        if (r == null || string.IsNullOrEmpty(r.id)) return;
        recipes[r.id] = r;
    }

    public RecipeDef GetRecipe(string id)
    {
        if (recipes.TryGetValue(id, out var r)) return r;
        return null;
    }

    public Dictionary<string, RecipeDef> GetAllRecipes() => recipes;
    public Dictionary<string, ItemDef> GetAllItems() => items;
}
