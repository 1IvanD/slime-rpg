using System.Collections.Generic;
using UnityEngine;

public class WarManager : MonoBehaviour
{
    public static WarManager Instance { get; private set; }

    public List<ArmyDef> armies = new List<ArmyDef>();
    public float tickInterval = 5f; // seconds between simulation steps in Play mode

    private float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timer = tickInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SimulateStep();
            timer = tickInterval;
        }
    }

    // Very simple simulation: armies near the same node fight; winner keeps node and reduces troop counts
    private void SimulateStep()
    {
        // group armies by node
        var byNode = new Dictionary<string, List<ArmyDef>>();
        foreach (var a in armies)
        {
            if (string.IsNullOrEmpty(a.homeNodeId)) continue;
            if (!byNode.ContainsKey(a.homeNodeId)) byNode[a.homeNodeId] = new List<ArmyDef>();
            byNode[a.homeNodeId].Add(a);
        }

        foreach (var kv in byNode)
        {
            var list = kv.Value;
            if (list.Count < 2) continue; // nothing to fight

            // compute strength
            ArmyDef strongest = null;
            float best = -1f;
            foreach (var a in list)
            {
                float strength = a.troopCount * Mathf.Max(1f, a.averageLevel);
                if (strength > best) { best = strength; strongest = a; }
            }

            // losers lose troops proportional to comparison
            foreach (var a in list)
            {
                if (a == strongest) continue;
                // casualty ratio
                float loss = Mathf.Min(a.troopCount, Mathf.RoundToInt(a.troopCount * 0.5f));
                a.troopCount = Mathf.Max(0, a.troopCount - (int)loss);
            }

            // winner loses some as well
            strongest.troopCount = Mathf.Max(0, strongest.troopCount - Mathf.RoundToInt(best * 0.05f));
        }

        Debug.Log("WarManager: simulated a tick.");
    }

    public void RegisterArmy(ArmyDef a)
    {
        if (!armies.Contains(a)) armies.Add(a);
    }

    public void UnregisterArmy(ArmyDef a)
    {
        if (armies.Contains(a)) armies.Remove(a);
    }
}
