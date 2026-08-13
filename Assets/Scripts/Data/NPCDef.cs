using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Data/NPC", fileName = "NPCDef")]
public class NPCDef : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public string faction;
    public string role; // e.g., Merchant, Knight, Companion, DemonLord, Villager

    [Header("Location")]
    public string homeNodeId; // MapNode id where NPC is usually located
    public string sceneName; // detailed scene name if applicable

    public bool important = false;
    public Sprite portrait;

    [Header("Gameplay")]
    public int level = 1;
    public string[] tags; // e.g., goblin, dwarf, demon, tempester

    [Header("Affinity / Sympathy")]
    [Range(0,100)]
    [Tooltip("Affinity (sympathy) percent toward the player. 0 = neutral/random passerby, 25 = subordinate, 50 = friendly, 100 = loves the player.")]
    public int affinity = 0;
}
