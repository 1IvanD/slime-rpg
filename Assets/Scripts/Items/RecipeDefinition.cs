using System;
using System.Collections.Generic;

[Serializable]
public class IngredientDef
{
    public string materialId;
    public int count;

    public IngredientDef(string id, int c)
    {
        materialId = id;
        count = c;
    }
}

[Serializable]
public class RecipeDef
{
    public string id;
    public string displayName;
    public string resultItemId;
    public int resultCount = 1;
    public List<IngredientDef> ingredients = new List<IngredientDef>();
    public bool stomachCraftable = false;
    public string requiredStation = ""; // e.g., Anvil, Furnace

    public RecipeDef(string id, string displayName, string resultItemId)
    {
        this.id = id;
        this.displayName = displayName;
        this.resultItemId = resultItemId;
    }
}
