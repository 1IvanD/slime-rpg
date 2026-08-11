#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Editor utility to generate a WorldMap scene with placeholder MapNodes positioned roughly according to the reference map image.
public static class WorldMapGenerator
{
    [MenuItem("Tools/Tempest/Setup WorldMap Scene (placeholder)")]
    public static void SetupWorldMapScene()
    {
        // Create new empty scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Ensure folders
        System.IO.Directory.CreateDirectory("Assets/Scenes");

        // Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        camGO.transform.position = new Vector3(0, 10f, 0);
        camGO.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Light
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // WorldMap manager
        var mgrGO = new GameObject("WorldMapManager");
        mgrGO.AddComponent<WorldMapManager>();

        // Map plane / reference
        GameObject mapPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mapPlane.name = "MapReferencePlane";
        mapPlane.transform.localScale = new Vector3(1.2f, 1f, 1f); // adjust size
        mapPlane.transform.position = Vector3.zero;
        mapPlane.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Try to load reference texture from Resources/MapReference (without extension)
        var tex = Resources.Load<Texture2D>("MapReference");
        if (tex != null)
        {
            var mat = new Material(Shader.Find("Unlit/Texture"));
            mat.mainTexture = tex;
            var mr = mapPlane.GetComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
        }
        else
        {
            Debug.LogWarning("WorldMapGenerator: Resources/MapReference.png not found. Plane will use default material. Place your reference image at Assets/Resources/MapReference.png to show it on the map.");
            var mr = mapPlane.GetComponent<MeshRenderer>();
            mr.sharedMaterial.color = new Color(0.9f, 0.9f, 0.85f);
        }

        // Parent for nodes
        var nodesRoot = new GameObject("MapNodes");
        nodesRoot.transform.SetParent(mgrGO.transform, false);

        // Define list of nodes with normalized positions (0..1 across the map texture). These positions are approximate and intended as a starting point.
        (string id, string displayName, string sceneName, float nx, float ny)[] nodes = new[] {
            ("continent_ice","Continent of Eternal Ice","ContinentIce", 0.5f, 0.95f),
            ("dragon_peak","Dragon's Peak","DragonPeak", 0.65f, 0.82f),
            ("dwarf_kingdom","Dwarf Kingdom","DwarfKingdom", 0.62f, 0.78f),
            ("eastern_empire","Eastern Empire","EasternEmpire", 0.88f, 0.72f),
            ("jura_forest","Jura Forest","JuraForest", 0.72f, 0.52f),
            ("veldora_cave","Veldora Cave","VeldoraCave", 0.76f, 0.58f),
            ("tempest","Tempest (Settlement)","TempestTown", 0.5f, 0.46f),
            ("goblin_village","Goblin Village","GoblinVillage", 0.58f, 0.42f),
            ("wolf_lair","Wolf Lair","WolfLair", 0.82f, 0.36f),
            ("demon_domains","Demon Lord Domains","DemonDomains", 0.88f, 0.32f),
            ("various_west","Various Western States","WesternStates", 0.38f, 0.48f),
            ("brumund","Brumund Kingdom","BrumundKingdom", 0.52f, 0.50f),
            ("holy_ruberium","Holy Kingdom Ruberium","HolyRuberium", 0.24f, 0.62f),
            ("barren_lands","Barren Lands","BarrenLands", 0.12f, 0.5f),
            ("ulbresia","Ulbresia Republic","UlbresiaRepublic", 0.5f, 0.26f),
            ("herectic_dynasty","Heretic's Dynasty","HereticDynasty", 0.45f, 0.34f),
            ("farmas","Farmas Kingdom","FarmasKingdom", 0.55f, 0.58f)
        };

        // Map plane bounds: Unity Plane is 10 units x 10 units times localScale
        var planeSize = 10f * mapPlane.transform.localScale.x; // assuming uniform scale in X/Z
        foreach (var n in nodes)
        {
            // compute world pos from normalized coords: map origin bottom-left maps to plane's -5..+5 in X and Z
            float wx = (n.nx - 0.5f) * planeSize;
            float wz = (n.ny - 0.5f) * planeSize;
            Vector3 pos = new Vector3(wx, 0.5f, wz);

            var nodeGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nodeGO.name = "Node_" + n.id;
            nodeGO.transform.position = pos;
            nodeGO.transform.localScale = Vector3.one * 0.6f;
            nodeGO.transform.SetParent(nodesRoot.transform, true);

            var mn = nodeGO.AddComponent<MapNode>();
            mn.id = n.id;
            mn.displayName = n.displayName;
            mn.sceneName = n.sceneName;
            mn.description = n.displayName + " (placeholder)";

            // remove collider to avoid blocking scene camera movements, but keep for future interaction
            var col = nodeGO.GetComponent<Collider>();
            if (col != null) Object.DestroyImmediate(col);
        }

        // Save scene
        string scenePath = "Assets/Scenes/WorldMap.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("WorldMap scene created and saved at " + scenePath + ".\nPlace map reference at Assets/Resources/MapReference.png to use as background in the map plane. Replace node spheres with your prefabs/models as needed.");
    }
}
#endif
