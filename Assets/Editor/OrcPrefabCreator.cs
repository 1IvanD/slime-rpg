#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class OrcPrefabCreator
{
    [MenuItem("Tools/Tempest/Create Placeholder Prefabs (Orc/Enemy/NPC)")]
    public static void CreatePrefabs()
    {
        string resDir = "Assets/Resources/Prefabs";
        if (!Directory.Exists(resDir)) Directory.CreateDirectory(resDir);

        // Placeholder EnemyCluster
        var enemyGO = new GameObject("EnemyCluster_Placeholder");
        var cap = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cap.transform.SetParent(enemyGO.transform, false);
        cap.transform.localScale = Vector3.one;
        var mr = cap.GetComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Standard"));
        mr.sharedMaterial.color = Color.magenta;
        var label = CreateTextMesh("Enemy", enemyGO.transform);

        string enemyPath = Path.Combine(resDir, "EnemyCluster.prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(enemyGO, enemyPath, InteractionMode.UserAction);
        Object.DestroyImmediate(enemyGO);

        // OrcCluster prefab (dark red)
        var orcGO = new GameObject("OrcCluster_Placeholder");
        var cap2 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cap2.transform.SetParent(orcGO.transform, false);
        cap2.transform.localScale = Vector3.one * 1.4f;
        var mr2 = cap2.GetComponent<MeshRenderer>();
        mr2.sharedMaterial = new Material(Shader.Find("Standard"));
        mr2.sharedMaterial.color = new Color(0.45f, 0.08f, 0.08f); // dark red
        var label2 = CreateTextMesh("Orc Army", orcGO.transform);

        string orcPath = Path.Combine(resDir, "OrcCluster.prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(orcGO, orcPath, InteractionMode.UserAction);
        Object.DestroyImmediate(orcGO);

        // Placeholder NPC prefab
        var npcGO = new GameObject("NPC_Placeholder");
        var cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cyl.transform.SetParent(npcGO.transform, false);
        cyl.transform.localScale = Vector3.one * 0.4f;
        var mr3 = cyl.GetComponent<MeshRenderer>();
        mr3.sharedMaterial = new Material(Shader.Find("Standard"));
        mr3.sharedMaterial.color = Color.cyan;
        var label3 = CreateTextMesh("NPC", npcGO.transform);

        string npcPath = Path.Combine(resDir, "PlaceholderNPC.prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(npcGO, npcPath, InteractionMode.UserAction);
        Object.DestroyImmediate(npcGO);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Created placeholder prefabs at {resDir}");
    }

    private static TextMesh CreateTextMesh(string text, Transform parent)
    {
        var go = new GameObject("Label");
        go.transform.SetParent(parent, false);
        go.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        var txt = go.AddComponent<TextMesh>();
        txt.text = text;
        txt.characterSize = 0.12f;
        txt.anchor = TextAnchor.MiddleCenter;
        txt.color = Color.white;
        return txt;
    }
}
#endif
