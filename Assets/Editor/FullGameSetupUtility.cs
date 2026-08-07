#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class FullGameSetupUtility
{
    [MenuItem("Tools/Full Game Setup/Create MainMenu, WorldMap, and Player Prefab")]
    public static void CreateFullSetup()
    {
        // Ensure folders
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Resources/Prefabs");

        // Create Player prefab
        GameObject playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "PlayerPrefab";
        playerGO.transform.position = Vector3.zero;
        playerGO.AddComponent<Player>();
        // Add Rigidbody and Collider
        if (playerGO.GetComponent<Rigidbody>() == null) playerGO.AddComponent<Rigidbody>();

        string prefabPath = "Assets/Resources/Prefabs/Player.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(playerGO, prefabPath);
        GameObject.DestroyImmediate(playerGO);

        // Create MainMenu scene
        Scene menuScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Canvas
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // EventSystem
        var es = new GameObject("EventSystem");
        es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Main Panel
        GameObject mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(canvasGO.transform, false);
        var mpRect = mainPanel.AddComponent<RectTransform>();
        mpRect.sizeDelta = new Vector2(600, 400);

        // Play Button
        GameObject playBtn = CreateUIButton("PlayButton", "Play", mainPanel.transform, new Vector2(0, 60));
        // Settings Button
        GameObject settingsBtn = CreateUIButton("SettingsButton", "Settings", mainPanel.transform, new Vector2(0, 0));
        // Exit Button
        GameObject exitBtn = CreateUIButton("ExitButton", "Exit", mainPanel.transform, new Vector2(0, -60));

        // Race Panel
        GameObject racePanel = new GameObject("RacePanel");
        racePanel.transform.SetParent(canvasGO.transform, false);
        var rpRect = racePanel.AddComponent<RectTransform>();
        rpRect.sizeDelta = new Vector2(600, 400);

        // Race Dropdown
        GameObject raceDropdownGO = CreateUIDropdown("RaceDropdown", racePanel.transform, new Vector2(0, 60));
        // Next Button
        GameObject raceNext = CreateUIButton("RaceNext", "Next", racePanel.transform, new Vector2(0, -100));

        // Difficulty Panel
        GameObject diffPanel = new GameObject("DifficultyPanel");
        diffPanel.transform.SetParent(canvasGO.transform, false);
        var dpRect = diffPanel.AddComponent<RectTransform>();
        dpRect.sizeDelta = new Vector2(600, 400);

        GameObject diffDropdownGO = CreateUIDropdown("DifficultyDropdown", diffPanel.transform, new Vector2(0, 60));
        GameObject startBtn = CreateUIButton("StartButton", "Start Adventure", diffPanel.transform, new Vector2(0, -100));

        // Attach MainMenuController
        var controllerGO = new GameObject("MainMenuController");
        var controller = controllerGO.AddComponent<MainMenuController>();

        // Assign references
        controller.mainPanel = mainPanel;
        controller.racePanel = racePanel;
        controller.difficultyPanel = diffPanel;
        controller.playButton = playBtn.GetComponent<Button>();
        controller.settingsButton = settingsBtn.GetComponent<Button>();
        controller.exitButton = exitBtn.GetComponent<Button>();
        controller.raceDropdown = raceDropdownGO.GetComponent<Dropdown>();
        controller.raceNextButton = raceNext.GetComponent<Button>();
        controller.difficultyDropdown = diffDropdownGO.GetComponent<Dropdown>();
        controller.startButton = startBtn.GetComponent<Button>();

        // Initially only main panel visible
        mainPanel.SetActive(true);
        racePanel.SetActive(false);
        diffPanel.SetActive(false);

        // Save MainMenu scene
        string menuScenePath = "Assets/Scenes/MainMenu.unity";
        EditorSceneManager.SaveScene(menuScene, menuScenePath);

        // Create WorldMap scene
        Scene worldScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Camera
        GameObject cam = new GameObject("Main Camera");
        cam.AddComponent<Camera>();
        cam.tag = "MainCamera";
        cam.transform.position = new Vector3(0, 30, -30);
        cam.transform.LookAt(Vector3.zero);
        cam.AddComponent<AudioListener>();

        // Light
        GameObject light = new GameObject("Directional Light");
        var l = light.AddComponent<Light>();
        l.type = LightType.Directional;
        l.intensity = 1f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Large plane as world
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "WorldPlane";
        plane.transform.localScale = new Vector3(50, 1, 50);

        // Dungeon area (simple cube as entrance) and spawn point
        GameObject dungeon = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dungeon.name = "DungeonEntrance";
        dungeon.transform.position = new Vector3(0, 0.5f, 0);
        dungeon.transform.localScale = new Vector3(10, 1, 10);

        GameObject spawn = new GameObject("StartDungeonSpawn");
        spawn.transform.position = new Vector3(0, 1.5f, 0);

        // Save WorldMap scene
        string worldScenePath = "Assets/Scenes/WorldMap.unity";
        EditorSceneManager.SaveScene(worldScene, worldScenePath);

        // Add scenes to build settings (MainMenu first)
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[] {
            new EditorBuildSettingsScene(menuScenePath, true),
            new EditorBuildSettingsScene(worldScenePath, true)
        };
        EditorBuildSettings.scenes = scenes;

        EditorUtility.DisplayDialog("Full Setup", "MainMenu, WorldMap, and Player prefab created.\nOpen Scenes folder to view scenes.", "OK");
    }

    private static GameObject CreateUIButton(string name, string text, Transform parent, Vector2 anchoredPosition)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);

        var rect = btnGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 50);
        rect.anchoredPosition = anchoredPosition;

        var img = btnGO.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 1);

        var btn = btnGO.AddComponent<Button>();

        GameObject txt = new GameObject("Text");
        txt.transform.SetParent(btnGO.transform, false);
        var txtRect = txt.AddComponent<RectTransform>();
        txtRect.sizeDelta = rect.sizeDelta;

        var textComp = txt.AddComponent<Text>();
        textComp.text = text;
        textComp.alignment = TextAnchor.MiddleCenter;
        textComp.color = Color.white;

        return btnGO;
    }

    private static GameObject CreateUIDropdown(string name, Transform parent, Vector2 anchoredPosition)
    {
        GameObject ddGO = new GameObject(name);
        ddGO.transform.SetParent(parent, false);

        var rect = ddGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 40);
        rect.anchoredPosition = anchoredPosition;

        var dd = ddGO.AddComponent<Dropdown>();

        // Add template parts minimal (placeholder)
        GameObject labelGO = new GameObject("Label");
        labelGO.transform.SetParent(ddGO.transform, false);
        var lbl = labelGO.AddComponent<Text>();
        lbl.text = "Option";
        lbl.alignment = TextAnchor.MiddleCenter;
        lbl.color = Color.white;

        return ddGO;
    }
}
#endif
