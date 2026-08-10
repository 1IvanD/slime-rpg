using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasSetup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SetupCanvas()
    {
        // Создание главного Canvas
        GameObject canvasObj = new GameObject("MainCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        
        GraphicRaycaster raycaster = canvasObj.AddComponent<GraphicRaycaster>();
        
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;
        
        // Создание HUD панели
        CreateHUDPanel(canvasObj.transform);
        
        // Создание панели умений
        CreateSkillsPanel(canvasObj.transform);
        
        // Создание панели статистики
        CreateStatsPanel(canvasObj.transform);
        
        // Создание игрового меню
        CreateGameMenu(canvasObj.transform);
        
        // Создание уведомлений
        CreateNotificationPanel(canvasObj.transform);

        // Создание панели инвентаря
        CreateInventoryPanel(canvasObj.transform);

        // --- Новые строки: добавляем контроллеры и менеджер игры ---

        // Добавляем UIController, чтобы он искал элементы под transform канваса
        if (canvasObj.GetComponent<UIController>() == null)
            canvasObj.AddComponent<UIController>();

        // Добавляем InventoryUIController on canvas
        if (canvasObj.GetComponent<InventoryUIController>() == null)
            canvasObj.AddComponent<InventoryUIController>();

        // Добавляем SkillsUIController on canvas
        if (canvasObj.GetComponent<SkillsUIController>() == null)
            canvasObj.AddComponent<SkillsUIController>();

        // Добавляем PauseMenu, чтобы он создал панель паузы и реагировал на P
        if (canvasObj.GetComponent<PauseMenu>() == null)
            canvasObj.AddComponent<PauseMenu>();

        // Создаём GameManager singleton, если его нет
        if (UnityEngine.Object.FindObjectOfType<GameManager>() == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
            UnityEngine.Object.DontDestroyOnLoad(gm);
        }
    }
    
    private static void CreateHUDPanel(Transform canvasTransform)
    {
        GameObject hudPanel = new GameObject("HUDPanel");
        hudPanel.transform.SetParent(canvasTransform, false);
        
        RectTransform hudRect = hudPanel.AddComponent<RectTransform>();
        hudRect.anchorMin = Vector2.zero;
        hudRect.anchorMax = new Vector2(1, 0.15f);
        hudRect.offsetMin = Vector2.zero;
        hudRect.offsetMax = Vector2.zero;
        
        Image hudBg = hudPanel.AddComponent<Image>();
        hudBg.color = new Color(0, 0, 0, 0.7f);
        
        // Здоровье
        CreateHealthDisplay(hudPanel.transform);
        
        // Уровень
        CreateLevelDisplay(hudPanel.transform);
        
        // Опыт
        CreateExperienceDisplay(hudPanel.transform);
        
        // Название врага
        CreateEnemyNameDisplay(hudPanel.transform);

        // Buttons: Inventory, Skills, Menu
        CreateHUDButton(hudPanel.transform, "InventoryButton", "Inventory [I]", new Vector2(60, 60), new Vector2(-200, 0), () => { UIController.GetInstance()?.ToggleInventoryUI(); });
        CreateHUDButton(hudPanel.transform, "SkillsButton", "Skills [K]", new Vector2(60, 60), new Vector2(-120, 0), () => { UIController.GetInstance()?.ToggleSkillsUI(); });
        CreateHUDButton(hudPanel.transform, "MenuButton", "Menu [Esc]", new Vector2(60, 60), new Vector2(-40, 0), () => { GameManager.Instance?.PauseGame(); });
    }

    private static void CreateHUDButton(Transform parent, string name, string text, Vector2 size, Vector2 anchoredPosition, UnityEngine.Events.UnityAction onClick)
    {
        var btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(1, 0.5f);
        rt.anchorMax = new Vector2(1, 0.5f);
        rt.anchoredPosition = anchoredPosition;

        var img = btnGO.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        var btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(onClick);

        var txtGO = new GameObject("Text"); txtGO.transform.SetParent(btnGO.transform, false);
        var txt = txtGO.AddComponent<TextMeshProUGUI>(); txt.text = text; txt.alignment = TextAlignmentOptions.Center; txt.color = Color.white; txt.fontSize = 14;
        var txtRT = txtGO.GetComponent<RectTransform>(); txtRT.anchorMin = Vector2.zero; txtRT.anchorMax = Vector2.one; txtRT.offsetMin = Vector2.zero; txtRT.offsetMax = Vector2.zero;
    }

    private static void CreateInventoryPanel(Transform canvasTransform)
    {
        GameObject invPanel = new GameObject("InventoryPanel");
        invPanel.transform.SetParent(canvasTransform, false);
        var rt = invPanel.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.6f, 0.1f); rt.anchorMax = new Vector2(0.98f, 0.6f);
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        var bg = invPanel.AddComponent<Image>(); bg.color = new Color(0.02f, 0.02f, 0.05f, 0.9f);

        // Title
        var title = new GameObject("Title"); title.transform.SetParent(invPanel.transform, false);
        var titleText = title.AddComponent<TextMeshProUGUI>(); titleText.text = "Inventory"; titleText.fontSize = 28; titleText.color = Color.white;
        var titleRT = title.GetComponent<RectTransform>(); titleRT.anchorMin = new Vector2(0.02f, 0.9f); titleRT.anchorMax = new Vector2(0.98f, 0.98f);

        // List container
        var list = new GameObject("List"); list.transform.SetParent(invPanel.transform, false);
        var listRT = list.AddComponent<RectTransform>(); listRT.anchorMin = new Vector2(0.02f, 0.02f); listRT.anchorMax = new Vector2(0.98f, 0.86f);
        var vlg = list.AddComponent<VerticalLayoutGroup>(); vlg.spacing = 6; vlg.childForceExpandHeight = false; vlg.childControlHeight = true;

        // Attach InventoryUIController
        var invCtrl = invPanel.AddComponent<InventoryUIController>();
        invCtrl.rootPanel = invPanel;
        invCtrl.listContainer = list.transform;
        invCtrl.listItemPrefab = Resources.Load<GameObject>("Prefabs/ChoiceButton");

        invPanel.SetActive(false);
    }

    private static void CreateSkillsPanel(Transform canvasTransform)
    {
        GameObject skillsPanel = new GameObject("SkillsPanel");
        skillsPanel.transform.SetParent(canvasTransform, false);

        RectTransform rect = skillsPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = skillsPanel.AddComponent<Image>();
        bg.color = new Color(0.05f, 0.1f, 0.05f, 0.95f);

        VerticalLayoutGroup layout = skillsPanel.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 15;
        layout.padding = new RectOffset(30, 30, 30, 30);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(skillsPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "Skills";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.green;

        // Skills list container
        var skillsList = new GameObject("SkillsList");
        skillsList.transform.SetParent(skillsPanel.transform, false);
        var listRT = skillsList.AddComponent<RectTransform>();
        listRT.sizeDelta = new Vector2(0, 400);

        // Attach SkillsUIController
        var sCtrl = skillsPanel.AddComponent<SkillsUIController>();
        sCtrl.rootPanel = skillsPanel;
        sCtrl.listContainer = skillsList.transform;
        sCtrl.entryPrefab = Resources.Load<GameObject>("Prefabs/ChoiceButton");

        skillsPanel.SetActive(false);
    }

    private static void CreateStatsPanel(Transform canvasTransform)
    {
        GameObject stats = new GameObject("StatsPanel");
        stats.transform.SetParent(canvasTransform, false);
        RectTransform rt = stats.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.02f, 0.15f); rt.anchorMax = new Vector2(0.22f, 0.45f);
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        Image bg = stats.AddComponent<Image>(); bg.color = new Color(0.05f, 0.05f, 0.08f, 0.9f);

        // Simple text placeholders for stats
        var atk = new GameObject("ATKStat"); atk.transform.SetParent(stats.transform, false); var atkT = atk.AddComponent<TextMeshProUGUI>(); atkT.text = "ATK: 0"; atkT.color = Color.white;
        var def = new GameObject("DEFStat"); def.transform.SetParent(stats.transform, false); var defT = def.AddComponent<TextMeshProUGUI>(); defT.text = "DEF: 0"; defT.color = Color.white;
        var abs = new GameObject("AbsorbedStat"); abs.transform.SetParent(stats.transform, false); var absT = abs.AddComponent<TextMeshProUGUI>(); absT.text = "Absorbed: 0"; absT.color = Color.white;
        var skl = new GameObject("SkillsStat"); skl.transform.SetParent(stats.transform, false); var sklT = skl.AddComponent<TextMeshProUGUI>(); sklT.text = "Skills: 0"; sklT.color = Color.white;
    }

    private static void CreateGameMenu(Transform canvasTransform)
    {
        GameObject menu = new GameObject("GameMenu");
        menu.transform.SetParent(canvasTransform, false);
        RectTransform rt = menu.AddComponent<RectTransform>(); rt.anchorMin = new Vector2(0.3f, 0.3f); rt.anchorMax = new Vector2(0.7f, 0.7f); rt.offsetMin = rt.offsetMax = Vector2.zero;
        Image bg = menu.AddComponent<Image>(); bg.color = new Color(0f, 0f, 0f, 0.9f);

        var title = new GameObject("Title"); title.transform.SetParent(menu.transform, false); var tText = title.AddComponent<TextMeshProUGUI>(); tText.text = "Game Menu"; tText.color = Color.white;
        menu.SetActive(false);
    }

    private static void CreateNotificationPanel(Transform canvasTransform)
    {
        GameObject notif = new GameObject("NotificationPanel"); notif.transform.SetParent(canvasTransform, false);
        RectTransform rt = notif.AddComponent<RectTransform>(); rt.anchorMin = new Vector2(0.25f, 0.85f); rt.anchorMax = new Vector2(0.75f, 0.95f); rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
}
