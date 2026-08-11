using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
