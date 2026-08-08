using UnityEngine;
using System.Collections.Generic;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance { get; private set; }

    private Dictionary<string, int> relations = new Dictionary<string, int>(); // -100..100
    private HashSet<string> enemies = new HashSet<string>();
    private HashSet<string> allies = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize common factions if desired
        relations["Goblins"] = 0;
        relations["Wolves"] = 0;
        relations["Orcs"] = 0;
        relations["Ogres"] = 0;
    }

    public void AddEnemy(string faction)
    {
        if (!enemies.Contains(faction)) enemies.Add(faction);
        if (allies.Contains(faction)) allies.Remove(faction);
        AdjustRelation(faction, -25);
        Debug.Log($"FactionManager: {faction} marked as enemy.");
    }

    public void AddAlly(string faction)
    {
        if (!allies.Contains(faction)) allies.Add(faction);
        if (enemies.Contains(faction)) enemies.Remove(faction);
        AdjustRelation(faction, +25);
        Debug.Log($"FactionManager: {faction} marked as ally.");
    }

    public int GetRelation(string faction)
    {
        return relations.TryGetValue(faction, out var v) ? v : 0;
    }

    public void AdjustRelation(string faction, int delta)
    {
        if (!relations.ContainsKey(faction)) relations[faction] = 0;
        relations[faction] = Mathf.Clamp(relations[faction] + delta, -100, 100);
    }

    public bool IsEnemy(string faction) => enemies.Contains(faction);
    public bool IsAlly(string faction) => allies.Contains(faction);

    // Optional: expose lists
    public Dictionary<string,int> GetAllRelations() => relations;
}
