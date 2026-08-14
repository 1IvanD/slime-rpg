using UnityEngine;
using System.Collections.Generic;

// Legacy EnemySpawner that was used for WorldMap markers. Renamed to avoid collision with runtime Combat/EnemySpawner.
public class EnemySpawner_Legacy : MonoBehaviour
{
    public string enemyPrefabPath = "Prefabs/EnemyCluster"; // Resources path
    public string orcPrefabPath = "Prefabs/OrcCluster";

    private void Start()
    {
        SpawnAllOnMap();
    }

    public void SpawnAllOnMap()
    {
        var defs = Resources.LoadAll<EnemyDef>("Enemies");
        if (defs == null || defs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner_Legacy: No EnemyDef found in Resources/Enemies. Run Tools → Tempest → Generate Starter Enemies.");
            return;
        }

        foreach (var def in defs)
        {
            SpawnEnemyMarker(def);
        }
    }

    private void SpawnEnemyMarker(EnemyDef def)
    {
        if (def == null) return;
        var node = WorldMapManager.Instance?.GetNode(def.homeNodeId);
        if (node == null)
        {
            Debug.LogWarning($"EnemySpawner_Legacy: MapNode not found for homeNodeId={def.homeNodeId} (enemy={def.id})");
            return;
        }

        GameObject marker = null;

        // Prefer special orc prefab for orc faction if available
        if (!string.IsNullOrEmpty(def.faction) && def.faction.ToLower().Contains("orc"))
        {
            var prefab = Resources.Load<GameObject>(orcPrefabPath);
            if (prefab != null)
            {
                marker = GameObject.Instantiate(prefab, node.transform);
                marker.name = "Enemy_" + def.id;
            }
        }

        if (marker == null)
        {
            // fallback to simple capsule
            marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            marker.name = "Enemy_" + def.id;
            var mr = marker.GetComponent<MeshRenderer>();
            if (mr != null) mr.material.color = def.boss ? Color.red : Color.magenta;
            marker.transform.SetParent(node.transform, true);
        }

        Vector3 pos = node.transform.position + new Vector3(Random.Range(-0.5f,0.5f), 0.6f, Random.Range(-0.5f,0.5f));
        marker.transform.position = pos;

        float scale = 0.6f + Mathf.Log10(Mathf.Max(1, def.troopCount)) * 0.15f;
        if (def.boss) scale *= 1.6f;
        marker.transform.localScale = Vector3.one * scale;

        // add a label with formatted troopCount
        GameObject label = new GameObject("Label");
        label.transform.SetParent(marker.transform, false);
        label.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        var txt = label.AddComponent<TextMesh>();
        txt.text = def.troopCount > 1 ? NumberFormatter.FormatCount(def.troopCount) : def.displayName;
        txt.characterSize = 0.12f;
        txt.anchor = TextAnchor.MiddleCenter;
        txt.color = Color.white;

        var comp = marker.AddComponent<EnemyMarker>();
        comp.def = def;
    }
}
