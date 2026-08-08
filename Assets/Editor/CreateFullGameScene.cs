#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public static class CreateFullGameScene
{
    [MenuItem("Tools/Setup/Create Full Game Scene")]
    public static void CreateScene()
    {
        // create directories
        System.IO.Directory.CreateDirectory("Assets/Scenes");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Ambient
        RenderSettings.ambientIntensity = 0.6f;

        // Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        camGO.tag = "MainCamera";
        cam.transform.position = new Vector3(0, 2, -8);
        camGO.AddComponent<AudioListener>();

        // Light
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Ground
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Ground";
        floor.transform.localScale = new Vector3(8, 1, 8);

        // Managers container
        var managers = new GameObject("Managers");

        // Helper to find type by name across assemblies
        Func<string, Type> findType = (string typeName) =>
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var t = asm.GetTypes().FirstOrDefault(x => x.Name == typeName);
                    if (t != null) return t;
                }
                catch { }
            }
            return null;
        };

        // Add manager components if types exist
        string[] managerTypes = new string[] {
            "GameManager", "QuestManager", "DungeonManager", "SettlementManager",
            "EconomySystem", "SaveSystem", "FactionManager", "DialogueSystem", "UnifiedDialogueSystem",
            "UIController", "WorldMapUI", "SettlementSystem", "DungeonManager"
        };

        foreach (var mt in managerTypes.Distinct())
        {
            var t = findType(mt);
            if (t != null && t.IsSubclassOf(typeof(MonoBehaviour)))
            {
                managers.AddComponent(t);
                Debug.Log($"Added manager component: {mt}");
            }
            else
            {
                // allow non-MonoBehaviour or missing types — just log
                if (t == null) Debug.LogWarning($"Type not found: {mt}");
                else Debug.LogWarning($"Type {mt} found but is not MonoBehaviour");
            }
        }

        // Create UI Canvas
        var canvasGO = new GameObject("MainCanvas");
        var canvas = canvasGO.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Add UIController if available
        var uiCtrlType = findType("UIController");
        if (uiCtrlType != null && uiCtrlType.IsSubclassOf(typeof(MonoBehaviour)))
            canvasGO.AddComponent(uiCtrlType);

        // Player spawn
        var playerGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        playerGO.name = "Player";
        playerGO.transform.position = new Vector3(0, 1f, 0);
        var playerType = findType("Player");
        if (playerType != null && playerType.IsSubclassOf(typeof(MonoBehaviour)))
        {
            playerGO.AddComponent(playerType);
        }

        // Player Abilities
        var pa = findType("PlayerAbilities");
        if (pa != null && pa.IsSubclassOf(typeof(MonoBehaviour))) playerGO.AddComponent(pa);

        // Add Veldora NPC
        var veldora = GameObject.CreatePrimitive(PrimitiveType.Cube);
        veldora.name = "Veldora";
        veldora.transform.position = new Vector3(0, 1f, 6f);
        var veldoraType = findType("VeldoraNPC");
        if (veldoraType != null && veldoraType.IsSubclassOf(typeof(MonoBehaviour))) veldora.AddComponent(veldoraType);

        // Magic stone
        var stone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stone.name = "MagicStone";
        stone.transform.position = new Vector3(3f, 0.5f, 2f);
        var msType = findType("MagicStone");
        if (msType != null && msType.IsSubclassOf(typeof(MonoBehaviour))) stone.AddComponent(msType);

        // Absorbable plant
        var plant = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        plant.name = "AbsorbablePlant";
        plant.transform.position = new Vector3(-2f, 0.5f, -1f);
        var absType = findType("Absorbable");
        if (absType != null && absType.IsSubclassOf(typeof(MonoBehaviour))) plant.AddComponent(absType);

        // Simple enemy
        var enemy = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        enemy.name = "Bat_01";
        enemy.transform.position = new Vector3(2f, 0.5f, -3f);
        var seType = findType("SimpleEnemy");
        if (seType != null && seType.IsSubclassOf(typeof(MonoBehaviour))) enemy.AddComponent(seType);

        // Create a UI panel for testing if TMPro exists
        var tmpType = findType("TextMeshProUGUI");
        if (tmpType != null)
        {
            var testTextObj = new GameObject("TestHUDText");
            testTextObj.transform.SetParent(canvasGO.transform, false);
            var rt = testTextObj.AddComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(200, -50);
            var tcomp = testTextObj.AddComponent(tmpType) as MonoBehaviour;
            // attempt to set text via reflection
            if (tcomp != null)
            {
                var prop = tcomp.GetType().GetProperty("text");
                if (prop != null) prop.SetValue(tcomp, "HUD: Ready");
            }
        }

        // Save scene
        string scenePath = "Assets/Scenes/FullGameScene.unity";
        bool ok = EditorSceneManager.SaveScene(scene, scenePath);
        if (ok) EditorUtility.DisplayDialog("Scene Created", "FullGameScene created at Assets/Scenes/FullGameScene.unity", "OK");
        else Debug.LogError("Failed to save scene");
    }
}
#endif
