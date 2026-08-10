using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyDamage(GameObject attacker, EnemyStats victim, float amount)
    {
        if (victim == null) return;
        float mitigated = Mathf.Max(0f, amount - victim.defense);
        victim.currentHealth -= mitigated;
        Debug.Log($"{attacker?.name ?? "Unknown"} dealt {mitigated} to {victim.displayName} ({victim.currentHealth}/{victim.maxHealth})");

        if (victim.currentHealth <= 0f)
        {
            HandleDeath(attacker, victim);
        }
    }

    private void HandleDeath(GameObject killer, EnemyStats dead)
    {
        Debug.Log($"Enemy defeated: {dead.displayName}");
        // award XP
        if (killer != null)
        {
            var player = killer.GetComponent<Player>();
            if (player != null)
            {
                player.AddExperience(dead.xpReward);
            }
        }

        // drop loot (attempt to add to inventory or log)
        if (dead.lootTable != null)
        {
            foreach (var e in dead.lootTable.entries)
            {
                if (UnityEngine.Random.value <= e.chance)
                {
                    int qty = UnityEngine.Random.Range(e.minAmount, e.maxAmount + 1);
                    Debug.Log($"Dropped {qty}x {e.itemId}");
                    if (InventorySystem.Instance != null)
                    {
                        // try to add with default params if item definition unknown
                        InventorySystem.Instance.AddItem(e.itemId, e.itemId, ItemRarity.Common, ItemCategory.Resource, 0.1f, qty, "Loot", 0);
                    }
                    else
                    {
                        // TODO: spawn pickup prefab
                    }
                }
            }
        }

        // destroy gameobject
        Destroy(dead.gameObject);
    }
}
