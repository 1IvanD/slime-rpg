#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class GenerateCampaignEvents
{
    [MenuItem("Tools/Tempest/Generate Campaign Events (Resources/CampaignEvents)")]
    public static void Generate()
    {
        string dir = "Assets/Resources/CampaignEvents";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        // Orcs event
        var e1 = ScriptableObject.CreateInstance<WarEventSO>();
        e1.id = "evt_orcs";
        e1.displayName = "Орки наступают";
        e1.description = "Массированное наступление орков на западные границы; колонны собираются у границы.";
        e1.order = 0;
        e1.targetNodeId = "barren_lands";
        e1.participantFactions = new string[] { "Orc" };
        e1.forceWinner = false;
        e1.requiredQuestId = "";
        AssetDatabase.CreateAsset(e1, Path.Combine(dir, e1.id + ".asset"));

        // Falmuth event (requires orc arc completion)
        var e2 = ScriptableObject.CreateInstance<WarEventSO>();
        e2.id = "evt_falmuth";
        e2.displayName = "Фальмут наступает";
        e2.description = "Армии Фальмута двигаются на Темпест в крупной операции.";
        e2.order = 1;
        e2.targetNodeId = "tempest";
        e2.participantFactions = new string[] { "Falmuth", "Human", "Brumund" };
        e2.forceWinner = false;
        e2.requiredQuestId = "q_orc_21";
        AssetDatabase.CreateAsset(e2, Path.Combine(dir, e2.id + ".asset"));

        // Clayman event (occurs after Falmuth arc)
        var e3 = ScriptableObject.CreateInstance<WarEventSO>();
        e3.id = "evt_clayman";
        e3.displayName = "Клейман наступает";
        e3.description = "Организованное наступление сил, связанных с Клейманом.";
        e3.order = 2;
        e3.targetNodeId = "demon_domains";
        e3.participantFactions = new string[] { "Demon", "Clayman" };
        e3.forceWinner = false;
        e3.requiredQuestId = "q_falmuth_27";
        AssetDatabase.CreateAsset(e3, Path.Combine(dir, e3.id + ".asset"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"GenerateCampaignEvents: created 3 WarEventSO assets under {dir}");
    }
}
#endif
