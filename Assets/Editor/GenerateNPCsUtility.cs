#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class GenerateNPCsUtility
{
    [MenuItem("Tools/Tempest/Generate Starter NPCs (Resources/NPCs)")]
    public static void GenerateNPCs()
    {
        string dir = "Assets/Resources/NPCs";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var npcs = new (string id, string name, string faction, string role, string home)[ ]
        {
            ("youm","Юм","People","Villager","tempest"),
            ("kabal","Кабал","People","Merchant","tempest"),
            ("eren","Эрен","People","Knight","brumund"),
            ("gido","Гидо","People","Blacksmith","brumund"),
            ("fuze","Фузе","People","Scholar","farmas"),
            ("mjollmile","Мьёллмайл","People","Noble","ulbresia"),
            ("garel","Гарель","People","Trader","western_states"),
            ("gazel_dwargo","Газель Дворго","Dwarf","Artisan","dwarf_kingdom"),
            ("vesta","Веста","People","Priestess","holy_ruberium"),
            ("king_edmarais","Король Эдмарайс","Royalty","King","holy_ruberium"),
            ("ellen","Эллен","People","Adventurer","western_states"),
            ("alice_rondo","Алиса Рондо","People","Merchant","tempest"),
            ("kenia_misaki","Кения Мисаки","People","Hunter","jura_forest"),
            ("ryota_sekimoto","Рёта Сэкимото","People","Scholar","various_west"),
            ("chloe_ober","Хлоя Обер","People","Noble","eastern_empire"),
            ("gale_gibson","Гейл Гибсон","People","Captain","ulbresia"),

            // Tempest inhabitants / ogres
            ("tempest_ogre_1","Огр Темпеста A","Ogre","Warrior","tempest"),
            ("tempest_ogre_2","Огр Темпеста B","Ogre","Worker","tempest"),

            // Goblin group (later important)
            ("rigurd","Ригурд","Goblins","Leader","goblin_village"),
            ("rigur","Ригур","Goblins","Officer","goblin_village"),
            ("gobta","Гобта","Goblins","Soldier","goblin_village"),
            ("geld","Гельд","Goblins","Carpenter","goblin_village"),
            ("gabiru","Габиру","Goblins","Hunter","goblin_village"),
            ("soka","Сока","Goblins","Scout","goblin_village"),
            ("kurobe","Куробэ","Goblins","Shaman","goblin_village"),
            ("ranga","Ранга","Companions","SpiritWolf","tempest"),

            // Demons & antagonists
            ("clayman","Клейман","Demon","Antagonist","demon_domains"),
            ("laplace","Лаплас","Demon","Lieutenant","demon_domains"),
            ("futman","Футман","Demon","Commander","demon_domains"),
            ("mirida","Мирида","Demon","Officer","demon_domains"),
            ("mailan","Майлан","Demon","Officer","demon_domains"),
            ("miulan","Миулан","Demon","Officer","demon_domains"),
            ("beretta","Беретта","Demon","Subordinate","demon_domains"),

            // Powerful / ancient
            ("milim","Милим Нова","DemonLord","Empress","demon_domains"),
            ("ramiris","Рамирис","Ancient","Queen","various_west"),
            ("leon_cromwell","Леон Кромвель","Ancient","General","eastern_empire"),
            ("luminos","Люминос Валентайн","Church","HighPriest","holy_ruberium"),
            ("hinata_sakaguchi","Хината Сакагути","Church","Bishop","holy_ruberium"),
            ("veldora","Вельдора","Ancient","Dragon","veldora_cave"),
            ("ifrit","Ифрит","Ancient","FireSpirit","continent_ice"),
            ("traini","Трейни","Ancient","Sage","various_west"),
            ("troya","Трёя","Ancient","Guardian","western_states"),
            ("dryad","Дриада","Ancient","NatureSpirit","jura_forest"),
            ("beren","Берен","Ancient","Knight","brumund"),
            ("albis","Альбис","Ancient","Warrior","ulbresia"),
            ("supia","Супия","Mage","Varies","various_west"),
            ("fabio","Фобио","People","Noble","eastern_empire"),

            // Church related
            ("arno","Арно","Church","Paladin","holy_ruberium"),
            ("reihim","Рейхим","Church","Cleric","holy_ruberium"),
            ("ruminus","Руминус","Church","Sage","holy_ruberium"),

            // Misc groups
            ("goblin_village_inhabitant","Гоблин (житель)","Goblins","Villager","goblin_village"),
            ("dwarf_inhabitant","Дворф","Dwarf","Worker","dwarf_kingdom"),
            ("brumund_citizen","Житель Брумунда","People","Citizen","brumund"),
            ("shizu_student","Ученик Шизу","People","Student","various_west"),
            ("holy_knight","Рыцарь Святой Империи","Knight","Soldier","holy_ruberium"),
            ("orc_warrior","Орк","Orc","Warrior","barren_lands"),
            ("lizardman","Ящерочеловек","Lizard","Tribal","barren_lands")
        };

        List<string> created = new List<string>();

        foreach (var t in npcs)
        {
            string assetPath = Path.Combine(dir, t.id + ".asset");
            NPCDef asset = ScriptableObject.CreateInstance<NPCDef>();
            asset.id = t.id;
            asset.displayName = t.name;
            asset.description = t.name + " (Generated placeholder)";
            asset.faction = t.faction;
            asset.role = t.role;
            asset.homeNodeId = t.home;
            asset.sceneName = ""; // keep blank so user assigns scenes later
            asset.important = false;
            AssetDatabase.CreateAsset(asset, assetPath);
            created.Add(assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // write a simple placement json mapping (Assets/Data/NPCPlacement.json)
        string dataDir = "Assets/Data";
        if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
        string jsonPath = Path.Combine(dataDir, "NPCPlacement.json");

        var placements = new Dictionary<string, object>();
        foreach (var t in npcs)
        {
            placements[t.id] = new { id = t.id, displayName = t.name, homeNode = t.home };
        }
        string json = JsonUtility.ToJson(new SerializationWrapper(placements), true);
        File.WriteAllText(jsonPath, json);

        AssetDatabase.ImportAsset(jsonPath);

        Debug.Log($"Generated {created.Count} NPC assets under {dir} and placement file at {jsonPath}.");
    }

    // wrapper to allow serializing dictionary to JSON via JsonUtility
    private class SerializationWrapper
    {
        public List<Placement> list = new List<Placement>();
        public SerializationWrapper(Dictionary<string, object> dict)
        {
            foreach (var kv in dict)
            {
                var dyn = kv.Value as dynamic;
                list.Add(new Placement { id = dyn.id, displayName = dyn.displayName, homeNode = dyn.homeNode });
            }
        }
    }

    [System.Serializable]
    private class Placement
    {
        public string id;
        public string displayName;
        public string homeNode;
    }
}
#endif
