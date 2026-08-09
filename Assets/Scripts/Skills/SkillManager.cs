using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    // All known skills
    private Dictionary<string, SkillDefinition> skills = new Dictionary<string, SkillDefinition>();
    // Unlocked skill ids
    private HashSet<string> unlocked = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        // Core innate skills
        AddSkill(new SkillDefinition("Absorption", "Absorption", "Позволяет поглощать объекты и существ. Врожденная способность слизня.", true));
        AddSkill(new SkillDefinition("Dissolve", "Dissolve", "Позволяет растворять поглощённые материалы.", true));
        AddSkill(new SkillDefinition("SelfRegeneration", "Self-Regeneration", "Самовосстановление: регенерация здоровья с течением времени.", true));
        AddSkill(new SkillDefinition("Mimicry", "Mimicry", "Способность воспроизводить внешний вид поглощённых существ.", true));
        AddSkill(new SkillDefinition("UltraspeedRegeneration", "Ultraspeed Regeneration", "Сверхскоростная регенерация, объединение регенеративных навыков.", true));

        // From Veldora
        AddSkill(new SkillDefinition("MagicSense", "Magic Sense", "Чувство магии: позволяет обнаруживать магикулы, магические кристаллы и присутствие магической энергии.", true));
        AddSkill(new SkillDefinition("ThoughtCommunication", "Thought Communication", "Передача мыслей другим существам.", true));
        AddSkill(new SkillDefinition(SkillIDs.Telepathy, "Telepathy", "Телепатия: позволяет общаться мысленно с союзниками/слугами.", true));

        // From Dirvulfs
        AddSkill(new SkillDefinition("UltraSmell", "Ultra Smell", "Сверхнюх: позволяет обнаруживать существ по запаху.", true));
        AddSkill(new SkillDefinition("Coercion", "Coercion", "Запугивание: подавляет противников присутствием.", true));

        // Spider and threads
        AddSkill(new SkillDefinition("StickyThreadSkill", "Sticky Thread", "Навык/материал: липкая нить от чёрного паука.", true));
        AddSkill(new SkillDefinition("SteelThreadSkill", "Steel Thread", "Стальная нить от чёрного паука.", true));
        AddSkill(new SkillDefinition("StickySteelThreadSkill", "Sticky-Steel Thread", "Комбинированная липко-стальная нить.", true));

        // Water/Ice
        AddSkill(new SkillDefinition("HydraulicPropulsion", "Hydraulic Propulsion", "Водяная тяга: позволяет использовать воду для движения.", true));
        AddSkill(new SkillDefinition("WaterCurrentMotion", "Water Current Motion", "Движение при помощи потоков воды.", true));
        AddSkill(new SkillDefinition("WaterBlade", "Water Blade", "Водяной клинок: режущая атака из воды.", false));
        AddSkill(new SkillDefinition("WaterManipulation", "Water Manipulation", "Управление водой: контроль водных стихий.", false));
        AddSkill(new SkillDefinition("IcicleLance", "Icicle Lance", "Ледяная атака: Icicle Lance.", false));
        AddSkill(new SkillDefinition("IcicleShot", "Icicle Shot", "Стрельба льдинками.", false));

        // Fire / Ifrit
        AddSkill(new SkillDefinition("FlameManipulation", "Flame Manipulation", "Управление пламенем.", false));
        AddSkill(new SkillDefinition("FlameTransformation", "Flame Transformation", "Преобразование тела в огненную форму.", false));
        AddSkill(new SkillDefinition("ExplosiveFlames", "Explosive Flames", "Взрывное пламя.", false));
        AddSkill(new SkillDefinition("BlackFlame", "Black Flame", "Чёрное пламя — усиленная форма огня.", false));
        AddSkill(new SkillDefinition("BodyDouble", "Body Double", "Создание двойника тела.", false));
        AddSkill(new SkillDefinition("FlareCircle", "Flare Circle", "Магическая техника — Flare Circle.", false));

        // Electric
        AddSkill(new SkillDefinition("BlackLightning", "Black Lightning", "Чёрная молния: электрическая форма.", false));
        AddSkill(new SkillDefinition("BlackThunder", "Black Thunder", "Развитая форма чёрной молнии.", false));

        // Predation / Gluttony chain
        AddSkill(new SkillDefinition("Predator", "Predator", "Навык поглощения, анализ и копирование. (Satoru -> Rimuru)", true));
        AddSkill(new SkillDefinition("Starved", "Starved", "Способность голода, полученная от Орк-Лорда.", true));
        AddSkill(new SkillDefinition("Gluttony", "Gluttony", "Объединение Predator и Starved — мощная форма потребления.", true));

        // Sizu / Degenerate
        AddSkill(new SkillDefinition("Degenerate", "Degenerate", "Навык, полученный от Сидзу: Degenerate и его эффекты.", true));
        AddSkill(new SkillDefinition("UniversalShapeshift", "Universal Shapeshift", "Универсальная смена формы (Mimicry-based).", false));

        // Senses / other
        AddSkill(new SkillDefinition("PoisonBreath", "Poison Breath", "Ядовитое дыхание.", false));
        AddSkill(new SkillDefinition("ParalyzingBreath", "Paralyzing Breath", "Парализующее дыхание.", false));
        AddSkill(new SkillDefinition("BodyArmorSkill", "Body Armor", "Форма телесной брони от поглощения.", true));
        AddSkill(new SkillDefinition("UltraSoundWaves", "Ultra Sound Waves", "Ультразвуковые волны для обнаружения.", true));
        AddSkill(new SkillDefinition("Drain", "Drain", "Поглощение крови/ресурсов.", true));

        // Great Sage family
        AddSkill(new SkillDefinition("GreatSage", "Great Sage", "Великий Мудрец — система анализа и поддержки.", true));
        AddSkill(new SkillDefinition("ThoughtAcceleration", "Thought Acceleration", "Ускорение мыслительных процессов Great Sage.", true));
        AddSkill(new SkillDefinition("AnalyticalAppraisal", "Analytical Appraisal", "Анализ существ, предметов и навыков.", true));
        AddSkill(new SkillDefinition("ParallelCalculation", "Parallel Calculation", "Параллельные вычисления Great Sage.", true));
        AddSkill(new SkillDefinition("AllOfCreation", "All of Creation", "Широкая систематизация знаний.", true));
        AddSkill(new SkillDefinition("ChantAnnulment", "Chant Annulment", "Использование магии без произнесения заклинаний.", true));
        AddSkill(new SkillDefinition("AutoBattle", "Auto Battle", "Помощь в выборе оптимальных действий в бою.", true));

        // Barriers / defenses
        AddSkill(new SkillDefinition("RangedBarrier", "Ranged Barrier", "Дальний барьер.", true));
        AddSkill(new SkillDefinition("MultilayerBarrier", "Multilayer Barrier", "Многослойный барьер.", true));
        AddSkill(new SkillDefinition("PhysicalAttackResistance", "Physical Attack Resistance", "Сопротивление физическим атакам.", true));
        AddSkill(new SkillDefinition("ElectricityResistance", "Electricity Resistance", "Сопротивление электричеству.", true));
        AddSkill(new SkillDefinition("ParalysisResistance", "Paralysis Resistance", "Сопротивление параличу.", true));
        AddSkill(new SkillDefinition("FireResistance", "Fire Resistance", "Сопротивление огню.", true));
        AddSkill(new SkillDefinition("ColdResistance", "Cold Resistance", "Сопротивление холоду.", true));

        // Other combat
        AddSkill(new SkillDefinition("MagicAura", "Magic Aura", "Магическая аура.", true));
        AddSkill(new SkillDefinition("StrengthenBody", "Strengthen Body", "Усиление тела для повышения характеристик.", true));
        AddSkill(new SkillDefinition("HakiLike", "Haki", "Пара похожих на haki эффектов: повышенная проницательность и давление.", true));
        AddSkill(new SkillDefinition("AuraSword", "Aura Sword", "Комбинация магии и меча.", false));
        AddSkill(new SkillDefinition("Swordsmanship", "Swordsmanship", "Фехтовальные навыки, получаемые обучением.", false));

        // Summon/others
        AddSkill(new SkillDefinition("SummonDemon", "Summon Demon", "Призыв демона / суггестивные магические действия.", false));

        // Mark some defaults as unlocked from rebirth
        Unlock("GreatSage");
        Unlock("Predator");
        Unlock("Absorption");
        Unlock("Dissolve");
        Unlock("SelfRegeneration");
        Unlock("Mimicry");
        Unlock(SkillIDs.Telepathy);
        Unlock("ElectricityResistance");
        Unlock("ParalysisResistance");

        // additional early unlocked skills
        Unlock("MagicSense");
    }

    public void AddSkill(SkillDefinition def)
    {
        if (def == null || string.IsNullOrEmpty(def.id)) return;
        skills[def.id] = def;
    }

    public SkillDefinition GetSkill(string id)
    {
        if (skills.TryGetValue(id, out var s)) return s;
        return null;
    }

    public bool Unlock(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        if (!skills.ContainsKey(id))
        {
            Debug.LogWarning($"SkillManager: trying to unlock unknown skill {id}");
            // create a placeholder skill automatically
            AddSkill(new SkillDefinition(id, id, "(placeholder)", true));
        }
        if (unlocked.Contains(id)) return false;
        unlocked.Add(id);
        Debug.Log($"Skill unlocked: {id}");
        return true;
    }

    public bool Lock(string id)
    {
        if (unlocked.Contains(id)) { unlocked.Remove(id); return true; }
        return false;
    }

    public bool IsUnlocked(string id)
    {
        return unlocked.Contains(id);
    }

    public List<string> GetUnlockedSkills()
    {
        return new List<string>(unlocked);
    }

    public List<SkillDefinition> GetAllSkillDefinitions()
    {
        return new List<SkillDefinition>(skills.Values);
    }
}
