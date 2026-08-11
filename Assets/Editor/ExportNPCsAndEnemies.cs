#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public static class ExportNPCsAndEnemies
{
    [MenuItem("Tools/Tempest/Export NPCs and Enemies CSV")] 
    public static void Export()
    {
        string outDir = "Assets/Data";
        if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
        string outPath = Path.Combine(outDir, "NPCs_Enemies.csv");

        var sb = new StringBuilder();
        sb.AppendLine("type,id,displayName,homeNodeId,sceneName,level,troopCount,important");

        var npcs = Resources.LoadAll<NPCDef>("NPCs");
        foreach (var n in npcs)
        {
            sb.AppendLine($"NPC,{n.id},{Escape(n.displayName)},{n.homeNodeId},{n.sceneName},, ,{n.important}");
        }

        var enemies = Resources.LoadAll<EnemyDef>("Enemies");
        foreach (var e in enemies)
        {
            sb.AppendLine($"ENEMY,{e.id},{Escape(e.displayName)},{e.homeNodeId},{e.sceneName},{e.level},{e.troopCount},{e.important}");
        }

        File.WriteAllText(outPath, sb.ToString());
        AssetDatabase.ImportAsset(outPath);
        Debug.Log($"Exported NPC/Enemy CSV to {outPath}");
    }

    private static string Escape(string s)
    {
        if (s == null) return "";
        return s.Replace(",", " ").Replace('\n', ' ');
    }
}
#endif
