using System.Collections.Generic;
using UnityEngine;

// Legacy SettlementManager kept to preserve editor utilities; renamed to avoid collision with canonical SettlementManager.
public class SettlementManager_Legacy : MonoBehaviour
{
    public static SettlementManager_Legacy Instance { get; private set; }

    public List<BuildingSO> buildings = new List<BuildingSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddBuilding(BuildingSO b)
    {
        if (b == null) return;
        buildings.Add(b);
        UIController.GetInstance()?.ShowNotification($"Построено: {b.displayName}");
    }

    public void UpgradeBuilding(string id)
    {
        var b = buildings.Find(x => x.id == id);
        if (b == null) return;
        if (b.level < b.maxLevel) { b.level++; UIController.GetInstance()?.ShowNotification($"Улучшено: {b.displayName} до уровня {b.level}"); }
    }
}
