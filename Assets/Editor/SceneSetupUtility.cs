#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSetupUtility
{
    [MenuItem("Tools/Scene Setup/Create TestScene and MainMenu")]
    public static void CreateScenes()
    {
        // Ensure Scenes folder exists
        System.IO.Directory.CreateDirectory("Assets/Scenes");

        // Create TestScene
        Scene testScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject cam = new GameObject("Main Camera");
        cam.AddComponent<Camera>();
        cam.tag = "MainCamera";
        cam.transform.position = new Vector3(0, 10, -10);
        cam.transform.LookAt(Vector3.zero);
        cam.AddComponent<AudioListener>();

        GameObject light = new GameObject("Directional Light");
        Light l = light.AddComponent<Light>();
        l.type = LightType.Directional;
        l.intensity = 1f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // EventSystem
        var es = new GameObject("EventSystem");
        es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Spawn point
        var spawn = new GameObject("SpawnPoint");
        spawn.transform.position = Vector3.zero;

        // Player placeholder
        var playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "PlayerPlaceholder";
        playerGO.transform.position = new Vector3(0, 1, 0);
        playerGO.AddComponent<Player>();

        // GameManager (will also be created at runtime by CanvasSetup if not present)
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
        gm.AddComponent<AudioManager>();
        gm.AddComponent<SaveManager>();

        // Save the scene
        string testScenePath = "Assets/Scenes/TestScene.unity";
        EditorSceneManager.SaveScene(testScene, testScenePath);

        // Create MainMenu scene
        Scene menuScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // EventSystem in menu
        var es2 = new GameObject("EventSystem");
        es2.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es2.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Simple Start button
        var buttonGO = new GameObject("StartButton");
        buttonGO.transform.SetParent(canvasGO.transform, false);
        var rect = buttonGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchoredPosition = Vector2.zero;
        var img = buttonGO.AddComponent<UnityEngine.UI.Image>();
        var btn = buttonGO.AddComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(() => { EditorSceneManager.OpenScene(testScenePath); EditorApplication.isPlaying = true; });

        EditorSceneManager.SaveScene(menuScene, "Assets/Scenes/MainMenu.unity");

        // Add scenes to build settings
        var scenes = new EditorBuildSettingsScene[] {
            new EditorBuildSettingsScene(testScenePath, true),
            new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true)
        };
        EditorBuildSettings.scenes = scenes;

        EditorUtility.DisplayDialog("Scene Setup", "Created TestScene and MainMenu in Assets/Scenes and updated Build Settings.", "OK");
    }
}
#endif
