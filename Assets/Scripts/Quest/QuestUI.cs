using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Simple left-side Quest Log UI created at runtime. Shows active and completed quests.
public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance { get; private set; }

    private Canvas canvas;
    private RectTransform panel;
    private VerticalLayoutGroup layout;
    private Dictionary<string, Text> questTexts = new Dictionary<string, Text>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CreateUI();
    }

    private void CreateUI()
    {
        canvas = new GameObject("QuestCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(canvas.gameObject);

        var panelGO = new GameObject("QuestPanel");
        panelGO.transform.SetParent(canvas.transform, false);
        panel = panelGO.AddComponent<RectTransform>();
        var img = panelGO.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.45f);
        panel.anchorMin = new Vector2(0f, 0f);
        panel.anchorMax = new Vector2(0f, 1f);
        panel.pivot = new Vector2(0f, 0.5f);
        panel.sizeDelta = new Vector2(300f, 0f);
        panel.anchoredPosition = new Vector2(0f, 0f);

        var scrollGO = new GameObject("QuestScroll");
        scrollGO.transform.SetParent(panel, false);
        var rt = scrollGO.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = new Vector2(8f, 8f);
        rt.offsetMax = new Vector2(-8f, -8f);

        var scroll = scrollGO.AddComponent<ScrollRect>();
        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(scrollGO.transform, false);
        var contentRT = contentGO.AddComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0f, 1f);
        contentRT.anchorMax = new Vector2(1f, 1f);
        contentRT.pivot = new Vector2(0.5f, 1f);
        contentRT.anchoredPosition = Vector2.zero;
        contentRT.sizeDelta = new Vector2(0f, 2000f);

        var contentLayout = contentGO.AddComponent<VerticalLayoutGroup>();
        contentLayout.childForceExpandHeight = false;
        contentLayout.childControlHeight = true;

        scroll.content = contentRT;
        scroll.vertical = true;

        layout = contentLayout;

        // initial population
        RefreshAll();
    }

    public void RefreshAll()
    {
        // clear existing
        foreach (var kv in questTexts)
        {
            if (kv.Value != null) Destroy(kv.Value.gameObject);
        }
        questTexts.Clear();

        var qs = QuestManager.Instance?.GetAllQuests();
        if (qs == null) return;

        foreach (var q in qs)
        {
            var line = new GameObject("QuestLine_" + q.id);
            line.transform.SetParent(layout.transform, false);
            var txt = line.AddComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = 14;
            txt.alignment = TextAnchor.MiddleLeft;
            txt.color = q.status == QuestDef.QuestStatus.Completed ? Color.gray : Color.white;
            txt.text = FormatQuestLine(q);
            questTexts[q.id] = txt;
        }
    }

    private string FormatQuestLine(QuestDef q)
    {
        string status = q.status == QuestDef.QuestStatus.Completed ? "[✓] " : (q.status == QuestDef.QuestStatus.Active ? "[•] " : "[ ] ");
        return status + q.displayName;
    }

    public void OnQuestUpdated(QuestDef q)
    {
        if (questTexts.TryGetValue(q.id, out var txt))
        {
            txt.text = FormatQuestLine(q);
            txt.color = q.status == QuestDef.QuestStatus.Completed ? Color.gray : Color.white;
        }
        else
        {
            RefreshAll();
        }
    }
}
