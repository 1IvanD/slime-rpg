using UnityEngine;
using System.Collections.Generic;

public enum SkillType
{
    Fishing,
    Hunting,
    Mining,
    Crafting,
    Cooking
}

[System.Serializable]
public class SkillData
{
    public SkillType skillType;
    public int level;
    public float experience;
    public float experiencePerLevel;
}

public class LabourSkillSystem : MonoBehaviour
{
    public static LabourSkillSystem Instance { get; private set; }

    private Dictionary<SkillType, SkillData> skills = new Dictionary<SkillType, SkillData>();

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
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)))
        {
            skills[skillType] = new SkillData
            {
                skillType = skillType,
                level = 1,
                experience = 0,
                experiencePerLevel = 100
            };
        }
    }

    public void AddSkillExperience(SkillType skillType, float amount)
    {
        if (skills.TryGetValue(skillType, out SkillData skill))
        {
            skill.experience += amount;
            
            while (skill.experience >= skill.experiencePerLevel)
            {
                skill.level++;
                skill.experience -= skill.experiencePerLevel;
                skill.experiencePerLevel *= 1.1f;
                Debug.Log($"Навык улучшен! {skillType} достиг уровня {skill.level}");
            }
        }
    }

    public void PerformFishing()
    {
        if (Random.value > 0.5f)
        {
            InventorySystem.Instance.AddItem("fish_common", "Обычная рыба", ItemRarity.Common, 
                ItemCategory.Resource, 0.5f, 1, "Пойманная рыба", 50);
            AddSkillExperience(SkillType.Fishing, 10);
            Debug.Log("Поймана рыба!");
        }
    }

    public void PerformMining()
    {
        if (Random.value > 0.4f)
        {
            InventorySystem.Instance.AddItem("ore_iron", "Железная руда", ItemRarity.Common, 
                ItemCategory.Resource, 1f, 1, "Добытая руда", 100);
            AddSkillExperience(SkillType.Mining, 15);
            Debug.Log("Добыта руда!");
        }
    }

    public void PerformHunting()
    {
        if (Random.value > 0.3f)
        {
            InventorySystem.Instance.AddItem("meat_raw", "Сырое мясо", ItemRarity.Common, 
                ItemCategory.Resource, 0.8f, 1, "Добытое мясо", 75);
            AddSkillExperience(SkillType.Hunting, 20);
            Debug.Log("Охота успешна!");
        }
    }

    public void PerformCooking()
    {
        if (InventorySystem.Instance.RemoveItem("meat_raw", 1))
        {
            InventorySystem.Instance.AddItem("food_cooked", "Приготовленная еда", ItemRarity.Uncommon, 
                ItemCategory.Consumable, 0.5f, 1, "Восстанавливает 100 HP", 150);
            AddSkillExperience(SkillType.Cooking, 25);
            Debug.Log("Еда приготовлена!");
        }
    }

    public SkillData GetSkill(SkillType skillType) => skills.TryGetValue(skillType, out var skill) ? skill : null;
    public Dictionary<SkillType, SkillData> GetAllSkills() => skills;
}
