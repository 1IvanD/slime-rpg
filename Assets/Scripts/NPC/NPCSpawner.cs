using UnityEngine;

// Spawns placeholder NPCs on WorldMap and in region scenes according to NPCDef.homeNodeId.
public class NPCSpawner : MonoBehaviour
{
    public bool spawnInWorldMap = true;
    public bool spawnInRegionScenes = false; // placeholder scenes spawn optional

    private void Start()
    {
        if (spawnInWorldMap) SpawnAllInWorldMap();
    }

    public void SpawnAllInWorldMap()
    {
        var defs = Resources.LoadAll<NPCDef>("NPCs");
        if (defs == null || defs.Length == 0)
        {
            Debug.LogWarning("NPCSpawner: No NPCDef found in Resources/NPCs. Run Tools → Tempest → Generate Starter NPCs.");
            return;
        }

        foreach (var def in defs)
        {
            SpawnNPCMarker(def);
        }
    }

    private void SpawnNPCMarker(NPCDef def)
    {
        if (def == null) return;
        var node = WorldMapManager.Instance?.GetNode(def.homeNodeId);
        if (node == null)
        {
            Debug.LogWarning($"NPCSpawner: MapNode not found for homeNodeId={def.homeNodeId} (npc={def.id})");
            return;
        }

        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.name = "NPC_" + def.id;
        Vector3 pos = node.transform.position + new Vector3(Random.Range(-0.6f,0.6f), 0.5f, Random.Range(-0.6f,0.6f));
        marker.transform.position = pos;
        marker.transform.localScale = Vector3.one * 0.4f;
        marker.transform.SetParent(node.transform, true);

        var mr = marker.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.material.color = def.important ? Color.yellow : Color.cyan;
        }

        // add a simple label
        GameObject label = new GameObject("Label");
        label.transform.SetParent(marker.transform, false);
        label.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        var txt = label.AddComponent<TextMesh>();
        txt.text = def.displayName;
        txt.characterSize = 0.09f;
        txt.anchor = TextAnchor.MiddleCenter;
        txt.color = Color.white;

        var comp = marker.AddComponent<NPCMarker>();
        comp.def = def;
    }
}
