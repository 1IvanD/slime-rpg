using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NegotiationSystem : MonoBehaviour
{
    private SettlementData currentSettlement;
    private GameObject negotiationPanel;
    private bool isNegotiating = false;

    private void Start()
    {
        CreateNegotiationPanel();
    }

    private void CreateNegotiationPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        negotiationPanel = new GameObject("NegotiationPanel");
        negotiationPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = negotiationPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.2f, 0.2f);
        panelRect.anchorMax = new Vector2(0.8f, 0.8f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelBg = negotiationPanel.AddComponent<Image>();
        panelBg.color = new Color(0.05f, 0.05f, 0.1f, 0.95f);

        VerticalLayoutGroup layoutGroup = negotiationPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 20;
        layoutGroup.padding = new RectOffset(30, 30, 30, 30);
        layoutGroup.childForceExpandWidth = true;

        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(negotiationPanel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 50);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "ПЕРЕГОВОРЫ";
        titleText.fontSize = 40;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.cyan;

        // Информация о поселении
        GameObject infoObj = new GameObject("SettlementInfo");
        infoObj.transform.SetParent(negotiationPanel.transform, false);

        RectTransform infoRect = infoObj.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(0, 80);

        TextMeshProUGUI infoText = infoObj.AddComponent<TextMeshProUGUI>();
        infoText.text = "Governor: Unknown\nPopulation: 0\nFriendliness: 50";
        infoText.fontSize = 24;
        infoText.alignment = TextAlignmentOptions.TopLeft;
        infoText.color = Color.white;

        // Параметры дружественности
        CreateFriendlinessBar(negotiationPanel.transform);

        // Кнопки действий
        GameObject actionsContainer = new GameObject("ActionsContainer");
        actionsContainer.transform.SetParent(negotiationPanel.transform, false);

        RectTransform actionsRect = actionsContainer.AddComponent<RectTransform>();
        actionsRect.sizeDelta = new Vector2(0, 80);

        GridLayoutGroup gridLayout = actionsContainer.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(200, 40);
        gridLayout.spacing = new Vector2(10, 10);

        // Кнопка мирных переговоров
        CreateNegotiationButton(actionsContainer.transform, "Мирные переговоры", () => {
            if (currentSettlement != null)
            {
                SettlementManager.Instance.NegotiateWithSettlement(false);
                UpdateNegotiationInfo();
            }
        });

        // Кнопка торговли
        CreateNegotiationButton(actionsContainer.transform, "Торговать", () => {
            if (currentSettlement != null)
            {
                SettlementManager.Instance.TradeWithSettlement(50);
                UpdateNegotiationInfo();
            }
        });

        // Кнопка угроз
        CreateNegotiationButton(actionsContainer.transform, "Угрозы", () => {
            if (currentSettlement != null)
            {
                SettlementManager.Instance.NegotiateWithSettlement(true);
                UpdateNegotiationInfo();
            }
        });

        // Кнопка объявить войну
        CreateNegotiationButton(actionsContainer.transform, "Объявить войну", () => {
            if (currentSettlement != null)
            {
                Debug.Log($"War declared against {currentSettlement.settlementName}!");
                CloseNegotiationPanel();
            }
        });

        // Кнопка закрыть
        CreateNegotiationButton(negotiationPanel.transform, "Закрыть", CloseNegotiationPanel);

        negotiationPanel.SetActive(false);
    }

    private void CreateFriendlinessBar(Transform parent)
    {
        GameObject barObj = new GameObject("FriendlinessBar");
        barObj.transform.SetParent(parent, false);

        RectTransform barRect = barObj.AddComponent<RectTransform>();
        barRect.sizeDelta = new Vector2(0, 40);

        Image barBg = barObj.AddComponent<Image>();
        barBg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(barObj.transform, false);

        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0.5f, 1);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = new Color(1, 0.5f, 0, 0.8f);

        TextMeshProUGUI barText = barObj.AddComponent<TextMeshProUGUI>();
        barText.text = "Friendliness: 50/100";
        barText.fontSize = 20;
        barText.alignment = TextAlignmentOptions.Center;
        barText.color = Color.white;
    }

    private void CreateNegotiationButton(Transform parent, string label, System.Action onClick)
    {
        GameObject buttonObj = new GameObject(label);
        buttonObj.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(0, 40);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.5f, 0.3f, 0.8f);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.colors = new ColorBlock()
        {
            normalColor = new Color(0.3f, 0.5f, 0.3f, 0.8f),
            highlightedColor = new Color(0.5f, 0.7f, 0.5f, 1),
            pressedColor = new Color(0.2f, 0.4f, 0.2f, 1),
            disabledColor = new Color(0.3f, 0.5f, 0.3f, 0.5f),
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        button.onClick.AddListener(() => onClick?.Invoke());

        TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = label;
        buttonText.fontSize = 18;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }

    public void StartNegotiation(SettlementData settlement)
    {
        currentSettlement = settlement;
        isNegotiating = true;
        negotiationPanel.SetActive(true);
        UpdateNegotiationInfo();
    }

    private void UpdateNegotiationInfo()
    {
        if (currentSettlement == null) return;
        
        // Обновить информацию о поселении
        Debug.Log($"Negotiating with {currentSettlement.settlementName}\nFriendliness: {currentSettlement.friendliness}");
    }

    private void CloseNegotiationPanel()
    {
        isNegotiating = false;
        negotiationPanel.SetActive(false);
        currentSettlement = null;
    }
}
