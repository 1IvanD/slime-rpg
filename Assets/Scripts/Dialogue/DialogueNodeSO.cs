using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Dialogue/DialogueNode", fileName = "DialogueNodeSO")]
public class DialogueNodeSO : ScriptableObject
{
    public string id;
    [TextArea] public string text;
    public string speakerName;
    public Sprite speakerIcon;

    [Tooltip("Requirements to show this node. Evaluated by DialogueManager. If empty, always true.")]
    public string[] requiredItemIds;

    [Tooltip("IDs of quests that must be active for this node to show")]
    public string[] requiredQuestIds;

    [Tooltip("IDs of nodes to which player can choose to go from this node")]
    public DialogueChoice[] choices = new DialogueChoice[0];

    [Tooltip("Effects executed when this node is entered (give item / start quest / complete quest)")]
    public DialogueEffect[] onEnterEffects = new DialogueEffect[0];
}
