using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WorldMapManager : MonoBehaviour
{
    public static WorldMapManager Instance { get; private set; }

    private Dictionary<string, MapNode> nodes = new Dictionary<string, MapNode>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // register existing nodes in scene
        var all = FindObjectsOfType<MapNode>();
        foreach (var n in all)
        {
            if (!string.IsNullOrEmpty(n.id)) nodes[n.id] = n;
        }
    }

    public void RegisterNode(MapNode node)
    {
        if (node == null || string.IsNullOrEmpty(node.id)) return;
        nodes[node.id] = node;
    }

    public MapNode GetNode(string id)
    {
        nodes.TryGetValue(id, out var n);
        return n;
    }

    public List<MapNode> GetAllNodes()
    {
        return new List<MapNode>(nodes.Values);
    }

    // TryTravelTo performs prerequisite checks (campaign/quest) and returns true if travel is allowed.
    public bool TryTravelTo(string nodeId)
    {
        var n = GetNode(nodeId);
        if (n == null)
        {
            Debug.LogWarning($"WorldMapManager: node {nodeId} not found");
            return false;
        }

        // If there are campaign events targeting this node that require a quest, check them.
        var events = Resources.LoadAll<WarEventSO>("CampaignEvents");
        if (events != null)
        {
            foreach (var ev in events)
            {
                if (ev == null) continue;
                if (ev.targetNodeId == nodeId && !string.IsNullOrEmpty(ev.requiredQuestId))
                {
                    if (QuestManager.Instance == null || !QuestManager.Instance.IsQuestCompleted(ev.requiredQuestId))
                    {
                        WorldMapUI.Instance?.ShowNotification($"Нельзя перейти: требуется завершить квест '{ev.requiredQuestId}'");
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void TravelTo(string nodeId)
    {
        var n = GetNode(nodeId);
        if (n == null)
        {
            Debug.LogWarning($"WorldMapManager: node {nodeId} not found");
            return;
        }
        if (string.IsNullOrEmpty(n.sceneName))
        {
            Debug.LogWarning($"WorldMapManager: node {nodeId} has no sceneName assigned (placeholder)");
            return;
        }

        // check prerequisites
        if (!TryTravelTo(nodeId)) return;

        // For now do a simple synchronous load of the sceneName (user should create these scenes later)
        try
        {
            SceneManager.LoadScene(n.sceneName);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"WorldMapManager: failed to load scene '{n.sceneName}'. Make sure a scene with this name exists in Build Settings. Exception: {ex.Message}");
        }
    }

    // Highlight node visually (simple pulse) and optionally focus camera if available
    public void HighlightNode(string nodeId, float duration = 2f)
    {
        var n = GetNode(nodeId);
        if (n == null) return;
        StartCoroutine(PulseNode(n.gameObject, duration));
    }

    private IEnumerator PulseNode(GameObject go, float duration)
    {
        if (go == null) yield break;
        var start = go.transform.localScale;
        var t = 0f;
        while (t < duration)
        {
            float phase = Mathf.Sin(t * Mathf.PI * 2f);
            float scale = 1f + 0.15f * phase;
            go.transform.localScale = start * scale;
            t += Time.deltaTime;
            yield return null;
        }
        go.transform.localScale = start;
    }
}
