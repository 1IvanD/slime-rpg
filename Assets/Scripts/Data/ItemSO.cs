using UnityEngine;

public enum ItemType
{
    Resource,
    Consumable,
    Material,
    Equipment,
    Component,
    Currency
}

[CreateAssetMenu(menuName = "Tempest/Data/Item", fileName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public ItemType itemType = ItemType.Resource;
    public Sprite icon;
    public int maxStack = 99;

    // craft-related tags
    public bool isCraftable = false;
}
