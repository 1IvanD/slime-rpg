#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class GenerateEnemiesUtility
{
    [MenuItem("Tools/Tempest/Generate Starter Enemies (Resources/Enemies)")]
    public static void GenerateEnemies()
    {
        string dir = "Assets/Resources/Enemies";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var enemies = new (string id, string name, string faction, string home, bool boss, int level, int troopCount)[]
        {
            // Jurassic Forest beasts
            ("wolf_pack","Волк (пак)","Beast","jura_forest", false, 1, 3),
            ("giant_serpent","Гигантская змея","Beast","jura_forest", true, 8, 1),
            ("giant_spider","Гигантский паук","Beast","jura_forest", false, 5, 2),
            ("forest_small_monster","Лесная тварь","Beast","jura_forest", false, 1, 4),

            // Goblins
            ("goblin_bandit","Гоблин-разбойник","Goblin","goblin_village", false, 2, 6),
            ("goblin_hostile","Враждебный гоблин","Goblin","goblin_village", false, 1, 8),

            // Ifrit and possessed Shizu
            ("ifrit","Ифрит","Elemental","continent_ice", true, 20, 1),
            ("shizu_possessed","Шизу (под влиянием Ифрита)","Humanoid","veldora_cave", true, 15, 1),

            // Orcs - representative defs (use troopCount for mass armies)
            ("ork_lord_geld","Гельд (Орк-лорд)","Orc","barren_lands", true, 18, 1),
            ("ork_general","Орк-генерал","Orc","barren_lands", false, 14, 20),
            ("ork_knight","Орк-рыцарь","Orc","barren_lands", false, 10, 200),
            ("ork_elite","Орк-элита","Orc","barren_lands", false, 12, 2000),
            ("ork_regular","Обычный орк","Orc","barren_lands", false, 4, 100000),

            // Charybdis and associates
            ("charybdis","Чарыбдис","Leviathan","western_states", true, 30, 1),
            ("megalodon_minion","Мегалодон (миньон Чарыбдиса)","Leviathan","western_states", false, 8, 6),

            // Clayman group and allies
            ("clayman","Клейман","Demon","demon_domains", true, 25, 1),
            ("laplace","Лаплас","Demon","demon_domains", false, 16, 4),
            ("tear","Тиар","Demon","demon_domains", false, 12, 8),
            ("footman","Футман","Demon","demon_domains", false, 6, 30),
            ("miulan","Миулан","Demon","demon_domains", false, 10, 6),

            // Falmuth / Falmut forces (army examples)
            ("falmuth_knight","Рыцарь Фальмута","Human","brumund", false, 12, 4000),
            ("falmuth_mage","Маг Фальмута","Human","brumund", false, 14, 200),

            // Church-related opponents
            ("hinata_sakaguchi","Хината Сакагути","Church","holy_ruberium", true, 22, 1),
            ("holy_knight_generic","Рыцарь церкви","Church","holy_ruberium", false, 10, 800),

            // Temporary / misc opponents
            ("supia","Супия","Human","various_west", false, 9, 1),
            ("albis","Альбис","Human","ulbresia", false, 11, 1),
            ("fabio_temp","Фобио (времен.)","Human","various_west", false, 13, 1)
        };

        int created = 0;
        foreach (var e in enemies)
        {
            string assetPath = Path.Combine(dir, e.id + ".asset");
            EnemyDef asset = ScriptableObject.CreateInstance<EnemyDef>();
            asset.id = e.id;
            asset.displayName = e.name;
            asset.description = e.name + " (Generated placeholder)";
            asset.faction = e.faction;
            asset.homeNodeId = e.home;
            asset.sceneName = "";
            asset.boss = e.boss;
            asset.level = e.level;
            asset.troopCount = e.troopCount;
            asset.health = Mathf.Max(5f, e.level * 10f);
            asset.damage = Mathf.Max(1f, e.level * 1.5f);
            AssetDatabase.CreateAsset(asset, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"GenerateEnemies: created {created} EnemyDef assets under {dir}.");
    }
}
#endif
