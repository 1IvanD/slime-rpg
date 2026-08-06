using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DemonSummonUI : MonoBehaviour
{
    private GameObject summonPanel;
    private bool isSummonPanelOpen = false;
    private DemonSummonSystem summonSystem;
    private EconomySystem economy;

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            summonSystem = player.GetComponent<DemonSummonSystem>();
        }
        economy = EconomySystem.Instance;
        CreateSummonPanel();
    }

    private void CreateSummonPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        summonPanel = new GameObject("DemonSummonPanel");
        summonPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = summonPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.2f, 0.15f);
        panelRect.anchorMax = new Vector2(0.8f, 0.85f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelBg = summonPanel.AddComponent<Image>();
        panelBg.color = new Color(0.05f, 0.02f, 0.1f, 0.95f);

        VerticalLayoutGroup layoutGroup = summonPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 15;
        layoutGroup.padding = new RectOffset(20, 20, 20, 20);
        layoutGroup.childForceExpandWidth = true;

        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(summonPanel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "👿 ПРИЗЫВ ДЕМОНОВ 👿";
        titleText.fontSize = 48;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(1, 0.2f, 0.8f);

        // Информация об энергии
        CreateEnergyInfo(summonPanel.transform);

        // Список демонов для призыва
        CreateDemonsList(summonPanel.transform);

        // Кнопка закрытия
        GameObject closeButtonObj = new GameObject("CloseButton");
        closeButtonObj.transform.SetParent(summonPanel.transform, false);

        RectTransform closeRect = closeButtonObj.AddComponent<RectTransform>();
        closeRect.sizeDelta = new Vector2(0, 50);

        Image closeImage = closeButtonObj.AddComponent<Image>();
        closeImage.color = new Color(0.7f, 0.2f, 0.2f, 0.8f);

        Button closeButton = closeButtonObj.AddComponent<Button>();
        closeButton.targetGraphic = closeImage;
        closeButton.onClick.AddListener(() => ToggleSummonPanel());

        TextMeshProUGUI closeText = closeButtonObj.AddComponent<TextMeshProUGUI>();
        closeText.text = "ЗАКРЫТЬ [D]";
        closeText.fontSize = 24;
        closeText.alignment = TextAlignmentOptions.Center;
        closeText.color = Color.white;

        summonPanel.SetActive(false);
    }

    private void CreateEnergyInfo(Transform parent)
    {
        GameObject energyObj = new GameObject("EnergyInfo");
        energyObj.transform.SetParent(parent, false);

        RectTransform energyRect = energyObj.AddComponent<RectTransform>();
        energyRect.sizeDelta = new Vector2(0, 50);

        TextMeshProUGUI energyText = energyObj.AddComponent<TextMeshProUGUI>();
        energyText.text = "Энергия призыва: 0/100";
        energyText.fontSize = 28;
        energyText.alignment = TextAlignmentOptions.Center;
        energyText.color = new Color(1, 0.5f, 0.2f);
    }

    private void CreateDemonsList(Transform parent)
    {
        GameObject listObj = new GameObject("DemonsList");
        listObj.transform.SetParent(parent, false);

        RectTransform listRect = listObj.AddComponent<RectTransform>();
        listRect.sizeDelta = new Vector2(0, 400);

        VerticalLayoutGroup layoutGroup = listObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 10;
        layoutGroup.childForceExpandWidth = true;

        // Получить всех демонов
        Dictionary<string, DemonLord> allDemons = DemonManager.Instance.GetAllDemons();
        int count = 0;
        
        foreach (var demonEntry in allDemons)
        {
            if (count >= 12) break; // Показать первых 12 демонов
            
            CreateDemonOption(listObj.transform, demonEntry.Value);
            count++;
        }
    }

    private void CreateDemonOption(Transform parent, DemonLord demon)
    {
        GameObject optionObj = new GameObject(demon.name);
        optionObj.transform.SetParent(parent, false);

        RectTransform optionRect = optionObj.AddComponent<RectTransform>();
        optionRect.sizeDelta = new Vector2(0, 60);

        Image optionBg = optionObj.AddComponent<Image>();
        optionBg.color = GetDemonColor(demon.rank);

        Button optionButton = optionObj.AddComponent<Button>();
        optionButton.targetGraphic = optionBg;
        optionButton.onClick.AddListener(() => AttemptSummonDemon(demon));

        // Информация о демоне
        TextMeshProUGUI demonInfo = optionObj.AddComponent<TextMeshProUGUI>();
        demonInfo.text = $"{demon.name} ({demon.rank}) | Сила: {demon.power} | HP: {demon.health:F0}";
        demonInfo.fontSize = 18;
        demonInfo.alignment = TextAlignmentOptions.Center;
        demonInfo.color = Color.white;
    }

    private Color GetDemonColor(DemonRank rank)
    {
        switch (rank)
        {
            case DemonRank.LowerDemon:
                return new Color(0.3f, 0.3f, 0.5f, 0.7f);
            case DemonRank.MidDemon:
                return new Color(0.5f, 0.2f, 0.5f, 0.7f);
            case DemonRank.UpperDemon:
                return new Color(0.7f, 0.1f, 0.1f, 0.7f);
            case DemonRank.ArcDemon:
                return new Color(1, 0.1f, 0.1f, 0.7f);
            case DemonRank.PrimordialDemon:
                return new Color(1, 0.5f, 0, 0.7f);
            case DemonRank.DemonLord:
                return new Color(1, 0, 0, 0.7f);
            default:
                return new Color(0.5f, 0.5f, 0.5f, 0.7f);
        }
    }

    private void AttemptSummonDemon(DemonLord demon)
    {
        if (summonSystem != null)
        {
            if (summonSystem.SummonDemon(demon))
            {
                // Вычесть стоимость в золоте в зависимости от ранга
                float cost = demon.power * 10f;
                if (economy != null)
                {
                    economy.SpendGold(cost);
                }
                Debug.Log($"Successfully summoned: {demon.name}");
            }
            else
            {
                Debug.Log($"Failed to summon: {demon.name}");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleSummonPanel();
        }
    }

    private void ToggleSummonPanel()
    {
        isSummonPanelOpen = !isSummonPanelOpen;
        summonPanel.SetActive(isSummonPanelOpen);
        
        if (isSummonPanelOpen)
        {
            Time.timeScale = 0f;
            Debug.Log("Demon summon panel opened");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Demon summon panel closed");
        }
    }
}
