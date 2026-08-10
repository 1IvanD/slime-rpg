using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Dialogue/DialogueTree", fileName = "DialogueTreeSO")]
public class DialogueTreeSO : ScriptableObject
{
    public string id;
    public string characterName;
    public Sprite characterIcon;
    public DialogueNodeSO[] nodes = new DialogueNodeSO[0];

    public DialogueNodeSO GetNodeById(string id)
    {
        foreach (var n in nodes) if (n != null && n.id == id) return n;
        return null;
    }
}
