using System;
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
            var player = killer.GetComponent<PlayerExperience>();
            if (player != null)
            {
                player.AddXP(dead.xpReward);
            }
        }

        // drop loot (placeholder: log drops)
        if (dead.lootTable != null)
        {
            foreach (var e in dead.lootTable.entries)
            {
                if (Random.value <= e.chance)
                {
                    int qty = Random.Range(e.minAmount, e.maxAmount + 1);
                    Debug.Log($"Dropped {qty}x {e.itemId}");
                    // TODO: spawn physical item in world / add to container
                }
            }
        }

        // destroy gameobject
        Destroy(dead.gameObject);
    }
}
