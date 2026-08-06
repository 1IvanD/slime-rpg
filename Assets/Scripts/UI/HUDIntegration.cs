using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HUDIntegration : MonoBehaviour
{
    private Canvas mainCanvas;
    private QuestSystem questSystem;
    private InventorySystem inventorySystem;
    private AdvancedCombatSystem combatSystem;
    private EconomySystem economySystem;

    // HUD элементы для квестов
    private TextMeshProUGUI questNameText;
    private TextMeshProUGUI questObjectiveText;
    private Image questProgressBar;
    private TextMeshProUGUI questProgressText;

    // HUD элементы для инвентаря
    private TextMeshProUGUI inventoryWeightText;
    private TextMeshProUGUI inventoryCountText;

    // HUD элементы для боевой системы
    private TextMeshProUGUI manaText;
    private Image manaBar;
    private TextMeshProUGUI comboText;

    private void Start()
    {
        mainCanvas = FindObjectOfType<Canvas>();
        questSystem = QuestSystem.Instance;
        inventorySystem = InventorySystem.Instance;
        combatSystem = FindObjectOfType<AdvancedCombatSystem>();
        economySystem = EconomySystem.Instance;

        CreateHUDElements();
    }

    private void CreateHUDElements()
    {
        // Панель квестов (верхний левый угол)
        GameObject questPanel = new GameObject("QuestHUD");
        questPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform questRect = questPanel.AddComponent<RectTransform>();
        questRect.anchorMin = new Vector2(0, 0.7f);
        questRect.anchorMax = new Vector2(0.3f, 1);
        questRect.offsetMin = new Vector2(10, 10);
        questRect.offsetMax = new Vector2(-10, -10);

        Image questBg = questPanel.AddComponent<Image>();
        questBg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        VerticalLayoutGroup questLayout = questPanel.AddComponent<VerticalLayoutGroup>();
        questLayout.spacing = 5;
        questLayout.padding = new RectOffset(10, 10, 10, 10);

        // Название квеста
        GameObject questNameObj = new GameObject("QuestName");
        questNameObj.transform.SetParent(questPanel.transform, false);
        questNameText = questNameObj.AddComponent<TextMeshProUGUI>();
        questNameText.text = "Нет активного квеста";
        questNameText.fontSize = 24;
        questNameText.color = Color.yellow;

        // Описание квеста
        GameObject questObjObj = new GameObject("QuestObjective");
        questObjObj.transform.SetParent(questPanel.transform, false);
        questObjectiveText = questObjObj.AddComponent<TextMeshProUGUI>();
        questObjectiveText.text = "";
        questObjectiveText.fontSize = 16;
        questObjectiveText.color = Color.white;

        // Прогресс квеста
        GameObject progressBarObj = new GameObject("ProgressBar");
        progressBarObj.transform.SetParent(questPanel.transform, false);
        RectTransform progRect = progressBarObj.AddComponent<RectTransform>();
        progRect.sizeDelta = new Vector2(0, 20);
        questProgressBar = progressBarObj.AddComponent<Image>();
        questProgressBar.color = Color.green;

        GameObject progressTextObj = new GameObject("ProgressText");
        progressTextObj.transform.SetParent(questPanel.transform, false);
        questProgressText = progressTextObj.AddComponent<TextMeshProUGUI>();
        questProgressText.text = "Прогресс: 0/0";
        questProgressText.fontSize = 14;
        questProgressText.color = Color.white;

        // Панель инвентаря (нижний левый угол)
        GameObject inventoryPanel = new GameObject("InventoryHUD");
        inventoryPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform invRect = inventoryPanel.AddComponent<RectTransform>();
        invRect.anchorMin = new Vector2(0, 0.4f);
        invRect.anchorMax = new Vector2(0.2f, 0.65f);
        invRect.offsetMin = new Vector2(10, 10);
        invRect.offsetMax = new Vector2(-10, -10);

        Image invBg = inventoryPanel.AddComponent<Image>();
        invBg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        VerticalLayoutGroup invLayout = inventoryPanel.AddComponent<VerticalLayoutGroup>();
        invLayout.spacing = 5;
        invLayout.padding = new RectOffset(10, 10, 10, 10);

        // Вес инвентаря
        GameObject weightObj = new GameObject("Weight");
        weightObj.transform.SetParent(inventoryPanel.transform, false);
        inventoryWeightText = weightObj.AddComponent<TextMeshProUGUI>();
        inventoryWeightText.text = "Вес: 0/100";
        inventoryWeightText.fontSize = 18;
        inventoryWeightText.color = Color.cyan;

        // Количество предметов
        GameObject countObj = new GameObject("Count");
        countObj.transform.SetParent(inventoryPanel.transform, false);
        inventoryCountText = countObj.AddComponent<TextMeshProUGUI>();
        inventoryCountText.text = "Предметы: 0/50";
        inventoryCountText.fontSize = 18;
        inventoryCountText.color = Color.green;

        // Панель боевой системы (правый угол)
        GameObject combatPanel = new GameObject("CombatHUD");
        combatPanel.transform.SetParent(mainCanvas.transform, false);

        RectTransform combatRect = combatPanel.AddComponent<RectTransform>();
        combatRect.anchorMin = new Vector2(0.75f, 0.1f);
        combatRect.anchorMax = new Vector2(1, 0.35f);
        combatRect.offsetMin = new Vector2(-10, 10);
        combatRect.offsetMax = new Vector2(-10, -10);

        Image combatBg = combatPanel.AddComponent<Image>();
        combatBg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        VerticalLayoutGroup combatLayout = combatPanel.AddComponent<VerticalLayoutGroup>();
        combatLayout.spacing = 5;
        combatLayout.padding = new RectOffset(10, 10, 10, 10);

        // Мана
        GameObject manaObj = new GameObject("Mana");
        manaObj.transform.SetParent(combatPanel.transform, false);
        manaText = manaObj.AddComponent<TextMeshProUGUI>();
        manaText.text = "Мана: 100/100";
        manaText.fontSize = 18;
        manaText.color = Color.blue;

        // Полоса маны
        GameObject manaBarObj = new GameObject("ManaBar");
        manaBarObj.transform.SetParent(combatPanel.transform, false);
        RectTransform manaBarRect = manaBarObj.AddComponent<RectTransform>();
        manaBarRect.sizeDelta = new Vector2(0, 20);
        manaBar = manaBarObj.AddComponent<Image>();
        manaBar.color = Color.blue;

        // Комбо
        GameObject comboObj = new GameObject("Combo");
        comboObj.transform.SetParent(combatPanel.transform, false);
        comboText = comboObj.AddComponent<TextMeshProUGUI>();
        comboText.text = "Комбо: 0";
        comboText.fontSize = 20;
        comboText.color = new Color(1, 0.5f, 0);
    }

    private void Update()
    {
        UpdateQuestHUD();
        UpdateInventoryHUD();
        UpdateCombatHUD();
    }

    private void UpdateQuestHUD()
    {
        QuestData currentQuest = questSystem.GetCurrentMainQuest();
        if (currentQuest != null)
        {
            questNameText.text = currentQuest.questName;
            questObjectiveText.text = currentQuest.objective;
            questProgressText.text = $"Прогресс: {currentQuest.progress}/{currentQuest.progressMax}";
            questProgressBar.fillAmount = currentQuest.progress / currentQuest.progressMax;
        }
        else
        {
            questNameText.text = "Нет активного квеста";
            questObjectiveText.text = "";
            questProgressText.text = "";
        }
    }

    private void UpdateInventoryHUD()
    {
        inventoryWeightText.text = $"Вес: {inventorySystem.GetCurrentWeight():F1}/{inventorySystem.GetMaxWeight()}";
        inventoryCountText.text = $"Предметы: {inventorySystem.GetItemCount()}/50";
    }

    private void UpdateCombatHUD()
    {
        if (combatSystem != null)
        {
            manaText.text = $"Мана: {combatSystem.GetMana():F0}/{combatSystem.GetMaxMana()}";
            manaBar.fillAmount = combatSystem.GetMana() / combatSystem.GetMaxMana();
            comboText.text = $"Комбо: {combatSystem.GetComboCounter()}";
        }
    }
}
