using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarVisualization : MonoBehaviour
{
    private GameObject warPanel;
    private bool isWarOpen = false;

    private void Start()
    {
        CreateWarPanel();
    }

    private void CreateWarPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        warPanel = new GameObject("WarPanel");
        warPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = warPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.25f, 0.2f);
        panelRect.anchorMax = new Vector2(0.75f, 0.8f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelBg = warPanel.AddComponent<Image>();
        panelBg.color = new Color(0.1f, 0.05f, 0.05f, 0.95f);

        VerticalLayoutGroup layoutGroup = warPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 15;
        layoutGroup.padding = new RectOffset(30, 30, 30, 30);
        layoutGroup.childForceExpandWidth = true;

        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(warPanel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 50);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "⚔️ ВОЙНЫ И КОНФЛИКТЫ ⚔️";
        titleText.fontSize = 40;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(1, 0.2f, 0.2f);

        // Список войн
        CreateWarsList(warPanel.transform);

        // Информация о фракциях
        CreateFactionsInfo(warPanel.transform);

        warPanel.SetActive(false);
    }

    private void CreateWarsList(Transform parent)
    {
        GameObject listObj = new GameObject("WarsList");
        listObj.transform.SetParent(parent, false);

        RectTransform listRect = listObj.AddComponent<RectTransform>();
        listRect.sizeDelta = new Vector2(0, 150);

        VerticalLayoutGroup layoutGroup = listObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 5;
        layoutGroup.childForceExpandWidth = true;

        TextMeshProUGUI headerText = listObj.AddComponent<TextMeshProUGUI>();
        headerText.text = "Активные войны:";
        headerText.fontSize = 24;
        headerText.color = Color.yellow;

        // Здесь будут отображаться активные войны
        foreach (var war in WarManager.Instance.GetActiveWars())
        {
            GameObject warItemObj = new GameObject($"{war.attacker} vs {war.defender}");
            warItemObj.transform.SetParent(listObj.transform, false);

            RectTransform warItemRect = warItemObj.AddComponent<RectTransform>();
            warItemRect.sizeDelta = new Vector2(0, 30);

            TextMeshProUGUI warText = warItemObj.AddComponent<TextMeshProUGUI>();
            warText.text = $"{war.attacker} ⚔️ {war.defender} ({war.progress:F1}/{war.duration:F1}s)";
            warText.fontSize = 18;
            warText.color = new Color(1, 0.5f, 0.5f);
        }
    }

    private void CreateFactionsInfo(Transform parent)
    {
        GameObject factionsObj = new GameObject("FactionsInfo");
        factionsObj.transform.SetParent(parent, false);

        RectTransform factionsRect = factionsObj.AddComponent<RectTransform>();
        factionsRect.sizeDelta = new Vector2(0, 200);

        VerticalLayoutGroup layoutGroup = factionsObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 10;
        layoutGroup.childForceExpandWidth = true;

        TextMeshProUGUI headerText = factionsObj.AddComponent<TextMeshProUGUI>();
        headerText.text = "Информация о фракциях:";
        headerText.fontSize = 24;
        headerText.color = Color.yellow;

        // Список фракций
        foreach (var factionEntry in WarManager.Instance.GetAllFactions())
        {
            var faction = factionEntry.Value;
            
            GameObject factionObj = new GameObject(faction.factionName);
            factionObj.transform.SetParent(factionsObj.transform, false);

            RectTransform factionRect = factionObj.AddComponent<RectTransform>();
            factionRect.sizeDelta = new Vector2(0, 40);

            TextMeshProUGUI factionText = factionObj.AddComponent<TextMeshProUGUI>();
            factionText.text = $"{faction.factionName}: Strength {faction.strength} | Territory {faction.territory} | Resources {faction.resources:F0}";
            factionText.fontSize = 16;
            factionText.color = Color.green;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleWarPanel();
        }
    }

    private void ToggleWarPanel()
    {
        isWarOpen = !isWarOpen;
        warPanel.SetActive(isWarOpen);
        
        if (isWarOpen)
        {
            Time.timeScale = 0f;
            Debug.Log("War panel opened");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("War panel closed");
        }
    }
}
