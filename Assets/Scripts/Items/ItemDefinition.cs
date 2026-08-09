using System;
using UnityEngine;

[Serializable]
public class ItemDef
{
    public string id;
    public string displayName;
    public string description;
    public int rarity; // cast to ItemRarity
    public int category; // cast to ItemCategory
    public float weight;
    public int value;

    public ItemDef(string id, string displayName, string description = "", int rarity = 0, int category = 0, float weight = 1f, int value = 0)
    {
        this.id = id;
        this.displayName = displayName;
        this.description = description;
        this.rarity = rarity;
        this.category = category;
        this.weight = weight;
        this.value = value;
    }
}
