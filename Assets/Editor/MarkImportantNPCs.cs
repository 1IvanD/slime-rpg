#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class MarkImportantNPCs
{
    [MenuItem("Tools/Tempest/Mark Important NPCs (Resources/NPCs)")]
    public static void Mark()
    {
        string[] importantIds = new[] {
            // main characters and companions
            "benimaru", "shune", "shion", "soe", "hakuro",
            // companions and goblin leaders
            "kurobe", "rigurd", "rigur", "gobta", "geld", "gabiru", "soka", "ranga",
            // named humans / nobles / others
            "youm","kabal","eren","gido","fuze","mjollmile","garel","gazel_dwargo","vesta","king_edmarais","ellen","alice_rondo","kenia_misaki","ryota_sekimoto","chloe_ober","gale_gibson",
            // demons & antagonists
            "clayman","laplace","futman","mirida","mailan","miulan","beretta",
            // ancient / strong
            "milim","ramiris","leon_cromwell","luminos","hinata_sakaguchi","veldora","ifrit"
        };

        string dir = "Assets/Resources/NPCs";
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning("MarkImportantNPCs: Resources/NPCs directory not found. Run Generate Starter NPCs first.");
            return;
        }

        int marked = 0;
        foreach (var id in importantIds)
        {
            string path = Path.Combine(dir, id + ".asset");
            var npc = AssetDatabase.LoadAssetAtPath<NPCDef>(path);
            if (npc != null)
            {
                if (!npc.important)
                {
                    npc.important = true;
                    EditorUtility.SetDirty(npc);
                    marked++;
                }
            }
            else
            {
                // try case-insensitive search
                var guids = AssetDatabase.FindAssets(id + " t:ScriptableObject", new[] { dir });
                foreach (var g in guids)
                {
                    var p = AssetDatabase.GUIDToAssetPath(g);
                    var n = AssetDatabase.LoadAssetAtPath<NPCDef>(p);
                    if (n != null && n.id == id)
                    {
                        if (!n.important) { n.important = true; EditorUtility.SetDirty(n); marked++; }
                        break;
                    }
                }
            }
        }

        if (marked > 0) AssetDatabase.SaveAssets();
        Debug.Log($"MarkImportantNPCs: marked {marked} NPC assets as important (if they existed).");
    }
}
#endif
