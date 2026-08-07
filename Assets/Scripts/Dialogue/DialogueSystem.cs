using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Unified DialogueSystem: stores dialogue data and renders UI dialogs.
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [Serializable]
    public class DialogueNode
    {
        public string nodeId;
        public string npcName;
        public string dialogueText;
        public List<DialogueChoice> choices;
    }

    [Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public string nextNodeId;
        public int rewardGold;
        public string consequence;
    }

    private Dictionary<string, DialogueNode> dialogueNodes = new Dictionary<string, DialogueNode>();
    private Dictionary<string, int> npcReputation = new Dictionary<string, int>();
    private DialogueNode currentDialogue;

    // UI fields
    private GameObject dialogRoot;
    private Text messageText;
    private List<Button> optionButtons = new List<Button>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeDialogues();
    }

    private void InitializeDialogues()
    {
        // Example dialogues (can be expanded or loaded from data files)
        CreateDialogue("rimuru_greeting", "Риммуру", "Привет, путник! Ты ищешь приключений?",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Да, дай мне квест!", nextNodeId = "rimuru_quest", rewardGold = 100, consequence = "quest_accepted" },
                new DialogueChoice { choiceText = "Просто проходил мимо", nextNodeId = "rimuru_goodbye", rewardGold = 0, consequence = "none" }
            });

        CreateDialogue("rimuru_quest", "Риммуру", "Отлично! Помоги мне собрать 5 редких трав для зелья.",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Я их принесу!", nextNodeId = "rimuru_thanks", rewardGold = 500, consequence = "quest_given" }
            });

        CreateDialogue("rimuru_thanks", "Риммуру", "Спасибо за помощь! Твоя репутация выросла.",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Рад помочь", nextNodeId = "rimuru_goodbye", rewardGold = 0, consequence = "none" }
            });

        CreateDialogue("rimuru_goodbye", "Риммуру", "До встречи, путник!",
            new List<DialogueChoice>());

        // Merchant
        CreateDialogue("merchant_greeting", "Торговец", "Добро пожаловать в мою лавку! Что тебя интересует?",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Покажи мне товары", nextNodeId = "merchant_shop", rewardGold = 0, consequence = "shop_open" },
                new DialogueChoice { choiceText = "Уходи отсюда", nextNodeId = "merchant_angry", rewardGold = 0, consequence = "reputation_down" }
            });

        CreateDialogue("merchant_shop", "Торговец", "Вот мой лучший товар! Покупай!",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Спасибо", nextNodeId = "merchant_goodbye", rewardGold = 0, consequence = "none" }
            });

        CreateDialogue("merchant_angry", "Торговец", "Ты груб! Не приходи больше!",
            new List<DialogueChoice>
            {
                new DialogueChoice { choiceText = "Извини!", nextNodeId = "merchant_goodbye", rewardGold = 0, consequence = "reputation_down" }
            });

        CreateDialogue("merchant_goodbye", "Торговец", "Приходи еще!",
            new List<DialogueChoice>());

        // Initialize reputation
        npcReputation["Риммуру"] = 50;
        npcReputation["Торговец"] = 50;
    }

    private void CreateDialogue(string nodeId, string npcName, string text, List<DialogueChoice> choices)
    {
        DialogueNode node = new DialogueNode
        {
            nodeId = nodeId,
            npcName = npcName,
            dialogueText = text,
            choices = choices
        };
        dialogueNodes[nodeId] = node;
    }

    // Data API
    public void StartDialogue(string nodeId)
    {
        if (dialogueNodes.TryGetValue(nodeId, out DialogueNode node))
        {
            currentDialogue = node;
            ShowDialogueNode(node);
            Debug.Log($"🗣️ {node.npcName}: {node.dialogueText}");
        }
        else Debug.LogWarning($"Dialogue node not found: {nodeId}");
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentDialogue != null && choiceIndex < currentDialogue.choices.Count)
        {
            DialogueChoice choice = currentDialogue.choices[choiceIndex];
            Debug.Log($"💬 Ты выбрал: {choice.choiceText}");

            if (choice.rewardGold > 0)
            {
                var economy = FindObjectOfType<EconomySystem>();
                if (economy != null) economy.AddGold(choice.rewardGold);
            }

            if (!string.IsNullOrEmpty(choice.nextNodeId)) StartDialogue(choice.nextNodeId);
        }
    }

    public void ModifyReputation(string npcName, int amount)
    {
        if (npcReputation.TryGetValue(npcName, out int rep))
        {
            npcReputation[npcName] = Mathf.Max(0, Mathf.Min(100, rep + amount));
            Debug.Log($"📊 Репутация {npcName}: {npcReputation[npcName]}/100");
        }
    }

    public DialogueNode GetCurrentDialogue() => currentDialogue;
    public int GetReputation(string npcName) => npcReputation.TryGetValue(npcName, out var rep) ? rep : 0;

    // UI API
    public void ShowDialog(string message, string[] options, Action<int> onChoose)
    {
        EnsureUI();
        if (messageText != null) messageText.text = message;

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

    private void ShowDialogueNode(DialogueNode node)
    {
        if (node == null) return;
        string[] opts = new string[node.choices != null ? node.choices.Count : 0];
        for (int i = 0; i < opts.Length; i++) opts[i] = node.choices[i].choiceText;
        ShowDialog($"{node.npcName}: {node.dialogueText}", opts, (idx) => { MakeChoice(idx); });
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
