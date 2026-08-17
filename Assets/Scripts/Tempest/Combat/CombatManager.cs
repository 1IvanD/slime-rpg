using UnityEngine;
using System.Collections.Generic;

namespace Tempest.Combat
{
    /// <summary>
    /// Core combat system for player, enemies, and NPCs.
    /// Handles attack resolution, damage, XP, leveling, loot, animations, and VFX.
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[CombatManager] Initialized");
        }

        #region Combat Resolution

        /// <summary>
        /// Execute an attack from attacker to target.
        /// Calculates hit chance, damage, applies effects, triggers animations/VFX.
        /// </summary>
        public CombatResult Attack(CombatEntity attacker, CombatEntity target)
        {
            CombatResult result = new CombatResult();

            if (attacker == null || target == null)
            {
                Debug.LogError("[CombatManager] Attack: invalid attacker or target");
                return result;
            }

            // Calculate hit chance (based on attacker accuracy vs target dodge)
            float hitChance = CalculateHitChance(attacker, target);
            bool isHit = Random.value < hitChance;

            result.isHit = isHit;
            result.attacker = attacker;
            result.target = target;

            if (!isHit)
            {
                Debug.Log($"[Combat] {attacker.entityName} missed {target.entityName}!");
                PlayMissAnimation(attacker, target);
                return result;
            }

            // Calculate damage
            float baseDamage = attacker.stats.attack;
            float damageMultiplier = Random.Range(0.8f, 1.2f); // variance
            float finalDamage = baseDamage * damageMultiplier;

            // Apply defense reduction
            float defense = target.stats.defense;
            float damageReduction = defense * 0.1f; // 10% per defense point
            finalDamage = Mathf.Max(1, finalDamage - damageReduction);

            result.damageDealt = (int)finalDamage;

            // Apply damage to target
            target.TakeDamage(result.damageDealt);

            Debug.Log($"[Combat] {attacker.entityName} hits {target.entityName} for {result.damageDealt} damage!");

            // Play attack animation and VFX
            PlayAttackAnimation(attacker);
            PlayAttackVFX(attacker, target, finalDamage);

            // Play hit/damage animation on target
            PlayHitAnimation(target);
            PlayDamageVFX(target, result.damageDealt);

            // Check if target is defeated
            if (target.stats.health <= 0)
            {
                result.targetDefeated = true;
                HandleDefeat(attacker, target);
            }

            return result;
        }

        /// <summary>
        /// Calculate hit chance based on accuracy and evasion stats.
        /// </summary>
        private float CalculateHitChance(CombatEntity attacker, CombatEntity target)
        {
            float baseHitChance = 0.85f; // 85% base
            float accuracyBonus = attacker.stats.accuracy * 0.01f; // +1% per accuracy
            float evasionPenalty = target.stats.evasion * 0.01f;   // -1% per evasion

            float finalHitChance = baseHitChance + accuracyBonus - evasionPenalty;
            return Mathf.Clamp01(finalHitChance);
        }

        #endregion

        #region Health & Damage

        /// <summary>
        /// Apply damage to entity and trigger damage effects.
        /// </summary>
        public void ApplyDamage(CombatEntity entity, int damage)
        {
            if (entity == null) return;

            entity.TakeDamage(damage);
            PlayDamageVFX(entity, damage);

            if (entity.stats.health <= 0)
            {
                HandleDefeat(null, entity);
            }
        }

        /// <summary>
        /// Restore health to entity.
        /// </summary>
        public void Heal(CombatEntity entity, int healAmount)
        {
            if (entity == null) return;

            int oldHealth = entity.stats.health;
            entity.stats.health = Mathf.Min(entity.stats.health + healAmount, entity.stats.maxHealth);
            int actualHealing = entity.stats.health - oldHealth;

            Debug.Log($"[Combat] {entity.entityName} healed for {actualHealing} HP");
            PlayHealVFX(entity, actualHealing);
        }

        #endregion

        #region Experience & Leveling

        /// <summary>
        /// Award XP to attacker when defeating an enemy.
        /// </summary>
        private void HandleDefeat(CombatEntity victor, CombatEntity defeated)
        {
            if (defeated == null) return;

            Debug.Log($"[Combat] {defeated.entityName} has been defeated!");
            PlayDeathAnimation(defeated);
            PlayDeathVFX(defeated);

            // Award XP to victor
            if (victor != null && victor.entityType == CombatEntityType.Player)
            {
                int xpReward = CalculateXPReward(defeated);
                GainXP(victor, xpReward);

                // Roll for loot
                LootTable lootTable = defeated.lootTable;
                if (lootTable != null)
                {
                    DropLoot(defeated.transform.position, lootTable);
                }
            }
        }

        /// <summary>
        /// Calculate XP reward based on defeated entity level and player level.
        /// </summary>
        private int CalculateXPReward(CombatEntity defeated)
        {
            int baseXP = defeated.stats.level * 100;

            // Level scaling: bonus for higher-level enemies, penalty for lower
            // (can be adjusted based on game balance)

            return baseXP;
        }

        /// <summary>
        /// Award XP to entity and handle leveling.
        /// </summary>
        public void GainXP(CombatEntity entity, int xpAmount)
        {
            if (entity == null) return;

            entity.stats.experience += xpAmount;
            Debug.Log($"[Combat] {entity.entityName} gained {xpAmount} XP (total: {entity.stats.experience})");

            // Check for level up
            while (entity.stats.experience >= GetXPForLevel(entity.stats.level + 1))
            {
                LevelUp(entity);
            }
        }

        /// <summary>
        /// Level up entity and increase stats.
        /// </summary>
        private void LevelUp(CombatEntity entity)
        {
            entity.stats.level++;
            entity.stats.maxHealth += 10;
            entity.stats.health = entity.stats.maxHealth; // Full heal on level up
            entity.stats.attack += 2;
            entity.stats.defense += 1;

            Debug.Log($"[Combat] {entity.entityName} leveled up to {entity.stats.level}!");
            PlayLevelUpVFX(entity);
        }

        /// <summary>
        /// Get total XP required to reach a specific level.
        /// </summary>
        private int GetXPForLevel(int level)
        {
            // Exponential XP curve: level 2 = 200, level 3 = 400, level 4 = 800, etc.
            return level * level * 100;
        }

        #endregion

        #region Loot System

        [System.Serializable]
        public class LootTable
        {
            public List<LootItem> items = new List<LootItem>();
            public int goldMin = 10;
            public int goldMax = 50;
        }

        [System.Serializable]
        public class LootItem
        {
            public string itemId;
            public float dropChance = 0.1f; // 10%
            public int minQuantity = 1;
            public int maxQuantity = 1;
        }

        /// <summary>
        /// Drop loot at specified position.
        /// TODO: Integrate with inventory/item system.
        /// </summary>
        private void DropLoot(Vector3 position, LootTable lootTable)
        {
            if (lootTable == null) return;

            // Roll for items
            foreach (var item in lootTable.items)
            {
                if (Random.value < item.dropChance)
                {
                    int quantity = Random.Range(item.minQuantity, item.maxQuantity + 1);
                    Debug.Log($"[Loot] Dropped {quantity}x {item.itemId} at {position}");
                    // TODO: Spawn item pickup in world
                }
            }

            // Roll for gold
            int goldDrop = Random.Range(lootTable.goldMin, lootTable.goldMax + 1);
            Debug.Log($"[Loot] Dropped {goldDrop} gold at {position}");
            // TODO: Spawn gold pickup in world
        }

        #endregion

        #region Animations

        /// <summary>
        /// Play attack animation on attacker.
        /// </summary>
        private void PlayAttackAnimation(CombatEntity attacker)
        {
            if (attacker.animator == null) return;

            attacker.animator.SetTrigger("Attack");
            Debug.Log($"[Animation] {attacker.entityName} attack");
        }

        /// <summary>
        /// Play hit/damage animation on target.
        /// </summary>
        private void PlayHitAnimation(CombatEntity target)
        {
            if (target.animator == null) return;

            target.animator.SetTrigger("Hit");
            Debug.Log($"[Animation] {target.entityName} hit");
        }

        /// <summary>
        /// Play death animation on entity.
        /// </summary>
        private void PlayDeathAnimation(CombatEntity entity)
        {
            if (entity.animator == null) return;

            entity.animator.SetTrigger("Death");
            entity.isDefeated = true;
            Debug.Log($"[Animation] {entity.entityName} death");
        }

        /// <summary>
        /// Play miss animation (dodge effect).
        /// </summary>
        private void PlayMissAnimation(CombatEntity attacker, CombatEntity target)
        {
            if (target.animator == null) return;

            target.animator.SetTrigger("Dodge");
            Debug.Log($"[Animation] {target.entityName} dodge");
        }

        #endregion

        #region VFX (Visual Effects)

        /// <summary>
        /// Play attack VFX (projectile, slash, etc).
        /// TODO: Instantiate VFX prefabs based on attack type.
        /// </summary>
        private void PlayAttackVFX(CombatEntity attacker, CombatEntity target, float damage)
        {
            Debug.Log($"[VFX] Attack VFX: {attacker.entityName} → {target.entityName} ({damage} dmg)");
            // TODO: Spawn attack VFX from attacker towards target
        }

        /// <summary>
        /// Play damage/hit VFX on target.
        /// </summary>
        private void PlayDamageVFX(CombatEntity target, int damage)
        {
            Debug.Log($"[VFX] Damage VFX on {target.entityName}: {damage} damage");
            // TODO: Spawn blood splat, impact effect, damage numbers
        }

        /// <summary>
        /// Play healing VFX on entity.
        /// </summary>
        private void PlayHealVFX(CombatEntity entity, int healAmount)
        {
            Debug.Log($"[VFX] Heal VFX on {entity.entityName}: +{healAmount} HP");
            // TODO: Spawn green heal particles
        }

        /// <summary>
        /// Play death VFX on entity.
        /// </summary>
        private void PlayDeathVFX(CombatEntity entity)
        {
            Debug.Log($"[VFX] Death VFX on {entity.entityName}");
            // TODO: Spawn death explosion/fade effect
        }

        /// <summary>
        /// Play level-up VFX on entity.
        /// </summary>
        private void PlayLevelUpVFX(CombatEntity entity)
        {
            Debug.Log($"[VFX] Level-up VFX on {entity.entityName}");
            // TODO: Spawn levelup stars/burst effect
        }

        #endregion
    }

    #region Data Structures

    /// <summary>
    /// Combat entity (Player, Enemy, NPC).
    /// Manages stats, health, animations, loot table.
    /// </summary>
    public class CombatEntity : MonoBehaviour
    {
        [System.Serializable]
        public class Stats
        {
            public int level = 1;
            public int health = 100;
            public int maxHealth = 100;
            public int attack = 10;
            public int defense = 5;
            public int accuracy = 85; // 0-100, hit chance %
            public int evasion = 10;  // 0-100, dodge chance %
            public int experience = 0;
        }

        public CombatEntityType entityType = CombatEntityType.Enemy;
        public string entityName = "Entity";
        public Stats stats = new Stats();
        public Animator animator;
        public CombatManager.LootTable lootTable;
        public bool isDefeated = false;

        /// <summary>
        /// Apply damage to this entity.
        /// </summary>
        public void TakeDamage(int damage)
        {
            stats.health -= damage;
            stats.health = Mathf.Max(0, stats.health);
            Debug.Log($"[Health] {entityName}: {stats.health}/{stats.maxHealth}");
        }

        /// <summary>
        /// Restore health to this entity.
        /// </summary>
        public void Heal(int amount)
        {
            stats.health += amount;
            stats.health = Mathf.Min(stats.health, stats.maxHealth);
        }

        /// <summary>
        /// Check if entity is alive.
        /// </summary>
        public bool IsAlive()
        {
            return stats.health > 0 && !isDefeated;
        }
    }

    /// <summary>
    /// Result of a combat action (attack).
    /// </summary>
    public class CombatResult
    {
        public CombatEntity attacker;
        public CombatEntity target;
        public bool isHit = false;
        public int damageDealt = 0;
        public bool targetDefeated = false;
    }

    /// <summary>
    /// Entity type in combat.
    /// </summary>
    public enum CombatEntityType
    {
        Player,
        Enemy,
        NPC
    }

    #endregion
}
