#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CombatSetupUtility
{
    private const string folder = "Assets/Data/Enemies";

    [MenuItem("Tools/Tempest/Generate Enemy Prefab Placeholder")]
    public static void GenerateEnemyPrefabPlaceholder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Data", "Enemies");

        // create a simple GameObject
        var go = new GameObject("Enemy_Placeholder");
        var stats = go.AddComponent<EnemyStats>();
        stats.displayName = "Goblin Placeholder";
        stats.maxHealth = 50f;
        stats.attackPower = 8f;
        stats.xpReward = 5;

        var ai = go.AddComponent<EnemyAI>();
        ai.moveSpeed = 1.8f;
        ai.aggroRange = 5f;
        ai.attackRange = 1.2f;

        // add a simple visual placeholder
        var sr = go.AddComponent<SpriteRenderer>();
        // leave sprite null — user can assign art later

        string path = folder + "/Enemy_Goblin_Placeholder.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = prefab;
        Debug.Log("Generated Enemy prefab placeholder at " + path + ". Assign sprites/animations and use it as enemy prefab in EnemySpawner or events.");
    }
}
#endif
