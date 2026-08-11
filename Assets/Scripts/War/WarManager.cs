using System.Collections.Generic;
using UnityEngine;

public partial class WarManager : MonoBehaviour
{
    // Force control of a node by a faction (deterministic outcome)
    public void ForceControl(string nodeId, string winningFaction)
    {
        if (string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(winningFaction)) return;

        ArmyDef winnerArmy = null;
        // find one army of the winning faction at node
        foreach (var a in armies)
        {
            if (a.homeNodeId == nodeId && !string.IsNullOrEmpty(a.faction) && a.faction.ToLower().Contains(winningFaction.ToLower()))
            {
                winnerArmy = a;
                break;
            }
        }

        // if no existing winner army, try to find any army of that faction and move it here
        if (winnerArmy == null)
        {
            foreach (var a in armies)
            {
                if (!string.IsNullOrEmpty(a.faction) && a.faction.ToLower().Contains(winningFaction.ToLower()))
                {
                    winnerArmy = a;
                    winnerArmy.homeNodeId = nodeId;
                    break;
                }
            }
        }

        // eliminate or reduce other armies at this node
        foreach (var a in armies)
        {
            if (a.homeNodeId != nodeId) continue;
            if (a == winnerArmy) continue;
            // losers wiped out
            a.troopCount = 0;
        }

        if (winnerArmy != null)
        {
            // winner loses some troops but retains presence
            int loss = Mathf.RoundToInt(winnerArmy.troopCount * 0.1f);
            winnerArmy.troopCount = Mathf.Max(1, winnerArmy.troopCount - loss);
        }

        Debug.Log($"WarManager.ForceControl: {winningFaction} now controls {nodeId}");
    }

    // Deterministic single-step battle simulation for a node. Returns winning faction name or null.
    public string SimulateBattleAtNode(string nodeId, int simulationSteps = 1)
    {
        if (string.IsNullOrEmpty(nodeId)) return null;

        // gather armies at node
        var list = new List<ArmyDef>();
        foreach (var a in armies)
        {
            if (a.homeNodeId == nodeId) list.Add(a);
        }

        if (list.Count == 0) return null;
        if (list.Count == 1) return list[0].faction;

        // compute faction strengths
        var strengthByFaction = new Dictionary<string, float>();
        foreach (var a in list)
        {
            string f = string.IsNullOrEmpty(a.faction) ? "Unknown" : a.faction;
            float s = a.troopCount * Mathf.Max(1f, a.averageLevel);
            if (!strengthByFaction.ContainsKey(f)) strengthByFaction[f] = 0f;
            strengthByFaction[f] += s;
        }

        // pick winner (highest strength) deterministically
        string winner = null; float best = -1f;
        foreach (var kv in strengthByFaction)
        {
            if (kv.Value > best) { best = kv.Value; winner = kv.Key; }
        }

        // apply casualties: losers lose 50% troopCount, winner loses 5% (example constants)
        foreach (var a in list)
        {
            if (a.faction == winner)
            {
                int lost = Mathf.RoundToInt(a.troopCount * 0.05f);
                a.troopCount = Mathf.Max(1, a.troopCount - lost);
            }
            else
            {
                int lost = Mathf.RoundToInt(a.troopCount * 0.5f);
                a.troopCount = Mathf.Max(0, a.troopCount - lost);
            }
        }

        Debug.Log($"WarManager.SimulateBattleAtNode: node={nodeId} winner={winner}");
        return winner;
    }
}
