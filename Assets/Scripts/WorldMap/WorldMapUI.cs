using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    public static WorldMapUI Instance { get; private set; }

    private Canvas canvas;
    private GameObject panel;
    private Text panelText;
    private Button confirmBtn;
    private Button cancelBtn;
    private MapNode pendingNode;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateUI();
    }

    private void CreateUI()
    {
        canvas = new GameObject("WorldMapCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(canvas.gameObject);

        panel = new GameObject("TravelConfirmPanel");
        panel.transform.SetParent(canvas.transform, false);
        var img = panel.AddComponent<Image>();
        img.color = new Color(0f,0f,0f,0.7f);
        var rt = panel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(420, 140);
        rt.anchorMin = new Vector2(0.5f, 0.1f);
        rt.anchorMax = new Vector2(0.5f, 0.1f);
        rt.anchoredPosition = new Vector2(0, 80);
        panel.SetActive(false);

        var txtGO = new GameObject("Text"); txtGO.transform.SetParent(panel.transform, false);
        panelText = txtGO.AddComponent<Text>();
        panelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        panelText.color = Color.white;
        panelText.alignment = TextAnchor.MiddleCenter;
        var txtRT = txtGO.GetComponent<RectTransform>(); txtRT.anchorMin = new Vector2(0.05f, 0.4f); txtRT.anchorMax = new Vector2(0.95f, 0.85f); txtRT.offsetMin = txtRT.offsetMax = Vector2.zero;

        // Buttons
        var confirmGO = new GameObject("Confirm"); confirmGO.transform.SetParent(panel.transform, false);
        confirmBtn = confirmGO.AddComponent<Button>();
        var cimg = confirmGO.AddComponent<Image>(); cimg.color = new Color(0.2f,0.6f,0.2f,1f);
        var cRT = confirmGO.GetComponent<RectTransform>(); cRT.anchorMin = new Vector2(0.55f, 0.05f); cRT.anchorMax = new Vector2(0.9f, 0.25f); cRT.offsetMin = cRT.offsetMax = Vector2.zero;
        var ctxt = new GameObject("Text"); ctxt.transform.SetParent(confirmGO.transform, false); var ctext = ctxt.AddComponent<Text>(); ctext.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); ctext.text = "Travel"; ctext.color = Color.white; ctext.alignment = TextAnchor.MiddleCenter;

        var cancelGO = new GameObject("Cancel"); cancelGO.transform.SetParent(panel.transform, false);
        cancelBtn = cancelGO.AddComponent<Button>();
        var kimg = cancelGO.AddComponent<Image>(); kimg.color = new Color(0.6f,0.2f,0.2f,1f);
        var kRT = cancelGO.GetComponent<RectTransform>(); kRT.anchorMin = new Vector2(0.1f, 0.05f); kRT.anchorMax = new Vector2(0.45f, 0.25f); kRT.offsetMin = kRT.offsetMax = Vector2.zero;
        var ktxt = new GameObject("Text"); ktxt.transform.SetParent(cancelGO.transform, false); var ktext = ktxt.AddComponent<Text>(); ktext.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); ktext.text = "Cancel"; ktext.color = Color.white; ktext.alignment = TextAnchor.MiddleCenter;

        confirmBtn.onClick.AddListener(OnConfirmTravel);
        cancelBtn.onClick.AddListener(() => { HideConfirm(); });
    }

    public void ShowConfirm(MapNode node)
    {
        if (node == null) return;
        pendingNode = node;
        panelText.text = $"Travel to {node.displayName}?";
        panel.SetActive(true);
    }

    public void HideConfirm()
    {
        pendingNode = null;
        panel.SetActive(false);
    }

    private void OnConfirmTravel()
    {
        if (pendingNode == null) return;
        WorldMapManager.Instance?.TravelTo(pendingNode.id);
        HideConfirm();
    }
}
