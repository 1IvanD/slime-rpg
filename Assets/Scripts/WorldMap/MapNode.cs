using UnityEngine;

public class MapNode : MonoBehaviour
{
    public string id;
    public string displayName;
    public string sceneName;
    [TextArea]
    public string description;

    public enum NodeType { Settlement, Dungeon, Landmark, Wilderness, Capital }
    public NodeType nodeType = NodeType.Wilderness;

    public Sprite icon;
}
