using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public DialogueUIController uiController;

    private DialogueTreeSO currentTree;
    private DialogueNodeSO currentNode;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // auto-find UI controller if not assigned
        if (uiController == null)
        {
            uiController = FindObjectOfType<DialogueUIController>();
        }
    }

    public void StartDialogue(DialogueTreeSO tree, string startNodeId = null)
    {
        if (tree == null) return;
        currentTree = tree;
        currentNode = startNodeId != null ? tree.GetNodeById(startNodeId) : (tree.nodes.Length > 0 ? tree.nodes[0] : null);
        if (currentNode == null)
        {
            Debug.LogWarning("DialogueManager: no start node found");
            return;
        }
        EnterNode(currentNode);
    }

    private bool CheckRequirements(DialogueNodeSO node)
    {
        if (node == null) return false;
        // check items
        if (node.requiredItemIds != null && node.requiredItemIds.Length > 0)
        {
            foreach (var id in node.requiredItemIds)
            {
                // InventorySystem API: try to see if item exists (we only have Add/Remove/Use/Sell) - here we try a Remove with 0? Not available.
                // instead, we can check by a simple internal query API — if not present, assume false
                // For now assume InventorySystem has HasItem method; if not, skip this check with a warning
                var inv = InventorySystem.Instance;
                if (inv == null)
                {
                    Debug.LogWarning("DialogueManager: InventorySystem not found — skipping item requirement checks.");
                    break;
                }
                // try reflection for HasItem
                var mi = inv.GetType().GetMethod("HasItem");
                if (mi != null)
                {
                    bool has = (bool)mi.Invoke(inv, new object[] { id });
                    if (!has) return false;
                }
                else
                {
                    // no HasItem available — skip
                    break;
                }
            }
        }

        // check quests
        if (node.requiredQuestIds != null && node.requiredQuestIds.Length > 0)
        {
            var qm = QuestManager.Instance;
            if (qm == null)
            {
                Debug.LogWarning("DialogueManager: QuestManager not found — skipping quest requirement checks.");
            }
            else
            {
                foreach (var q in node.requiredQuestIds)
                {
                    if (!qm.HasQuest(q)) return false;
                }
            }
        }
        return true;
    }

    private void EnterNode(DialogueNodeSO node)
    {
        currentNode = node;
        ExecuteEffects(node.onEnterEffects);
        if (uiController != null)
        {
            uiController.ShowNode(node);
        }
    }

    private void ExecuteEffects(DialogueEffect[] effects)
    {
        if (effects == null || effects.Length == 0) return;
        foreach (var e in effects)
        {
            switch (e.type)
            {
                case DialogueEffect.EffectType.GiveItem:
                    if (InventorySystem.Instance != null)
                        InventorySystem.Instance.AddItem(e.paramId, e.paramId, InventorySystem.ItemRarity.Common, InventorySystem.ItemCategory.Resource, 0.1f, e.amount, "Given from dialog", 0);
                    else Debug.LogWarning("DialogueManager: InventorySystem missing — cannot give item.");
                    break;
                case DialogueEffect.EffectType.RemoveItem:
                    if (InventorySystem.Instance != null)
                        InventorySystem.Instance.RemoveItem(e.paramId, e.amount);
                    break;
                case DialogueEffect.EffectType.StartQuest:
                    QuestManager.Instance?.AddQuest(e.paramId, e.paramId);
                    break;
                case DialogueEffect.EffectType.CompleteQuest:
                    QuestManager.Instance?.CompleteQuest(e.paramId);
                    break;
                case DialogueEffect.EffectType.CustomEvent:
                    // raise a custom event — for modders
                    Debug.Log($"DialogueManager: CustomEvent {e.paramId}");
                    break;
            }
        }
    }

    public void ChooseChoice(DialogueChoice choice)
    {
        if (!string.IsNullOrEmpty(choice.startQuestId))
            QuestManager.Instance?.AddQuest(choice.startQuestId, choice.startQuestId);

        if (choice.grantItem && !string.IsNullOrEmpty(choice.grantItemId))
        {
            InventorySystem.Instance?.AddItem(choice.grantItemId, choice.grantItemId, InventorySystem.ItemRarity.Common, InventorySystem.ItemCategory.Resource, 0.1f, choice.grantItemAmount, "Given from choice", 0);
        }

        var next = currentTree.GetNodeById(choice.targetNodeId);
        if (next != null)
            EnterNode(next);
        else
            EndDialogue();
    }

    public void EndDialogue()
    {
        currentTree = null;
        currentNode = null;
        uiController?.Hide();
    }
}
