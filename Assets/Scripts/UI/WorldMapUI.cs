using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private Canvas mapCanvas;
    private GameObject mapPanel;
    private Image mapImage;
    private Transform dungeonPointsContainer;
    private Transform settlementPointsContainer;
    private bool isMapOpen = false;

    private void Start()
    {
        CreateWorldMap();
    }

    private void CreateWorldMap()
    {
        // Главная панель карты
        mapPanel = new GameObject("WorldMap");
        mapPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        RectTransform mapRect = mapPanel.AddComponent<RectTransform>();
        mapRect.anchorMin = Vector2.zero;
        mapRect.anchorMax = Vector2.one;
        mapRect.offsetMin = Vector2.zero;
        mapRect.offsetMax = Vector2.zero;

        Image mapBg = mapPanel.AddComponent<Image>();
        mapBg.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);

        // Контейнер для подземелий
        dungeonPointsContainer = new GameObject("DungeonPoints").transform;
        dungeonPointsContainer.SetParent(mapPanel.transform, false);

        // Контейнер для поселений
        settlementPointsContainer = new GameObject("SettlementPoints").transform;
        settlementPointsContainer.SetParent(mapPanel.transform, false);

        CreateMapContent();
        mapPanel.SetActive(false);
    }

    private void CreateMapContent()
    {
        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(mapPanel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.9f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "МИРОВАЯ КАРТА";
        titleText.fontSize = 48;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.cyan;

        // Контейнер информации
        GameObject infoContainer = new GameObject("InfoContainer");
        infoContainer.transform.SetParent(mapPanel.transform, false);

        RectTransform infoRect = infoContainer.AddComponent<RectTransform>();
        infoRect.anchorMin = Vector2.zero;
        infoRect.anchorMax = new Vector2(0.25f, 0.9f);
        infoRect.offsetMin = new Vector2(10, 10);
        infoRect.offsetMax = new Vector2(-10, -10);

        Image infoBg = infoContainer.AddComponent<Image>();
        infoBg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        VerticalLayoutGroup layoutGroup = infoContainer.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 5;
        layoutGroup.padding = new RectOffset(10, 10, 10, 10);
        layoutGroup.childForceExpandWidth = true;

        // Подземелья
        CreateMapSection(infoContainer.transform, "ПОДЗЕМЕЛЬЯ", DungeonManager.Instance.GetAllDungeons().Values, true);

        // Поселения
        CreateMapSection(infoContainer.transform, "ПОСЕЛЕНИЯ", SettlementManager.Instance.GetAllSettlements().Values, false);
    }

    private void CreateMapSection<T>(Transform parent, string title, System.Collections.Generic.IEnumerable<T> items, bool isDungeons)
    {
        // Заголовок секции
        GameObject titleObj = new GameObject(title);
        titleObj.transform.SetParent(parent, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 40);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = title;
        titleText.fontSize = 28;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.yellow;

        // Элементы списка
        if (isDungeons)
        {
            foreach (T item in items)
            {
                if (item is DungeonData dungeon)
                {
                    CreateMapItemButton(parent, dungeon.dungeonName, dungeon.isDiscovered);
                }
            }
        }
        else
        {
            foreach (T item in items)
            {
                if (item is SettlementData settlement)
                {
                    CreateMapItemButton(parent, settlement.settlementName, settlement.isDiscovered);
                }
            }
        }
    }

    private void CreateMapItemButton(Transform parent, string itemName, bool discovered)
    {
        GameObject buttonObj = new GameObject(itemName);
        buttonObj.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(0, 35);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = discovered ? new Color(0.3f, 0.5f, 0.3f, 0.7f) : new Color(0.5f, 0.2f, 0.2f, 0.7f);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = discovered ? $"✓ {itemName}" : $"? {itemName}";
        buttonText.fontSize = 20;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        mapPanel.SetActive(isMapOpen);
        
        if (isMapOpen)
        {
            Time.timeScale = 0f; // Пауза во время просмотра карты
            Debug.Log("Map opened");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Map closed");
        }
    }
}
