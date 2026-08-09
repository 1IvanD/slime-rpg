using System.Collections.Generic;
using UnityEngine;

public class StomachCraftingSystem : MonoBehaviour
{
    public ItemsDatabase db;
    public StomachInventory stomach;

    private void Awake()
    {
        if (db == null) db = ItemsDatabase.Instance;
        if (stomach == null) stomach = FindObjectOfType<StomachInventory>();
    }

    public List<RecipeDef> GetCraftableRecipes(bool onlyStomach = true)
    {
        var outList = new List<RecipeDef>();
        if (db == null) return outList;
        foreach (var kv in db.GetAllRecipes())
        {
            var r = kv.Value;
            if (onlyStomach && !r.stomachCraftable) continue;
            if (CanCraft(r)) outList.Add(r);
        }
        return outList;
    }

    public bool CanCraft(RecipeDef r)
    {
        if (r == null || stomach == null) return false;
        foreach (var ing in r.ingredients)
        {
            if (!stomach.HasMaterial(ing.materialId, ing.count)) return false;
        }
        return true;
    }

    public bool Craft(RecipeDef r)
    {
        if (r == null || db == null || stomach == null) return false;
        if (!CanCraft(r)) return false;

        // consume
        foreach (var ing in r.ingredients) stomach.ConsumeMaterial(ing.materialId, ing.count);

        // give result to Inventory (default behavior)
        var item = db.GetItem(r.resultItemId);
        if (item != null && InventorySystem.Instance != null)
        {
            try
            {
                InventorySystem.Instance.AddItem(item.id, item.displayName, (ItemRarity)item.rarity, (ItemCategory)item.category, item.weight, r.resultCount, item.description, item.value);
                UIController.GetInstance()?.ShowNotification($"Сварено: {item.displayName} x{r.resultCount}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Crafting error: " + e.Message);
                return false;
            }
        }

        return false;
    }
}
