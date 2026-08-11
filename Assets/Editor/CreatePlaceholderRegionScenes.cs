#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class CreatePlaceholderRegionScenes
{
    [MenuItem("Tools/Tempest/Create Placeholder Region Scenes (Regions)")]
    public static void CreateScenes()
    {
        string scenesDir = "Assets/Scenes/Regions";
        if (!Directory.Exists(scenesDir)) Directory.CreateDirectory(scenesDir);

        string[] sceneNames = new[] {
            "ContinentIce",
            "DragonPeak",
            "DwarfKingdom",
            "EasternEmpire",
            "JuraForest",
            "VeldoraCave",
            "TempestTown",
            "GoblinVillage",
            "WolfLair",
            "DemonDomains",
            "WesternStates",
            "BrumundKingdom",
            "HolyRuberium",
            "BarrenLands",
            "UlbresiaRepublic",
            "HereticDynasty",
            "FarmasKingdom"
        };

        var buildScenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        int createdCount = 0;

        foreach (var sname in sceneNames)
        {
            string path = Path.Combine(scenesDir, sname + ".unity");
            // if scene already exists, skip creating but ensure it's in Build Settings
            if (File.Exists(path))
            {
                Debug.Log($"Scene already exists: {path}");
                // ensure in build settings
                AddSceneToBuildIfMissing(path, ref buildScenes);
                continue;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Camera
            var camGO = new GameObject("Main Camera");
            var cam = camGO.AddComponent<Camera>();
            cam.tag = "MainCamera";
            camGO.transform.position = new Vector3(0f, 5f, -10f);
            camGO.transform.rotation = Quaternion.Euler(10f, 0f, 0f);

            // Light
            var lightGO = new GameObject("Directional Light");
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Ground plane
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(5f, 1f, 5f);

            // Player spawn
            var spawn = new GameObject("PlayerSpawn");
            spawn.transform.position = new Vector3(0f, 1f, 0f);

            // Scene info object
            var info = new GameObject("SceneInfo");
            var si = info.AddComponent<SceneInfoPlaceholder>();
            si.sceneDisplayName = sname;

            // NPCs root
            var npcs = new GameObject("NPCs");

            // Save scene
            EditorSceneManager.SaveScene(scene, path);
            Debug.Log($"Created placeholder scene: {path}");
            AddSceneToBuildIfMissing(path, ref buildScenes);
            createdCount++;
        }

        // update Build Settings
        EditorBuildSettings.scenes = buildScenes.ToArray();
        AssetDatabase.Refresh();
        Debug.Log($"CreatePlaceholderRegionScenes: done. Created or ensured {sceneNames.Length} scenes. Newly created: {createdCount}.");
    }

    private static void AddSceneToBuildIfMissing(string path, ref System.Collections.Generic.List<EditorBuildSettingsScene> list)
    {
        foreach (var es in list)
        {
            if (es.path == path) return;
        }
        list.Add(new EditorBuildSettingsScene(path, true));
    }
}
#endif
