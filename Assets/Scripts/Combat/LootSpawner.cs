using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public static LootSpawner Instance { get; private set; }

    public string defaultDropItemId = "herb_rare";
    public int defaultDropAmount = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnLoot(Vector3 atPosition)
    {
        // Try to add to InventorySystem of nearest player
        var player = FindObjectOfType<Player>();
        if (player != null && InventorySystem.Instance != null)
        {
            InventorySystem.Instance.AddItem(defaultDropItemId, defaultDropItemId, ItemRarity.Common, ItemCategory.Resource, 0.1f, defaultDropAmount, "Looted item", 0);
            UIController.GetInstance()?.ShowNotification($"Получено лут: {defaultDropItemId} x{defaultDropAmount}");
            return;
        }
        // otherwise just log
        Debug.Log($"LootSpawner: dropped {defaultDropItemId} at {atPosition}");
    }
}
