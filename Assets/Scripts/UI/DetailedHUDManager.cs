using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailedHUDManager : MonoBehaviour
{
    private Player player;
    private PlayerStats playerStats;
    private EconomySystem economy;
    private DemonSummonSystem summonSystem;

    // Основные элементы HUD
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI experienceText;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI healthText;
    private Image healthBar;
    private TextMeshProUGUI manaText;
    private Image manaBar;
    private TextMeshProUGUI skillCountText;
    private TextMeshProUGUI summonCountText;
    private TextMeshProUGUI summonEnergyText;
    private Image summonEnergyBar;

    // Информационные панели
    private TextMeshProUGUI playerNameText;
    private TextMeshProUGUI difficultyText;
    private TextMeshProUGUI playtimeText;
    private TextMeshProUGUI enemiesDefeatedText;

    private float playTime = 0f;

    private void Start()
    {
        FindAndInitializeUIElements();
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            playerStats = player.GetStats();
            summonSystem = player.GetComponent<DemonSummonSystem>();
        }
        economy = EconomySystem.Instance;
    }

    private void FindAndInitializeUIElements()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        // Поиск существующих элементов
        Transform hudPanel = canvas.transform.Find("HUDPanel");
        if (hudPanel != null)
        {
            healthText = hudPanel.Find("HealthDisplay/HealthText")?.GetComponent<TextMeshProUGUI>();
            healthBar = hudPanel.Find("HealthDisplay/HealthBar/Fill")?.GetComponent<Image>();
            levelText = hudPanel.Find("LevelDisplay/LevelText")?.GetComponent<TextMeshProUGUI>();
            experienceText = hudPanel.Find("ExperienceDisplay/ExperienceText")?.GetComponent<TextMeshProUGUI>();
        }

        // Создание новой расширенной информационной панели
        CreateDetailedInfoPanel(canvas.transform);
    }

    private void CreateDetailedInfoPanel(Transform canvasTransform)
    {
        // Главная панель информации
        GameObject infoPanel = new GameObject("DetailedInfoPanel");
        infoPanel.transform.SetParent(canvasTransform, false);

        RectTransform infoRect = infoPanel.AddComponent<RectTransform>();
        infoRect.anchorMin = new Vector2(0, 0);
        infoRect.anchorMax = new Vector2(0.35f, 0.25f);
        infoRect.offsetMin = new Vector2(10, 10);
        infoRect.offsetMax = new Vector2(-10, -10);

        Image infoBg = infoPanel.AddComponent<Image>();
        infoBg.color = new Color(0.05f, 0.05f, 0.1f, 0.85f);

        GridLayoutGroup gridLayout = infoPanel.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(150, 40);
        gridLayout.spacing = new Vector2(5, 5);

        // Золото
        CreateInfoItem(infoPanel.transform, "Gold: ", out goldText, Color.yellow);

        // Опыт
        CreateInfoItem(infoPanel.transform, "EXP: ", out experienceText, new Color(0.5f, 1, 0.5f));

        // Уровень
        CreateInfoItem(infoPanel.transform, "Lvl: ", out levelText, Color.cyan);

        // Здоровье
        CreateInfoItem(infoPanel.transform, "HP: ", out healthText, Color.red);

        // Количество призванных демонов
        CreateInfoItem(infoPanel.transform, "Demons: ", out summonCountText, new Color(1, 0.5f, 0));

        // Энергия призыва
        CreateInfoItem(infoPanel.transform, "Summon: ", out summonEnergyText, new Color(1, 0.2f, 0.8f));
    }

    private void CreateInfoItem(Transform parent, string label, out TextMeshProUGUI valueText, Color color)
    {
        GameObject itemObj = new GameObject(label);
        itemObj.transform.SetParent(parent, false);

        RectTransform itemRect = itemObj.AddComponent<RectTransform>();
        itemRect.sizeDelta = new Vector2(150, 40);

        TextMeshProUGUI itemText = itemObj.AddComponent<TextMeshProUGUI>();
        itemText.text = label + "0";
        itemText.fontSize = 20;
        itemText.alignment = TextAlignmentOptions.Center;
        itemText.color = color;

        valueText = itemText;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (playerStats != null)
        {
            if (healthText != null)
                healthText.text = $"HP: {playerStats.Health:F0}/{playerStats.MaxHealth:F0}";

            if (levelText != null)
                levelText.text = $"Level: {playerStats.Level}";

            if (experienceText != null)
                experienceText.text = $"Exp: {playerStats.Experience:F0}/{playerStats.ExperienceThreshold:F0}";

            if (skillCountText != null)
                skillCountText.text = $"Skills: {playerStats.UniqueSkillsLearned}";
        }

        if (economy != null)
        {
            if (goldText != null)
                goldText.text = $"Gold: {economy.GetGold():F0}";
        }

        if (summonSystem != null)
        {
            if (summonCountText != null)
                summonCountText.text = $"Demons: {summonSystem.GetSummonedDemons().Count}/3";

            if (summonEnergyText != null)
                summonEnergyText.text = $"Summon: {summonSystem.GetSummonEnergy():F0}/{summonSystem.GetMaxSummonEnergy():F0}";
        }
    }
}
