using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Legacy WorldMap UI implementation. Renamed to avoid conflict with canonical WorldMapUI in WorldMap folder.
public class WorldMapUI_Legacy : MonoBehaviour
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
        mapPanel = new GameObject("WorldMap_Legacy");
        var canvas = FindObjectOfType<Canvas>();
        if (canvas != null) mapPanel.transform.SetParent(canvas.transform, false);

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
        titleText.text = "МИРОВАЯ КАРТА (Legacy)";
        titleText.fontSize = 48;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.cyan;

        // Minimal content to avoid null refs in legacy scenes
    }
}
