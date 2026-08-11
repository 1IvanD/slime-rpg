#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class GenerateQuestsUtility
{
    [MenuItem("Tools/Tempest/Generate Starter Quests (Resources/Quests)")]
    public static void GenerateQuests()
    {
        string dir = "Assets/Resources/Quests";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        // We'll create quests based on the list provided. IDs are simple keys.
        void CreateQuest(string id, string name, string desc, string prereq, string[] objectives, string nodeId = "")
        {
            var q = ScriptableObject.CreateInstance<QuestDef>();
            q.id = id;
            q.displayName = name;
            q.description = desc;
            q.prerequisiteQuestId = prereq;
            q.associatedNodeId = nodeId;
            foreach (var o in objectives) q.objectives.Add(new QuestDef.Objective { description = o, completed = false });
            q.status = string.IsNullOrEmpty(prereq) ? QuestDef.QuestStatus.Active : QuestDef.QuestStatus.Locked;
            AssetDatabase.CreateAsset(q, Path.Combine(dir, id + ".asset"));
        }

        // 1. Veldora cave start quests
        CreateQuest("q_veldora_1","Пробуждение в темноте","Игрок появляется в пещере.", "", new string[]{"Осмотреться","Использовать Анализ","Найти магические кристаллы"}, "veldora_cave");
        CreateQuest("q_veldora_2","Голос из глубины","Игрок слышит голос Велдоры.", "q_veldora_1", new string[]{"Найти источник голоса","Проанализировать барьер"}, "veldora_cave");
        CreateQuest("q_veldora_3","Встреча с Велдорой","Поговорить с Велдорой.", "q_veldora_2", new string[]{"Поговорить с Велдорой","Узнать о заточении","Получить способность Великий Мудрец"}, "veldora_cave");
        CreateQuest("q_veldora_4","Договор дружбы","Дать имя Велдоре и эволюция.", "q_veldora_3", new string[]{"Дать Велдоре имя","Получить своё имя","Эволюция → Unique Slime"}, "veldora_cave");
        CreateQuest("q_veldora_5","Ослабление печати","Поглотить кристаллы.", "q_veldora_4", new string[]{"Поглотить магические кристаллы","Поглотить магическую энергию","Вернуться к Велдоре"}, "veldora_cave");
        CreateQuest("q_veldora_6","Исчезновение Велдоры","Найти следы и выйти.", "q_veldora_5", new string[]{"Найти следы магии","Выйти из пещеры"}, "veldora_cave");

        // 2. Goblins
        CreateQuest("q_goblin_7","Голодная деревня","Помочь деревне гоблинов.", "q_veldora_6", new string[]{"Поговорить со старейшиной","Узнать о проблемах деревни"}, "goblin_village");
        CreateQuest("q_goblin_8","Сбор еды","Собрать провизию для деревни.", "q_goblin_7", new string[]{"Собрать ягоды","Собрать грибы"}, "goblin_village");
        CreateQuest("q_goblin_9","Починка хижин","Помочь с ремонтом.", "q_goblin_8", new string[]{"Собрать дерево","Починить 3 хижины"}, "goblin_village");
        CreateQuest("q_goblin_10","Пропавший гоблин","Найти и спасти.", "q_goblin_9", new string[]{"Найти гоблина в лесу","Спасти его от монстров"}, "goblin_village");

        // 3. Wolves
        CreateQuest("q_wolf_11","Нападение волков","Защитить деревню.", "q_goblin_10", new string[]{"Защитить деревню","Победить волков"}, "jura_forest");
        CreateQuest("q_wolf_12","Союз с волками","Договориться с лидером.", "q_wolf_11", new string[]{"Поговорить с лидером","Дать ему имя","Волки становятся союзниками"}, "jura_forest");
        CreateQuest("q_wolf_13","Спасти волчонка","Спасти детёныша.", "q_wolf_12", new string[]{"Найти волчонка","Освободить его из ловушки"}, "jura_forest");

        // 4. Ogres
        CreateQuest("q_ogre_14","Таинственные следы","Найти источник следов.", "q_wolf_13", new string[]{"Проанализировать следы","Найти источник"}, "jura_forest");
        CreateQuest("q_ogre_15","Встреча с ограми","Установить контакт.", "q_ogre_14", new string[]{"Поговорить с ограми","Успокоить их","Узнать о 'демоне в лесу'"}, "jura_forest");
        CreateQuest("q_ogre_16","Доказать силу","Тренировочная битва.", "q_ogre_15", new string[]{"Победить огров в тренировочной битве","Получить уважение"}, "jura_forest");
        CreateQuest("q_ogre_17","Назначить роли ограм","Организация лагеря.", "q_ogre_16", new string[]{"Назначить танка","Назначить охотника","Назначить мага"}, "jura_forest");
        CreateQuest("q_ogre_18","Дом для огров","Постройка жилья.", "q_ogre_17", new string[]{"Построить хижину для огров","Улучшить поселение"}, "jura_forest");

        // 5. Orcs (main arc) - these are required before Falmuth
        CreateQuest("q_orc_19","Разведка лагеря орков","Подготовка к оркам.", "q_ogre_18", new string[]{"Найти лагерь","Проанализировать врагов"}, "barren_lands");
        CreateQuest("q_orc_20","Спасти пленников","Спасение гоблинов.", "q_orc_19", new string[]{"Освободить гоблинов","Вернуть их в Темпест"}, "barren_lands");
        CreateQuest("q_orc_21","Битва с орками","Ключевая битва.", "q_orc_20", new string[]{"Победить отряд орков","Уничтожить вождя"}, "barren_lands");

        // 6. Tempest building
        CreateQuest("q_tempest_22","Основание Темпеста","Постройте поселение.", "q_orc_21", new string[]{"Выбрать место","Построить первое здание"}, "tempest");
        CreateQuest("q_tempest_23","Роли жителей","Организация населения.", "q_tempest_22", new string[]{"Назначить рабочих","Назначить охрану"}, "tempest");
        CreateQuest("q_tempest_24","Сбор ресурсов","Добыча материалов.", "q_tempest_23", new string[]{"Дерево","Камень","Руда"}, "tempest");

        // 7. Falmuth (later; requires orc arc complete)
        CreateQuest("q_falmuth_25","Разведка границы Фалмоса","Подготовка к Фалмуту.", "q_orc_21", new string[]{"Разведать границу"}, "tempest");
        CreateQuest("q_falmuth_26","Спасти торговцев","Помочь торговцам.", "q_falmuth_25", new string[]{"Спасти торговцев"}, "tempest");
        CreateQuest("q_falmuth_27","Битва за Темпест","Крупная битва с Фальмутом.", "q_falmuth_26", new string[]{"Защитить Темпест","Победить силы Фальмута"}, "tempest");

        // 8. Milim
        CreateQuest("q_milim_28","Встретить Миллим","Встреча с демонической лордессой.", "q_falmuth_27", new string[]{"Встретить Миллим"}, "tempest");
        CreateQuest("q_milim_29","Успокоить Миллим","Покажите её Темпест.", "q_milim_28", new string[]{"Успокоить Миллим"}, "tempest");
        CreateQuest("q_milim_30","Показать Темпест","Покажите поселение.", "q_milim_29", new string[]{"Показать Темпест"}, "tempest");

        // 9. Empire late arc
        CreateQuest("q_empire_31","Разведка Империи","Сбор информации.", "q_milim_30", new string[]{"Разведать Империю"}, "eastern_empire");
        CreateQuest("q_empire_32","Спасти пленников","Миссия спасения.", "q_empire_31", new string[]{"Спасти пленников"}, "eastern_empire");
        CreateQuest("q_empire_33","Большая война","Финальная война.", "q_empire_32", new string[]{"Участвовать в большой войне"}, "eastern_empire");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"GenerateQuestsUtility: created starter quests under {dir}");
    }
}
#endif
