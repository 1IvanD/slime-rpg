using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public string nodeId;
    public string npcName;
    public string dialogueText;
    public List<DialogueChoice> choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public string nextNodeId;
    public int rewardGold;
    public string consequence;
}

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase Instance { get; private set; }

    private Dictionary<string, DialogueNode> dialogueNodes = new Dictionary<string, DialogueNode>();
    private Dictionary<string, int> npcReputation = new Dictionary<string, int>();
    private DialogueNode currentDialogue;

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
        // Диалог с Риммуру
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

        // Диалог с торговцем
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

        // Инициализация репутации
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

    public void StartDialogue(string nodeId)
    {
        if (dialogueNodes.TryGetValue(nodeId, out DialogueNode node))
        {
            currentDialogue = node;
            Debug.Log($"🗣️ {node.npcName}: {node.dialogueText}");
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentDialogue != null && choiceIndex < currentDialogue.choices.Count)
        {
            DialogueChoice choice = currentDialogue.choices[choiceIndex];
            Debug.Log($"💬 Ты выбрал: {choice.choiceText}");

            if (choice.rewardGold > 0)
            {
                // EconomySystem may not always exist in prototyping; check
                var econ = FindObjectOfType<EconomySystem>();
                if (econ != null) econ.AddGold(choice.rewardGold);
            }

            if (!string.IsNullOrEmpty(choice.nextNodeId))
            {
                StartDialogue(choice.nextNodeId);
            }
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
}
