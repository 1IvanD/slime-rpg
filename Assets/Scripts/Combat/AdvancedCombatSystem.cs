using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AdvancedCombatSystem : MonoBehaviour
{
    private PlayerStats playerStats;
    private float lastAttackTime = 0f;
    private float attackCooldown = 0.5f;
    private int comboCounter = 0;
    private float comboResetTime = 2f;
    private float lastComboTime = 0f;
    private float currentMana = 100f;
    private float maxMana = 100f;

    [System.Serializable]
    public class CombatAbility
    {
        public string abilityName;
        public float manaCost;
        public float cooldown;
        public float damage;
        public string description;
        public bool isAvailable = true;
    }

    private Dictionary<string, CombatAbility> abilities = new Dictionary<string, CombatAbility>();

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            playerStats = player.GetStats();
        }
        InitializeAbilities();
    }

    private void InitializeAbilities()
    {
        // Базовые способности
        abilities["slash"] = new CombatAbility
        {
            abilityName = "Мощный удар",
            manaCost = 10,
            cooldown = 1f,
            damage = 50,
            description = "Мощный удар мечом"
        };

        abilities["fireball"] = new CombatAbility
        {
            abilityName = "Огненный шар",
            manaCost = 30,
            cooldown = 2f,
            damage = 75,
            description = "Запускает огненный шар"
        };

        abilities["frost"] = new CombatAbility
        {
            abilityName = "Ледяной удар",
            manaCost = 25,
            cooldown = 1.5f,
            damage = 60,
            description = "Замораживает врага"
        };
    }

    public void PerformCombo()
    {
        if (Time.time - lastComboTime > comboResetTime)
        {
            comboCounter = 0;
        }

        comboCounter++;
        lastComboTime = Time.time;
        float comboDamage = playerStats.Attack * comboCounter * 0.5f;
        Debug.Log($"Комбо х{comboCounter}! Урон: {comboDamage}");
    }

    public bool UseAbility(string abilityName)
    {
        if (!abilities.TryGetValue(abilityName, out CombatAbility ability))
            return false;

        if (currentMana < ability.manaCost)
        {
            Debug.Log("Недостаточно маны!");
            return false;
        }

        if (!ability.isAvailable)
        {
            Debug.Log("Способность на перезарядке!");
            return false;
        }

        currentMana -= ability.manaCost;
        ability.isAvailable = false;
        Debug.Log($"Использована способность: {ability.abilityName}! Урон: {ability.damage}");

        StartCoroutine(ResetAbilityCooldown(ability, ability.cooldown));
        return true;
    }

    private System.Collections.IEnumerator ResetAbilityCooldown(CombatAbility ability, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        ability.isAvailable = true;
    }

    private void Update()
    {
        // Регенерация маны
        if (currentMana < maxMana)
        {
            currentMana += Time.deltaTime * 10f;
        }
    }

    public float GetMana() => currentMana;
    public float GetMaxMana() => maxMana;
    public int GetComboCounter() => comboCounter;
    public Dictionary<string, CombatAbility> GetAbilities() => abilities;
}
