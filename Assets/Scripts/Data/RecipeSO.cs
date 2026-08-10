using System;
using UnityEngine;

[Serializable]
public struct Ingredient
{
    public string itemId;
    public int amount;
}

[CreateAssetMenu(menuName = "Tempest/Crafting/Recipe", fileName = "RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public Ingredient[] inputs = new Ingredient[0];
    public Ingredient[] outputs = new Ingredient[0];

    [Tooltip("Crafting time in seconds")]
    public float craftTime = 5f;

    public enum StationType { Alchemy, Blacksmith, General }
    public StationType station = StationType.General;

    [Tooltip("Allow mass-production (process multiple at once)")]
    public bool allowBulk = false;
}
