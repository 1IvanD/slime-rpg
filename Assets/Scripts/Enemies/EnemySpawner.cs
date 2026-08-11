using UnityEngine;
using System.Collections.Generic;

// Spawns placeholder enemies on WorldMap and in region scenes according to EnemyDef.homeNodeId.
public class EnemySpawner : MonoBehaviour
{
    public string enemyPrefabPath = "Prefabs/EnemyPlaceholder"; // Resources path

    private void Start()
    {
        SpawnAllOnMap();
    }

    public void SpawnAllOnMap()
    {
        var defs = Resources.LoadAll<EnemyDef>("Enemies");
        if (defs == null || defs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: No EnemyDef found in Resources/Enemies. Run Tools → Tempest → Generate Starter Enemies.");
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
            Debug.LogWarning($"EnemySpawner: MapNode not found for homeNodeId={def.homeNodeId} (enemy={def.id})");
            return;
        }

        // create simple marker: sphere + label
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        marker.name = "Enemy_" + def.id;
        Vector3 pos = node.transform.position + new Vector3(Random.Range(-0.5f,0.5f), 0.6f, Random.Range(-0.5f,0.5f));
        marker.transform.position = pos;
        marker.transform.localScale = Vector3.one * (def.boss ? 1.4f : 0.6f);
        marker.transform.SetParent(node.transform, true);

        var mr = marker.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.material.color = def.boss ? Color.red : Color.magenta;
        }

        // add a simple label
        GameObject label = new GameObject("Label");
        label.transform.SetParent(marker.transform, false);
        label.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        var txt = label.AddComponent<TextMesh>();
        txt.text = def.displayName;
        txt.characterSize = 0.12f;
        txt.anchor = TextAnchor.MiddleCenter;
        txt.color = Color.white;

        // attach EnemyDef reference for quick inspection
        var comp = marker.AddComponent<EnemyMarker>();
        comp.def = def;
    }
}
