using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

#if UNITY_EDITOR
public static class PlaceMarkersInRegionScenes
{
    [MenuItem("Tools/Tempest/Place Markers In Region Scenes (WorldMap+Scenes)")]
    public static void Place()
    {
        // Ensure prefabs exist
        string prefabDir = "Assets/Resources/Prefabs";
        if (!Directory.Exists(prefabDir))
        {
            Debug.LogWarning("PlaceMarkersInRegionScenes: Prefab folder not found (Assets/Resources/Prefabs). Run Tools → Tempest → Create Placeholder Prefabs first.");
            return;
        }

        // find all region scenes
        string scenesDir = "Assets/Scenes/Regions";
        if (!Directory.Exists(scenesDir))
        {
            Debug.LogWarning("PlaceMarkersInRegionScenes: Scenes folder not found: Assets/Scenes/Regions. Create placeholder region scenes first.");
            return;
        }

        var sceneFiles = Directory.GetFiles(scenesDir, "*.unity", SearchOption.TopDirectoryOnly);
        int placed = 0;
        foreach (var s in sceneFiles)
        {
            var scene = EditorSceneManager.OpenScene(s, OpenSceneMode.Single);
            // find MapNode objects by name or by MapNode component
            var mapNodes = GameObject.FindObjectsOfType<MapNode>();
            if (mapNodes == null || mapNodes.Length == 0)
            {
                Debug.Log($"Scene {s} has no MapNode components — skipping marker placement.");
                EditorSceneManager.SaveScene(scene);
                continue;
            }

            // Spawn NPC markers
            var npcDefs = Resources.LoadAll<NPCDef>("NPCs");
            foreach (var def in npcDefs)
            {
                foreach (var mn in mapNodes)
                {
                    if (mn.id == def.homeNodeId)
                    {
                        // instantiate placeholder npc prefab at mn location
                        var prefab = Resources.Load<GameObject>("Prefabs/PlaceholderNPC");
                        GameObject marker = null;
                        if (prefab != null) marker = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                        else marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

                        marker.name = "NPC_" + def.id;
                        marker.transform.position = mn.transform.position + new Vector3(1f, 0.5f, 0f);
                        marker.transform.SetParent(scene.GetRootGameObjects()[0].transform, true);
                        placed++;
                    }
                }
            }

            // Spawn Enemy markers
            var enemyDefs = Resources.LoadAll<EnemyDef>("Enemies");
            foreach (var def in enemyDefs)
            {
                foreach (var mn in mapNodes)
                {
                    if (mn.id == def.homeNodeId)
                    {
                        var prefab = Resources.Load<GameObject>("Prefabs/EnemyCluster");
                        GameObject marker = null;
                        if (prefab != null) marker = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                        else marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                        marker.name = "Enemy_" + def.id;
                        marker.transform.position = mn.transform.position + new Vector3(-1f, 0.6f, 0f);
                        marker.transform.SetParent(scene.GetRootGameObjects()[0].transform, true);
                        placed++;
                    }
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        Debug.Log($"PlaceMarkersInRegionScenes: placed {placed} markers across {sceneFiles.Length} scenes.");
    }
}
#endif
