#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public static class SceneFullGameSetup
{
    [MenuItem("Tools/Tempest/Setup FullGameScreen Scene (placeholder)")]
    public static void SetupFullGameScene()
    {
        // Create new scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Ensure folders
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Resources/Prefabs");

        // ---------- Camera ----------
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camGO.AddComponent<AudioListener>();
        camGO.transform.position = new Vector3(0, 6f, -10f);
        camGO.transform.LookAt(Vector3.zero);

        // ---------- Lighting ----------
        var sun = new GameObject("Directional Light");
        var light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // ---------- Managers (only if types exist) ----------
        var managers = new GameObject("Managers");
        managers.transform.position = Vector3.zero;

        string[] managerTypes = new string[] {
            "GameManager", "QuestManager", "InventorySystem", "EconomySystem", "SaveSystem",
            "DialogueManager", "CombatManager", "WorldTimeManager", "WeatherSystem", "DynamicEventManager",
            "SkillManager", "DetailedHUDManager"
        };

        System.Reflection.Assembly asm = typeof(UnityEngine.Object).Assembly; // just to get assembly list

        System.Type findType(string name)
        {
            // search in all assemblies loaded in editor
            var t = System.AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(name))
                .FirstOrDefault(x => x != null);
            return t;
        }

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
                if (t == null) Debug.LogWarning($"Type not found (manager skipped): {mt}");
                else Debug.LogWarning($"Type {mt} found but is not MonoBehaviour");
            }
        }

        // ---------- Canvas & EventSystem ----------
        var canvasGO = new GameObject("MainCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var es = new GameObject("EventSystem");
        es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // ---------- HUD Panel (placeholder layout) ----------
        var hudPanel = new GameObject("HUDPanel");
        hudPanel.transform.SetParent(canvasGO.transform, false);
        var hudRect = hudPanel.AddComponent<RectTransform>();
        hudRect.anchorMin = new Vector2(0.02f, 0.7f);
        hudRect.anchorMax = new Vector2(0.98f, 0.98f);
        hudRect.offsetMin = Vector2.zero; hudRect.offsetMax = Vector2.zero;
        var hudImg = hudPanel.AddComponent<Image>();
        hudImg.color = new Color(0,0,0,0.25f);

        // HealthDisplay
        var healthDisplay = CreateUIItem(hudPanel.transform, "HealthDisplay", new Vector2(160,30));
        var hpText = healthDisplay.AddComponent<Text>();
        hpText.text = "HP: 100/100";
        hpText.color = Color.white;

        // Level/XP
        var lvlDisplay = CreateUIItem(hudPanel.transform, "LevelDisplay", new Vector2(160,30));
        var lvlText = lvlDisplay.AddComponent<Text>(); lvlText.text = "Level: 1"; lvlText.color = Color.white;

        // Create a simple choice button prefab for dialogue choices
        var choiceBtn = new GameObject("ChoiceButtonPrefab");
        var btn = choiceBtn.AddComponent<Button>();
        var img = choiceBtn.AddComponent<Image>(); img.color = Color.white * 0.9f;
        var t = new GameObject("Text"); t.transform.SetParent(choiceBtn.transform, false);
        var txt = t.AddComponent<Text>(); txt.text = "Choice"; txt.color = Color.black; txt.alignment = TextAnchor.MiddleCenter;
        var rect = choiceBtn.AddComponent<RectTransform>(); rect.sizeDelta = new Vector2(300,40);
        // Save prefab
        string prefabPath = "Assets/Resources/Prefabs/ChoiceButton.prefab";
        PrefabUtility.SaveAsPrefabAsset(choiceBtn, prefabPath);
        GameObject.DestroyImmediate(choiceBtn);

        // ---------- Dialogue UI (panel hidden by default) ----------
        var dialoguePanel = new GameObject("DialogueUI");
        dialoguePanel.transform.SetParent(canvasGO.transform, false);
        var dpRect = dialoguePanel.AddComponent<RectTransform>();
        dpRect.anchorMin = new Vector2(0.1f, 0.05f); dpRect.anchorMax = new Vector2(0.9f, 0.35f);
        var dpImg = dialoguePanel.AddComponent<Image>(); dpImg.color = new Color(0,0,0,0.6f);
        dialoguePanel.SetActive(false);

        // Speaker name
        var speaker = CreateUIItem(dialoguePanel.transform, "SpeakerName", new Vector2(400,30));
        var spText = speaker.AddComponent<Text>(); spText.text = "Veldora"; spText.color = Color.cyan;

        // Dialogue text
        var dlgTextGO = CreateUIItem(dialoguePanel.transform, "DialogueText", new Vector2(800,120));
        var dlgText = dlgTextGO.AddComponent<Text>(); dlgText.text = "..."; dlgText.color = Color.white; dlgText.alignment = TextAnchor.UpperLeft;

        // Choices container
        var choicesContainer = new GameObject("ChoicesContainer");
        choicesContainer.transform.SetParent(dialoguePanel.transform, false);
        var ccRect = choicesContainer.AddComponent<RectTransform>(); ccRect.anchoredPosition = new Vector2(0, -80);

        // Create DialogueUIController and wire refs if class exists
        var dmType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "DialogueUIController");
        if (dmType != null)
        {
            var uiCtrl = dialoguePanel.AddComponent(dmType);
            // try set fields via reflection
            var rootField = dmType.GetField("rootPanel");
            if (rootField != null) rootField.SetValue(dialoguePanel.GetComponent(dmType), dialoguePanel);
            var speakerNameField = dmType.GetField("speakerNameText");
            var speakerIconField = dmType.GetField("speakerIconImage");
            var dialogueTextField = dmType.GetField("dialogueText");
            var choicesField = dmType.GetField("choicesContainer");
            var prefabField = dmType.GetField("choiceButtonPrefab");
            if (speakerNameField != null) speakerNameField.SetValue(dialoguePanel.GetComponent(dmType), spText);
            if (dialogueTextField != null) dialogueTextField.SetValue(dialoguePanel.GetComponent(dmType), dlgText);
            if (choicesField != null) choicesField.SetValue(dialoguePanel.GetComponent(dmType), choicesContainer.transform);
            var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefabField != null && prefabAsset != null) prefabField.SetValue(dialoguePanel.GetComponent(dmType), prefabAsset);
        }

        // ---------- Ground / World objects (placeholders in anime style) ----------
        var worldRoot = new GameObject("World");

        // Player (sphere)
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Player.prefab");
        GameObject playerGO;
        if (playerPrefab != null) playerGO = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
        else
        {
            playerGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            playerGO.name = "Player";
            playerGO.AddComponent<Player>();
        }
        playerGO.transform.position = new Vector3(0, 1f, 0);
        playerGO.transform.SetParent(worldRoot.transform);
        // Add PlayerAbilities if exists
        var paType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "PlayerAbilities");
        if (paType != null && playerGO.GetComponent(paType) == null) playerGO.AddComponent(paType);

        // Veldora (large cube)
        var veldora = GameObject.CreatePrimitive(PrimitiveType.Cube);
        veldora.name = "Veldora";
        veldora.transform.position = new Vector3(0, 1.5f, 6f);
        veldora.transform.localScale = new Vector3(4f,4f,4f);
        veldora.transform.SetParent(worldRoot.transform);
        // add NPCDialogueController if available
        var npcType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "NPCDialogueController");
        if (npcType != null) {
            var comp = veldora.AddComponent(npcType);
            // try to assign the example dialogue tree if present
            var tree = AssetDatabase.FindAssets("Veldora_Tree t:DialogueTreeSO");
            if (tree != null && tree.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(tree[0]);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                var fld = npcType.GetField("dialogue");
                if (fld != null) fld.SetValue(comp, asset);
            }
        }

        // Settlement area: rows of cubes (houses)
        var settlementRoot = new GameObject("Settlement"); settlementRoot.transform.SetParent(worldRoot.transform);
        for (int i=0;i<6;i++)
        {
            var b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            b.name = "House_"+i;
            b.transform.position = new Vector3(-8f + i*3f, 0.5f, 8f);
            b.transform.localScale = new Vector3(2f,1f,2f);
            b.transform.SetParent(settlementRoot.transform);
        }

        // Blacksmith (anvil cube + cylinder furnace)
        var blacksmith = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blacksmith.name = "Blacksmith"; blacksmith.transform.position = new Vector3(6f, 0.5f, 4f); blacksmith.transform.localScale = new Vector3(3f,1.5f,3f);
        blacksmith.transform.SetParent(worldRoot.transform);
        var furnace = GameObject.CreatePrimitive(PrimitiveType.Cylinder); furnace.name = "Furnace"; furnace.transform.position = blacksmith.transform.position + new Vector3(1.5f,0f,0f); furnace.transform.SetParent(blacksmith.transform);
        var bsType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "BlacksmithStation");
        if (bsType != null) blacksmith.AddComponent(bsType);

        // Alchemy (tower)
        var alch = GameObject.CreatePrimitive(PrimitiveType.Cylinder); alch.name = "Alchemy"; alch.transform.position = new Vector3(-6f,1f,4f); alch.transform.localScale = new Vector3(2f,2f,2f); alch.transform.SetParent(worldRoot.transform);
        var alType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "AlchemyStation");
        if (alType != null) alch.AddComponent(alType);

        // Market (platform)
        var market = GameObject.CreatePrimitive(PrimitiveType.Cube); market.name = "Market"; market.transform.position = new Vector3(0f,0.2f,10f); market.transform.localScale = new Vector3(6f,0.4f,6f); market.transform.SetParent(worldRoot.transform);

        // Some trees (cylinders + spheres as foliage)
        for (int i=0;i<8;i++)
        {
            var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.transform.position = new Vector3(-10f + i*3f, 0.5f, -6f);
            trunk.transform.localScale = new Vector3(0.6f,1.5f,0.6f);
            trunk.name = "TreeTrunk_"+i; trunk.transform.SetParent(worldRoot.transform);
            var leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leaves.transform.position = trunk.transform.position + new Vector3(0,1.8f,0);
            leaves.transform.localScale = new Vector3(2f,2f,2f);
            leaves.name = "TreeLeaves_"+i; leaves.transform.SetParent(trunk.transform);
        }

        // Enemy spawn points (small cubes)
        for (int i=0;i<3;i++)
        {
            var sp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sp.name = "EnemySpawnPoint_"+i; sp.transform.position = new Vector3(4f + i*2f, 0.5f, -4f); sp.transform.localScale = new Vector3(0.3f,0.3f,0.3f); sp.transform.SetParent(worldRoot.transform);
            var spawnerType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == "EnemySpawner");
            if (spawnerType != null) sp.AddComponent(spawnerType);
        }

        // ---------- Assign assets to systems if present ----------
        // WorldTimeManager config
        var wtmGO = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(g => g.GetComponent(System.Type.GetType("WorldTimeManager")) != null);
        if (wtmGO != null)
        {
            var cfg = AssetDatabase.LoadAssetAtPath<Object>("Assets/Data/World/TimeOfDayConfigSO.asset");
            var wtm = wtmGO.GetComponent(System.Type.GetType("WorldTimeManager"));
            if (wtm != null && cfg != null)
            {
                var f = wtm.GetType().GetField("config");
                if (f != null) f.SetValue(wtm, cfg);
                Debug.Log("Assigned TimeOfDayConfigSO to WorldTimeManager (if present)");
            }
        }

        // WeatherSystem available weathers
        var weatherGO = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(g => g.GetComponent(System.Type.GetType("WeatherSystem")) != null);
        if (weatherGO != null)
        {
            var ws = weatherGO.GetComponent(System.Type.GetType("WeatherSystem"));
            if (ws != null)
            {
                var listField = ws.GetType().GetField("availableWeathers");
                if (listField != null)
                {
                    var weathers = AssetDatabase.FindAssets("t:WeatherSO", new[] { "Assets/Data/World" }).Select(g => AssetDatabase.GUIDToAssetPath(g)).Select(p => AssetDatabase.LoadAssetAtPath<Object>(p)).ToArray();
                    if (weathers.Length > 0) listField.SetValue(ws, weathers.ToList());
                }
            }
        }

        // DialogueManager.uiController assignment
        var dmGO = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(g => g.GetComponent(System.Type.GetType("DialogueManager")) != null);
        if (dmGO != null)
        {
            var dm = dmGO.GetComponent(System.Type.GetType("DialogueManager"));
            var uiCtrl = dialoguePanel.GetComponent(System.Type.GetType("DialogueUIController"));
            var fi = dm.GetType().GetField("uiController");
            if (fi != null && uiCtrl != null) fi.SetValue(dm, uiCtrl);
            Debug.Log("Assigned DialogueUIController to DialogueManager (if present)");
        }

        // Save scene
        string scenePath = "Assets/Scenes/FullGameScreen.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("FullGameScreen scene created and saved at " + scenePath + ".\nPlaceholders (cubes/spheres) created — replace models/textures in Editor as needed and set Inspector references.");
    }

    private static GameObject CreateUIItem(Transform parent, string name, Vector2 size)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        return go;
    }
}
#endif
