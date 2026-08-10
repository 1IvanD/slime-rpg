using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapManager : MonoBehaviour
{
    public static WorldMapManager Instance { get; private set; }

    [System.Serializable]
    public class MapLocation { public string id; public string displayName; public string sceneName; public Vector3 markerPosition; public bool discovered; }

    public List<MapLocation> locations = new List<MapLocation>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterLocation(string id, string displayName, string sceneName, Vector3 markerPosition)
    {
        if (locations.Exists(l => l.id == id)) return;
        locations.Add(new MapLocation { id = id, displayName = displayName, sceneName = sceneName, markerPosition = markerPosition, discovered = false });
    }

    public void Discover(string id)
    {
        var loc = locations.Find(l => l.id == id);
        if (loc != null) loc.discovered = true;
    }

    public void TravelTo(string id)
    {
        var loc = locations.Find(l => l.id == id);
        if (loc == null) { Debug.LogWarning($"Location {id} not found"); return; }
        if (!string.IsNullOrEmpty(loc.sceneName)) SceneManager.LoadScene(loc.sceneName);
    }
}
