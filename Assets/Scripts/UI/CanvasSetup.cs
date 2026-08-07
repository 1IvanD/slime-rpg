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

        // --- Новые строки: добавляем контроллеры и менеджер игры ---

        // Добавляем UIController, чтобы он искал элементы под transform канваса
        if (canvasObj.GetComponent<UIController>() == null)
            canvasObj.AddComponent<UIController>();

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
    }
    
    private static void CreateHealthDisplay(Transform parent)
    {
        GameObject healthObj = new GameObject("HealthDisplay");
        healthObj.transform.SetParent(parent, false);
        
        RectTransform healthRect = healthObj.AddComponent<RectTransform>();
        healthRect.anchorMin = Vector2.zero;
        healthRect.anchorMax = new Vector2(0.3f, 1);
        healthRect.offsetMin = new Vector2(20, 10);
        healthRect.offsetMax = new Vector2(-10, -10);
        
        // Фон
        Image bgImage = healthObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        // Текст здоровья
        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(healthObj.transform, false);
        
        RectTransform healthTextRect = healthTextObj.AddComponent<RectTransform>();
        healthTextRect.anchorMin = Vector2.zero;
        healthTextRect.anchorMax = Vector2.one;
        healthTextRect.offsetMin = Vector2.zero;
        healthTextRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
        healthText.text = "HP: 100/100";
        healthText.fontSize = 36;
        healthText.alignment = TextAlignmentOptions.Center;
        healthText.color = Color.white;
        
        // Полоса здоровья
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(healthObj.transform, false);
        
        RectTransform healthBarRect = healthBarObj.AddComponent<RectTransform>();
        healthBarRect.anchorMin = Vector2.zero;
        healthBarRect.anchorMax = new Vector2(1, 0.3f);
        healthBarRect.offsetMin = new Vector2(5, 5);
        healthBarRect.offsetMax = new Vector2(-5, -5);
        
        Image healthBarBg = healthBarObj.AddComponent<Image>();
        healthBarBg.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        // Заполнение полосы
        GameObject healthBarFillObj = new GameObject("Fill");
        healthBarFillObj.transform.SetParent(healthBarObj.transform, false);
        
        RectTransform healthBarFillRect = healthBarFillObj.AddComponent<RectTransform>();
        healthBarFillRect.anchorMin = Vector2.zero;
        healthBarFillRect.anchorMax = new Vector2(1, 1);
        healthBarFillRect.offsetMin = Vector2.zero;
        healthBarFillRect.offsetMax = Vector2.zero;
        
        Image healthBarFill = healthBarFillObj.AddComponent<Image>();
        healthBarFill.color = Color.red;
    }
    
    private static void CreateLevelDisplay(Transform parent)
    {
        GameObject levelObj = new GameObject("LevelDisplay");
        levelObj.transform.SetParent(parent, false);
        
        RectTransform levelRect = levelObj.AddComponent<RectTransform>();
        levelRect.anchorMin = new Vector2(0.3f, 0);
        levelRect.anchorMax = new Vector2(0.5f, 1);
        levelRect.offsetMin = new Vector2(10, 10);
        levelRect.offsetMax = new Vector2(-10, -10);
        
        Image bgImage = levelObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        GameObject levelTextObj = new GameObject("LevelText");
        levelTextObj.transform.SetParent(levelObj.transform, false);
        
        RectTransform levelTextRect = levelTextObj.AddComponent<RectTransform>();
        levelTextRect.anchorMin = Vector2.zero;
        levelTextRect.anchorMax = Vector2.one;
        levelTextRect.offsetMin = Vector2.zero;
        levelTextRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI levelText = levelTextObj.AddComponent<TextMeshProUGUI>();
        levelText.text = "Level: 1";
        levelText.fontSize = 36;
        levelText.alignment = TextAlignmentOptions.Center;
        levelText.color = Color.yellow;
    }
    
    private static void CreateExperienceDisplay(Transform parent)
    {
        GameObject expObj = new GameObject("ExperienceDisplay");
        expObj.transform.SetParent(parent, false);
        
        RectTransform expRect = expObj.AddComponent<RectTransform>();
        expRect.anchorMin = new Vector2(0.5f, 0);
        expRect.anchorMax = new Vector2(0.75f, 1);
        expRect.offsetMin = new Vector2(10, 10);
        expRect.offsetMax = new Vector2(-10, -10);
        
        Image bgImage = expObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        GameObject expTextObj = new GameObject("ExperienceText");
        expTextObj.transform.SetParent(expObj.transform, false);
        
        RectTransform expTextRect = expTextObj.AddComponent<RectTransform>();
        expTextRect.anchorMin = Vector2.zero;
        expTextRect.anchorMax = Vector2.one;
        expTextRect.offsetMin = Vector2.zero;
        expTextRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI expText = expTextObj.AddComponent<TextMeshProUGUI>();
        expText.text = "Exp: 0/100";
        expText.fontSize = 32;
        expText.alignment = TextAlignmentOptions.Center;
        expText.color = new Color(0.5f, 1, 0.5f);
    }
    
    private static void CreateEnemyNameDisplay(Transform parent)
    {
        GameObject enemyObj = new GameObject("EnemyNameDisplay");
        enemyObj.transform.SetParent(parent, false);
        
        RectTransform enemyRect = enemyObj.AddComponent<RectTransform>();
        enemyRect.anchorMin = new Vector2(0.75f, 0);
        enemyRect.anchorMax = Vector2.one;
        enemyRect.offsetMin = new Vector2(10, 10);
        enemyRect.offsetMax = new Vector2(-20, -10);
        
        Image bgImage = enemyObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        GameObject enemyTextObj = new GameObject("EnemyText");
        enemyTextObj.transform.SetParent(enemyObj.transform, false);
        
        RectTransform enemyTextRect = enemyTextObj.AddComponent<RectTransform>();
        enemyTextRect.anchorMin = Vector2.zero;
        enemyTextRect.anchorMax = Vector2.one;
        enemyTextRect.offsetMin = Vector2.zero;
        enemyTextRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI enemyText = enemyTextObj.AddComponent<TextMeshProUGUI>();
        enemyText.text = "No enemy targeted";
        enemyText.fontSize = 28;
        enemyText.alignment = TextAlignmentOptions.Center;
        enemyText.color = Color.white;
    }
    
    private static void CreateSkillsPanel(Transform canvasTransform)
    {
        GameObject skillsPanel = new GameObject("SkillsPanel");
        skillsPanel.transform.SetParent(canvasTransform, false);
        
        RectTransform skillsRect = skillsPanel.AddComponent<RectTransform>();
        skillsRect.anchorMin = new Vector2(0, 0.85f);
        skillsRect.anchorMax = Vector2.one;
        skillsRect.offsetMin = Vector2.zero;
        skillsRect.offsetMax = Vector2.zero;
        
        Image skillsBg = skillsPanel.AddComponent<Image>();
        skillsBg.color = new Color(0, 0, 0, 0.7f);
        
        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(skillsPanel.transform, false);
        
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = Vector2.zero;
        titleRect.anchorMax = new Vector2(0.3f, 1);
        titleRect.offsetMin = new Vector2(10, 5);
        titleRect.offsetMax = new Vector2(-10, -5);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "Skills";
        titleText.fontSize = 28;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.cyan;
        
        // Список умений
        GameObject skillListObj = new GameObject("SkillsList");
        skillListObj.transform.SetParent(skillsPanel.transform, false);
        
        RectTransform skillListRect = skillListObj.AddComponent<RectTransform>();
        skillListRect.anchorMin = new Vector2(0.3f, 0);
        skillListRect.anchorMax = Vector2.one;
        skillListRect.offsetMin = new Vector2(5, 5);
        skillListRect.offsetMax = new Vector2(-5, -5);
        
        HorizontalLayoutGroup layoutGroup = skillListObj.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 5;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = true;
    }
    
    private static void CreateStatsPanel(Transform canvasTransform)
    {
        GameObject statsPanel = new GameObject("StatsPanel");
        statsPanel.transform.SetParent(canvasTransform, false);
        
        RectTransform statsRect = statsPanel.AddComponent<RectTransform>();
        statsRect.anchorMin = new Vector2(0.85f, 0.15f);
        statsRect.anchorMax = Vector2.one;
        statsRect.offsetMin = new Vector2(-300, 10);
        statsRect.offsetMax = new Vector2(-10, -10);
        
        Image statsBg = statsPanel.AddComponent<Image>();
        statsBg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        
        VerticalLayoutGroup layoutGroup = statsPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 5;
        layoutGroup.padding = new RectOffset(10, 10, 10, 10);
        layoutGroup.childForceExpandWidth = true;
        
        // Атака
        CreateStatLine(statsPanel.transform, "ATK", "0");
        
        // Защита
        CreateStatLine(statsPanel.transform, "DEF", "0");
        
        // Поглощено врагов
        CreateStatLine(statsPanel.transform, "Absorbed", "0");
        
        // Выученные умения
        CreateStatLine(statsPanel.transform, "Skills", "0");
    }
    
    private static void CreateStatLine(Transform parent, string label, string value)
    {
        GameObject lineObj = new GameObject($"{label}Stat");
        lineObj.transform.SetParent(parent, false);
        
        RectTransform lineRect = lineObj.AddComponent<RectTransform>();
        lineRect.anchorMin = Vector2.zero;
        lineRect.anchorMax = Vector2.one;
        lineRect.offsetMin = Vector2.zero;
        lineRect.offsetMax = Vector2.zero;
        lineRect.sizeDelta = new Vector2(0, 30);
        
        HorizontalLayoutGroup hLayout = lineObj.AddComponent<HorizontalLayoutGroup>();
        hLayout.spacing = 5;
        hLayout.childForceExpandWidth = true;
        
        // Ярлык
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(lineObj.transform, false);
        
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;
        labelRect.sizeDelta = new Vector2(80, 0);
        
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = label + ":";
        labelText.fontSize = 20;
        labelText.alignment = TextAlignmentOptions.MidlineLeft;
        labelText.color = new Color(0.8f, 0.8f, 0.8f);
        
        // Значение
        GameObject valueObj = new GameObject("Value");
        valueObj.transform.SetParent(lineObj.transform, false);
        
        RectTransform valueRect = valueObj.AddComponent<RectTransform>();
        valueRect.anchorMin = Vector2.zero;
        valueRect.anchorMax = Vector2.one;
        valueRect.offsetMin = Vector2.zero;
        valueRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI valueText = valueObj.AddComponent<TextMeshProUGUI>();
        valueText.text = value;
        valueText.fontSize = 20;
        valueText.alignment = TextAlignmentOptions.MidlineRight;
        valueText.color = Color.yellow;
    }
    
    private static void CreateGameMenu(Transform canvasTransform)
    {
        GameObject menuPanel = new GameObject("GameMenu");
        menuPanel.transform.SetParent(canvasTransform, false);
        
        RectTransform menuRect = menuPanel.AddComponent<RectTransform>();
        menuRect.anchorMin = new Vector2(0, 0.85f);
        menuRect.anchorMax = new Vector2(0.15f, 1);
        menuRect.offsetMin = new Vector2(5, 5);
        menuRect.offsetMax = new Vector2(-5, -5);
        
        Image menuBg = menuPanel.AddComponent<Image>();
        menuBg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        VerticalLayoutGroup layoutGroup = menuPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 3;
        layoutGroup.padding = new RectOffset(5, 5, 5, 5);
        layoutGroup.childForceExpandWidth = true;
        
        // Кнопка паузы
        CreateMenuButton(menuPanel.transform, "Pause [P]");
        
        // Кнопка статистики
        CreateMenuButton(menuPanel.transform, "Stats [C]");
        
        // Кнопка выхода
        CreateMenuButton(menuPanel.transform, "Quit [Esc]");
    }
    
    private static void CreateMenuButton(Transform parent, string label)
    {
        GameObject buttonObj = new GameObject(label);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.anchorMin = Vector2.zero;
        buttonRect.anchorMax = Vector2.one;
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;
        buttonRect.sizeDelta = new Vector2(0, 30);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.colors = new ColorBlock()
        {
            normalColor = new Color(0.3f, 0.3f, 0.3f, 0.8f),
            highlightedColor = new Color(0.5f, 0.5f, 0.5f, 1),
            pressedColor = new Color(0.2f, 0.2f, 0.2f, 1),
            disabledColor = new Color(0.3f, 0.3f, 0.3f, 0.5f),
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = label;
        buttonText.fontSize = 18;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }
    
    private static void CreateNotificationPanel(Transform canvasTransform)
    {
        GameObject notifPanel = new GameObject("NotificationPanel");
        notifPanel.transform.SetParent(canvasTransform, false);
        
        RectTransform notifRect = notifPanel.AddComponent<RectTransform>();
        notifRect.anchorMin = new Vector2(0.5f, 0.85f);
        notifRect.anchorMax = new Vector2(1, 1);
        notifRect.offsetMin = new Vector2(10, 5);
        notifRect.offsetMax = new Vector2(-10, -5);
        
        VerticalLayoutGroup layoutGroup = notifPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 5;
        layoutGroup.childForceExpandWidth = true;
    }
}
