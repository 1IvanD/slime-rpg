using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public string itemId;
    [Range(0f,1f)] public float chance = 1f;
    public int minAmount = 1;
    public int maxAmount = 1;
}

[CreateAssetMenu(menuName = "Tempest/Data/LootTable", fileName = "LootTableSO")]
public class LootTableSO : ScriptableObject
{
    public List<LootEntry> entries = new List<LootEntry>();
}
