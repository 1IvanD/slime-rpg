using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public static ItemsDatabase Instance { get; private set; }
    private Dictionary<string, ItemSO> items = new Dictionary<string, ItemSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAll();
    }

    public void LoadAll()
    {
        items.Clear();
        // Load from Resources/Items (editor utility creates assets there)
        var arr = Resources.LoadAll<ItemSO>("Items");
        foreach (var it in arr)
        {
            if (it == null || string.IsNullOrEmpty(it.id)) continue;
            items[it.id] = it;
        }
        Debug.Log($"ItemsDatabase: Loaded {items.Count} items from Resources/Items");
    }

    public ItemSO GetItem(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        items.TryGetValue(id, out var it);
        return it;
    }

    public Dictionary<string, ItemSO> GetAll() => new Dictionary<string, ItemSO>(items);
}
