using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private static DialogueSystem instance;
    public static DialogueSystem Instance
    {
        get
        {
            if (instance == null)
            {
                var ds = FindObjectOfType<DialogueSystem>();
                if (ds == null)
                {
                    var go = new GameObject("DialogueSystem");
                    instance = go.AddComponent<DialogueSystem>();
                }
                else instance = ds;
            }
            return instance;
        }
    }

    private GameObject dialogRoot;
    private Text messageText;
    private List<Button> optionButtons = new List<Button>();

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowDialog(string message, string[] options, Action<int> onChoose)
    {
        EnsureUI();
        messageText.text = message;

        // Clear old
        foreach (var b in optionButtons) Destroy(b.gameObject);
        optionButtons.Clear();

        for (int i = 0; i < options.Length; i++)
        {
            int idx = i;
            var btnGO = CreateButton(options[i]);
            btnGO.transform.SetParent(dialogRoot.transform, false);
            var btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { HideDialog(); onChoose?.Invoke(idx); });
            optionButtons.Add(btn);
        }

        dialogRoot.SetActive(true);
    }

    private GameObject CreateButton(string text)
    {
        var go = new GameObject("OptionButton");
        var img = go.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        var btn = go.AddComponent<Button>();

        var txtGO = new GameObject("Text");
        txtGO.transform.SetParent(go.transform, false);
        var txt = txtGO.AddComponent<Text>();
        txt.text = text;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.white;
        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 40);
        return go;
    }

    private void EnsureUI()
    {
        if (dialogRoot != null) return;
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("DialogueSystem: Canvas not found.");
            return;
        }

        dialogRoot = new GameObject("DialogRoot");
        dialogRoot.transform.SetParent(canvas.transform, false);
        var bg = dialogRoot.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.8f);
        var rt = dialogRoot.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.2f, 0.2f);
        rt.anchorMax = new Vector2(0.8f, 0.5f);
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        var msgGO = new GameObject("Message");
        msgGO.transform.SetParent(dialogRoot.transform, false);
        messageText = msgGO.AddComponent<Text>();
        messageText.color = Color.white;
        messageText.alignment = TextAnchor.UpperLeft;
        var msgRT = msgGO.GetComponent<RectTransform>();
        msgRT.anchorMin = new Vector2(0.05f, 0.4f);
        msgRT.anchorMax = new Vector2(0.95f, 0.95f);

        dialogRoot.SetActive(false);
    }

    public void HideDialog()
    {
        if (dialogRoot != null) dialogRoot.SetActive(false);
    }
}
