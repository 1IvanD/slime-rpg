#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CaveAwakeningSetup
{
    [MenuItem("Tools/Setup/Cave Awakening Scene")]
    public static void CreateCaveScene()
    {
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Resources/Prefabs");

        // New scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Dark ambient
        RenderSettings.ambientIntensity = 0.2f;

        // Camera
        var camGO = new GameObject("Main Camera");
        camGO.AddComponent<Camera>();
        camGO.tag = "MainCamera";
        camGO.transform.position = new Vector3(0, 2, -5);
        camGO.AddComponent<AudioListener>();

        // Light (dim)
        var lightGO = new GameObject("PointLight");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 10f;
        light.intensity = 0.8f;
        lightGO.transform.position = new Vector3(0, 1.5f, 0);

        // Floor (cave)
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "CaveFloor";
        floor.transform.localScale = new Vector3(5, 1, 5);

        // Spawn point
        var spawn = new GameObject("PlayerSpawn");
        spawn.transform.position = new Vector3(0, 1f, 0);

        // Add GameManager, QuestManager
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
        gm.AddComponent<QuestManager>();

        // Create Player prefab into Resources/Prefabs if not exists
        string prefabPath = "Assets/Resources/Prefabs/Player_Cave.prefab";
        if (!System.IO.File.Exists(prefabPath))
        {
            var playerGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            playerGO.name = "Player_Slime";
            playerGO.transform.position = spawn.transform.position;
            var playerComp = playerGO.AddComponent<Player>();
            playerComp.displayName = "Rimuru";
            playerGO.AddComponent<PlayerAbilities>();
            PrefabUtility.SaveAsPrefabAsset(playerGO, prefabPath);
            GameObject.DestroyImmediate(playerGO);
        }

        // Veldora NPC with barrier
        var veldora = GameObject.CreatePrimitive(PrimitiveType.Cube);
        veldora.name = "Veldora";
        veldora.transform.position = new Vector3(0, 1f, 10f);
        veldora.transform.localScale = new Vector3(4, 4, 4);
        var veldoraComp = veldora.AddComponent<VeldoraNPC>();
        var barrier = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        barrier.name = "Barrier";
        barrier.transform.position = veldora.transform.position;
        barrier.transform.localScale = new Vector3(6, 6, 6);
        var collider = barrier.GetComponent<Collider>();
        collider.isTrigger = true;

        // Magic Stone
        var stone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stone.name = "MagicStone";
        stone.transform.position = new Vector3(3f, 0.5f, 2f);
        var ms = stone.AddComponent<MagicStone>();
        ms.description = "Магический камень с мягким сиянием.";

        // Absorbable plant
        var plant = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        plant.name = "AbsorbablePlant";
        plant.transform.position = new Vector3(-2f, 0.5f, -1f);
        var ab = plant.AddComponent<Absorbable>();
        ab.resourceName = "Сырой органический материал";

        // Simple enemy
        var enemy = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        enemy.name = "Bat_01";
        enemy.transform.position = new Vector3(2f, 0.5f, -3f);
        enemy.AddComponent<SimpleEnemy>();

        // Add InternalVoice and TutorialController
        var iv = new GameObject("InternalVoice");
        iv.AddComponent<InternalVoice>();
        var tut = new GameObject("TutorialController");
        tut.AddComponent<TutorialController>();

        // Save scene
        string scenePath = "Assets/Scenes/CaveAwakening.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        EditorUtility.DisplayDialog("Cave Setup", "CaveAwakening scene created at Assets/Scenes/CaveAwakening.unity", "OK");
    }
}
#endif
