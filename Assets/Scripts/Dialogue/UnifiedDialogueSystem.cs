using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// New unified DialogueSystem (replaces older gameplay DialogueSystem and Dialogue/DialogueSystem duplicates)
public class UnifiedDialogueSystem : MonoBehaviour
{
    public static UnifiedDialogueSystem Instance { get; private set; }

    [Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public string nextNodeId;
        public int rewardGold;
        public string consequence;
    }

    [Serializable]
    public class DialogueNode
    {
        public string nodeId;
        public string npcName;
        public string dialogueText;
        public List<DialogueChoice> choices = new List<DialogueChoice>();
    }

    private Dictionary<string, DialogueNode> nodes = new Dictionary<string, DialogueNode>();
    private DialogueNode currentNode;

    // UI
    private GameObject dialogRoot;
    private Text messageText;
    private List<Button> optionButtons = new List<Button>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeSample();
    }

    private void InitializeSample()
    {
        CreateNode(new DialogueNode { nodeId = "hello", npcName = "Guide", dialogueText = "Добро пожаловать!", choices = new List<DialogueChoice> { new DialogueChoice { choiceText = "Привет", nextNodeId = "hello_resp" } } });
        CreateNode(new DialogueNode { nodeId = "hello_resp", npcName = "Guide", dialogueText = "Рада тебя видеть.", choices = new List<DialogueChoice>() });
    }

    public void CreateNode(DialogueNode node)
    {
        if (node == null || string.IsNullOrEmpty(node.nodeId)) return;
        nodes[node.nodeId] = node;
    }

    public void StartDialogue(string nodeId)
    {
        if (nodes.TryGetValue(nodeId, out var node))
        {
            currentNode = node;
            ShowNode(node);
        }
        else Debug.LogWarning("UnifiedDialogueSystem: node not found " + nodeId);
    }

    private void ShowNode(DialogueNode node)
    {
        EnsureUI();
        if (messageText != null) messageText.text = $"{node.npcName}:\n{node.dialogueText}";

        foreach (var b in optionButtons) { if (b != null) Destroy(b.gameObject); }
        optionButtons.Clear();

        for (int i = 0; i < node.choices.Count; i++)
        {
            int idx = i;
            var btnGO = CreateButton(node.choices[i].choiceText);
            btnGO.transform.SetParent(dialogRoot.transform, false);
            var btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { OnChoice(idx); });
            optionButtons.Add(btn);
        }

        dialogRoot.SetActive(true);
    }

    private void OnChoice(int idx)
    {
        if (currentNode == null) return;
        if (idx < 0 || idx >= currentNode.choices.Count) return;
        var c = currentNode.choices[idx];
        if (c.rewardGold > 0) { var econ = FindObjectOfType<EconomySystem>(); if (econ != null) econ.AddGold(c.rewardGold); }
        if (!string.IsNullOrEmpty(c.consequence)) { /* handle consequences via StoryEventManager */ }
        if (!string.IsNullOrEmpty(c.nextNodeId)) StartDialogue(c.nextNodeId);
        else HideDialog();
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
            Debug.LogWarning("UnifiedDialogueSystem: Canvas not found.");
            return;
        }

        dialogRoot = new GameObject("DialogRoot_Unified");
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
