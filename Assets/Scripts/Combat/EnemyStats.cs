using UnityEngine;

[DisallowMultipleComponent]
public class EnemyStats : MonoBehaviour
{
    [Header("Identity")]
    public string enemyId = "enemy_placeholder";
    public string displayName = "Enemy";
    public int level = 1;

    [Header("Combat")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth = 100f;
    public float attackPower = 10f;
    public float defense = 2f;

    [Header("Rewards")]
    public int xpReward = 10;
    public LootTableSO lootTable;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
    }
}
