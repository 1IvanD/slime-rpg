using UnityEngine;

[System.Serializable]
public struct DialogueChoice
{
    public string text;
    public string targetNodeId; // id of node to jump to or empty for end
    public string requiredItemId; // optional requirement
    public string startQuestId; // optional: start quest when choose
    public bool grantItem;
    public string grantItemId;
    public int grantItemAmount;
}

[System.Serializable]
public struct DialogueEffect
{
    public enum EffectType { GiveItem, RemoveItem, StartQuest, CompleteQuest, CustomEvent }
    public EffectType type;
    public string paramId;
    public int amount;
}
