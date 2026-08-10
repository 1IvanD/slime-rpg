using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Combat/LootTable", fileName = "LootTableSO")]
public class LootTableSO : ScriptableObject
{
    [System.Serializable]
    public class LootEntry
n    {
        public string itemId;
        [Range(0f,1f)] public float chance = 1f; // probability to drop
        public int minAmount = 1;
        public int maxAmount = 1;
    }

    public LootEntry[] entries = new LootEntry[0];

    public LootEntry[] GetDrops()
    {
        // simple evaluation — caller should decide how to apply randomness per entry
        return entries;
    }
}
