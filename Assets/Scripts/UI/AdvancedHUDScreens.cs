using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AdvancedHUDScreens : MonoBehaviour
{
    private Canvas mainCanvas;
    private GameObject equipmentPanel;
    private GameObject skillsPanel;
    private GameObject magicPanel;
    private GameObject achievementPanel;
    private GameObject settlementPanel;

    private bool equipmentOpen = false;
    private bool skillsOpen = false;
    private bool magicOpen = false;
    private bool achievementOpen = false;
    private bool settlementOpen = false;

    private void Start()
    {
        mainCanvas = FindObjectOfType<Canvas>();
        CreateAllPanels();
    }

    private void CreateAllPanels()
    {
        CreateEquipmentPanel();
        CreateSkillsPanel();
        CreateMagicPanel();
        CreateAchievementPanel();
        CreateSettlementPanel();
    }

    private void CreateEquipmentPanel()
    {
        equipmentPanel = new GameObject("EquipmentPanel");
        equipmentPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform rect = equipmentPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = equipmentPanel.AddComponent<Image>();
        bg.color = new Color(0.05f, 0.05f, 0.1f, 0.95f);

        VerticalLayoutGroup layout = equipmentPanel.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.padding = new RectOffset(30, 30, 30, 30);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(equipmentPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "⚔️ УЛУЧШЕНИЕ ЭКИПИРОВКИ ⚔️";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.yellow;

        // Информация об улучшениях
        GameObject infoObj = new GameObject("UpgradeInfo");
        infoObj.transform.SetParent(equipmentPanel.transform, false);
        RectTransform infoRect = infoObj.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(0, 100);
        TextMeshProUGUI infoText = infoObj.AddComponent<TextMeshProUGUI>();
        infoText.text = "Текущие улучшения:\n" +
                       "Меч железный: +0 уровень\n" +
                       "Стоимость следующего: 100 золота";
        infoText.fontSize = 20;
        infoText.color = new Color(0.8f, 0.8f, 0.8f);

        // Кнопки
        GameObject upgradeButtonObj = new GameObject("UpgradeButton");
        upgradeButtonObj.transform.SetParent(equipmentPanel.transform, false);
        RectTransform btnRect = upgradeButtonObj.AddComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(0, 50);
        Image btnImage = upgradeButtonObj.AddComponent<Image>();
        btnImage.color = new Color(0.3f, 0.6f, 0.3f, 0.8f);
        Button btn = upgradeButtonObj.AddComponent<Button>();
        btn.targetGraphic = btnImage;
        btn.onClick.AddListener(() => Debug.Log("Улучшить экипировку"));
        TextMeshProUGUI btnText = upgradeButtonObj.AddComponent<TextMeshProUGUI>();
        btnText.text = "Улучшить [E]"; 
        btnText.fontSize = 24;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;

        // Кнопка очарования
        GameObject enchantButtonObj = new GameObject("EnchantButton");
        enchantButtonObj.transform.SetParent(equipmentPanel.transform, false);
        RectTransform enchantRect = enchantButtonObj.AddComponent<RectTransform>();
        enchantRect.sizeDelta = new Vector2(0, 50);
        Image enchantImage = enchantButtonObj.AddComponent<Image>();
        enchantImage.color = new Color(0.6f, 0.3f, 0.6f, 0.8f);
        Button enchantBtn = enchantButtonObj.AddComponent<Button>();
        enchantBtn.targetGraphic = enchantImage;
        enchantBtn.onClick.AddListener(() => Debug.Log("Добавить очарование"));
        TextMeshProUGUI enchantText = enchantButtonObj.AddComponent<TextMeshProUGUI>();
        enchantText.text = "Очаровать [R]"; 
        enchantText.fontSize = 24;
        enchantText.alignment = TextAlignmentOptions.Center;
        enchantText.color = Color.white;

        // Кнопка синтеза
        GameObject synthesizeButtonObj = new GameObject("SynthesizeButton");
        synthesizeButtonObj.transform.SetParent(equipmentPanel.transform, false);
        RectTransform synRect = synthesizeButtonObj.AddComponent<RectTransform>();
        synRect.sizeDelta = new Vector2(0, 50);
        Image synImage = synthesizeButtonObj.AddComponent<Image>();
        synImage.color = new Color(0.3f, 0.6f, 0.6f, 0.8f);
        Button synBtn = synthesizeButtonObj.AddComponent<Button>();
        synBtn.targetGraphic = synImage;
        synBtn.onClick.AddListener(() => Debug.Log("Синтезировать предмет"));
        TextMeshProUGUI synText = synthesizeButtonObj.AddComponent<TextMeshProUGUI>();
        synText.text = "Синтезировать [T]"; 
        synText.fontSize = 24;
        synText.alignment = TextAlignmentOptions.Center;
        synText.color = Color.white;

        equipmentPanel.SetActive(false);
    }

    private void CreateSkillsPanel()
    {
        skillsPanel = new GameObject("SkillsPanel");
        skillsPanel.transform.SetParent(mainCanvas.transform, false);

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
        titleText.text = "🎣 ТРУДОВЫЕ НАВЫКИ 🎣";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.green;

        // Навыки
        CreateSkillEntry(skillsPanel.transform, "Рыбалка", 1, 50);
        CreateSkillEntry(skillsPanel.transform, "Охота", 1, 50);
        CreateSkillEntry(skillsPanel.transform, "Добыча", 1, 50);
        CreateSkillEntry(skillsPanel.transform, "Крафт", 1, 50);
        CreateSkillEntry(skillsPanel.transform, "Готовка", 1, 50);

        skillsPanel.SetActive(false);
    }

    private void CreateSkillEntry(Transform parent, string skillName, int level, float exp)
    {
        GameObject skillObj = new GameObject(skillName);
        skillObj.transform.SetParent(parent, false);
        RectTransform skillRect = skillObj.AddComponent<RectTransform>();
        skillRect.sizeDelta = new Vector2(0, 50);

        Image skillBg = skillObj.AddComponent<Image>();
        skillBg.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

        VerticalLayoutGroup skillLayout = skillObj.AddComponent<VerticalLayoutGroup>();
        skillLayout.spacing = 2;

        TextMeshProUGUI skillText = skillObj.AddComponent<TextMeshProUGUI>();
        skillText.text = $"{skillName} - Уровень {level} ({exp}%)";
        skillText.fontSize = 18;
        skillText.color = Color.cyan;
    }

    private void CreateMagicPanel()
    {
        magicPanel = new GameObject("MagicPanel");
        magicPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform rect = magicPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = magicPanel.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.05f, 0.1f, 0.95f);

        VerticalLayoutGroup layout = magicPanel.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 15;
        layout.padding = new RectOffset(30, 30, 30, 30);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(magicPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "🔮 МАГИЧЕСКИЕ ШКОЛЫ 🔮";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(1, 0.5f, 0);

        // Школы магии
        CreateMagicSchool(magicPanel.transform, "Огненная магия", "Огонь", 1);
        CreateMagicSchool(magicPanel.transform, "Ледяная магия", "Лед", 1);
        CreateMagicSchool(magicPanel.transform, "Электрическая магия", "Молния", 1);
        CreateMagicSchool(magicPanel.transform, "Темная магия", "Тьма", 1);
        CreateMagicSchool(magicPanel.transform, "Светлая магия", "Свет", 1);

        magicPanel.SetActive(false);
    }

    private void CreateMagicSchool(Transform parent, string schoolName, string element, int level)
    {
        GameObject schoolObj = new GameObject(schoolName);
        schoolObj.transform.SetParent(parent, false);
        RectTransform schoolRect = schoolObj.AddComponent<RectTransform>();
        schoolRect.sizeDelta = new Vector2(0, 50);

        Image schoolBg = schoolObj.AddComponent<Image>();
        schoolBg.color = new Color(0.3f, 0.2f, 0.3f, 0.7f);

        TextMeshProUGUI schoolText = schoolObj.AddComponent<TextMeshProUGUI>();
        schoolText.text = $"{element}: Уровень {level}";
        schoolText.fontSize = 18;
        schoolText.color = Color.magenta;
    }

    private void CreateAchievementPanel()
    {
        achievementPanel = new GameObject("AchievementPanel");
        achievementPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform rect = achievementPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = achievementPanel.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.05f, 0.95f);

        VerticalLayoutGroup layout = achievementPanel.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 15;
        layout.padding = new RectOffset(30, 30, 30, 30);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(achievementPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "🏆 ДОСТИЖЕНИЯ 🏆";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.yellow;

        // Информация о прогрессе
        GameObject progressObj = new GameObject("ProgressInfo");
        progressObj.transform.SetParent(achievementPanel.transform, false);
        RectTransform progRect = progressObj.AddComponent<RectTransform>();
        progRect.sizeDelta = new Vector2(0, 50);
        TextMeshProUGUI progText = progressObj.AddComponent<TextMeshProUGUI>();
        progText.text = "Разблокировано: 0/12 (0%)";
        progText.fontSize = 22;
        progText.color = Color.cyan;

        // Примеры достижений
        CreateAchievementEntry(achievementPanel.transform, "Первая кровь", "Победи первого врага", false);
        CreateAchievementEntry(achievementPanel.transform, "Комбо х10", "Достигни комбо х10", false);
        CreateAchievementEntry(achievementPanel.transform, "Богач", "Накопи 5000 золота", false);

        achievementPanel.SetActive(false);
    }

    private void CreateAchievementEntry(Transform parent, string title, string desc, bool unlocked)
    {
        GameObject achObj = new GameObject(title);
        achObj.transform.SetParent(parent, false);
        RectTransform achRect = achObj.AddComponent<RectTransform>();
        achRect.sizeDelta = new Vector2(0, 60);

        Image achBg = achObj.AddComponent<Image>();
        achBg.color = unlocked ? new Color(0.3f, 0.5f, 0.3f, 0.7f) : new Color(0.3f, 0.3f, 0.3f, 0.7f);

        VerticalLayoutGroup achLayout = achObj.AddComponent<VerticalLayoutGroup>();
        achLayout.spacing = 2;

        TextMeshProUGUI achTitle = achObj.AddComponent<TextMeshProUGUI>();
        achTitle.text = (unlocked ? "✓ " : "✗ ") + title;
        achTitle.fontSize = 18;
        achTitle.color = unlocked ? Color.green : Color.gray;

        TextMeshProUGUI achDesc = achObj.AddComponent<TextMeshProUGUI>();
        achDesc.text = desc;
        achDesc.fontSize = 14;
        achDesc.color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void CreateSettlementPanel()
    {
        settlementPanel = new GameObject("SettlementPanel");
        settlementPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform rect = settlementPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = settlementPanel.AddComponent<Image>();
        bg.color = new Color(0.05f, 0.05f, 0.1f, 0.95f);

        VerticalLayoutGroup layout = settlementPanel.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.padding = new RectOffset(30, 30, 30, 30);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(settlementPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "🏛️ УПРАВЛЕНИЕ ПОСЕЛЕНИЯМИ 🏛️";
        titleText.fontSize = 44;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(0.8f, 0.7f, 0.4f);

        // Список поселений
        CreateSettlementEntry(settlementPanel.transform, "Деревня Риммуру", "50 жителей", "Дружелюбие: 70");
        CreateSettlementEntry(settlementPanel.transform, "Город Ингрессия", "500 жителей", "Дружелюбие: 50");
        CreateSettlementEntry(settlementPanel.transform, "Поселение Драконов", "100 жителей", "Дружелюбие: 40");

        settlementPanel.SetActive(false);
    }

    private void CreateSettlementEntry(Transform parent, string name, string population, string friendliness)
    {
        GameObject settObj = new GameObject(name);
        settObj.transform.SetParent(parent, false);
        RectTransform settRect = settObj.AddComponent<RectTransform>();
        settRect.sizeDelta = new Vector2(0, 70);

        Image settBg = settObj.AddComponent<Image>();
        settBg.color = new Color(0.2f, 0.15f, 0.05f, 0.7f);

        VerticalLayoutGroup settLayout = settObj.AddComponent<VerticalLayoutGroup>();
        settLayout.spacing = 2;

        TextMeshProUGUI settName = settObj.AddComponent<TextMeshProUGUI>();
        settName.text = name;
        settName.fontSize = 20;
        settName.color = Color.yellow;

        TextMeshProUGUI settPop = settObj.AddComponent<TextMeshProUGUI>();
        settPop.text = population;
        settPop.fontSize = 14;
        settPop.color = Color.cyan;

        TextMeshProUGUI settFriend = settObj.AddComponent<TextMeshProUGUI>();
        settFriend.text = friendliness;
        settFriend.fontSize = 14;
        settFriend.color = new Color(0.5f, 1, 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            ToggleEquipmentPanel();
        if (Input.GetKeyDown(KeyCode.F2))
            ToggleSkillsPanel();
        if (Input.GetKeyDown(KeyCode.F3))
            ToggleMagicPanel();
        if (Input.GetKeyDown(KeyCode.F4))
            ToggleAchievementPanel();
        if (Input.GetKeyDown(KeyCode.F5))
            ToggleSettlementPanel();
    }

    private void ToggleEquipmentPanel() => TogglePanel(ref equipmentOpen, equipmentPanel);
    private void ToggleSkillsPanel() => TogglePanel(ref skillsOpen, skillsPanel);
    private void ToggleMagicPanel() => TogglePanel(ref magicOpen, magicPanel);
    private void ToggleAchievementPanel() => TogglePanel(ref achievementOpen, achievementPanel);
    private void ToggleSettlementPanel() => TogglePanel(ref settlementOpen, settlementPanel);

    private void TogglePanel(ref bool isOpen, GameObject panel)
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
    }
}
