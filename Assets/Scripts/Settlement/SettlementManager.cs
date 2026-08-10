using System.Collections.Generic;
using UnityEngine;

public class SettlementManager : MonoBehaviour
{
    public static SettlementManager Instance { get; private set; }

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
