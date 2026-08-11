using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Data/Enemy", fileName = "EnemyDef")]
public class EnemyDef : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public string faction; // e.g., Goblin, Orc, Demon, Beast
    public string homeNodeId; // MapNode id for placement
    public string sceneName; // optional scene override

    public int level = 1;
    public float health = 10f;
    public float damage = 2f;
    public bool boss = false;
    public bool important = false;
}
