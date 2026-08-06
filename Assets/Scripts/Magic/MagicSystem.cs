using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Spell
{
    public string spellId;
    public string spellName;
    public MagicSchool school;
    public float manaCost;
    public float damage;
    public string description;
}

public enum MagicSchool
{
    Fire,
    Frost,
    Lightning,
    Dark,
    Light
}

public class MagicSystem : MonoBehaviour
{
    public static MagicSystem Instance { get; private set; }

    private Dictionary<MagicSchool, List<Spell>> spellsBySchool = new Dictionary<MagicSchool, List<Spell>>();
    private List<Spell> learnedSpells = new List<Spell>();
    private Dictionary<MagicSchool, int> schoolLevel = new Dictionary<MagicSchool, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeSpells();
    }

    private void InitializeSpells()
    {
        // Инициализация каждой школы магии
        foreach (MagicSchool school in System.Enum.GetValues(typeof(MagicSchool)))
        {
            spellsBySchool[school] = new List<Spell>();
            schoolLevel[school] = 1;
        }

        // Огненная магия
        CreateSpell("fireball", "Огненный шар", MagicSchool.Fire, 30, 75, "Запускает огненный шар");
        CreateSpell("inferno", "Инферно", MagicSchool.Fire, 50, 120, "Вызывает инферно");

        // Ледяная магия
        CreateSpell("frostbolt", "Ледяной болт", MagicSchool.Frost, 25, 60, "Запускает ледяной болт");
        CreateSpell("blizzard", "Буран", MagicSchool.Frost, 45, 110, "Вызывает буран");

        // Электрическая магия
        CreateSpell("lightning", "Молния", MagicSchool.Lightning, 35, 85, "Запускает молнию");
        CreateSpell("chain_lightning", "Цепная молния", MagicSchool.Lightning, 55, 130, "Цепная молния поражает врагов");

        // Темная магия
        CreateSpell("shadow_bolt", "Теневой болт", MagicSchool.Dark, 30, 70, "Болт теневой энергии");
        CreateSpell("curse", "Проклятие", MagicSchool.Dark, 40, 50, "Проклинает врага");

        // Светлая магия
        CreateSpell("holy_light", "Святой свет", MagicSchool.Light, 25, 60, "Исцеляет и наносит урон");
        CreateSpell("divine_shield", "Божественный щит", MagicSchool.Light, 35, 0, "Защищает от урона");
    }

    private void CreateSpell(string id, string name, MagicSchool school, float manaCost, float damage, string desc)
    {
        Spell spell = new Spell
        {
            spellId = id,
            spellName = name,
            school = school,
            manaCost = manaCost,
            damage = damage,
            description = desc
        };

        spellsBySchool[school].Add(spell);
    }

    public bool LearnSpell(Spell spell)
    {
        if (!learnedSpells.Contains(spell))
        {
            learnedSpells.Add(spell);
            Debug.Log($"Выучено заклинание: {spell.spellName}");
            return true;
        }
        return false;
    }

    public bool CastSpell(Spell spell)
    {
        AdvancedCombatSystem combatSystem = FindObjectOfType<AdvancedCombatSystem>();
        if (combatSystem.GetMana() >= spell.manaCost)
        {
            Debug.Log($"Заклинание произнесено: {spell.spellName}! Урон: {spell.damage}");
            return true;
        }
        Debug.Log("Недостаточно маны!");
        return false;
    }

    public void UpgradeSchool(MagicSchool school)
    {
        if (schoolLevel.TryGetValue(school, out int level))
        {
            schoolLevel[school]++;
            Debug.Log($"Школа {school} улучшена до уровня {schoolLevel[school]}");
        }
    }

    public List<Spell> GetLearnedSpells() => learnedSpells;
    public List<Spell> GetSchoolSpells(MagicSchool school) => spellsBySchool.TryGetValue(school, out var spells) ? spells : null;
    public int GetSchoolLevel(MagicSchool school) => schoolLevel.TryGetValue(school, out var level) ? level : 0;
}
