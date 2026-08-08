#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Editor utility: create basic enemy prefabs into Assets/Resources/Prefabs/Enemies
public static class EnemyPrefabCreator
{
    [MenuItem("Tools/Create Enemy Prefabs (Enemies/Orc/Ogre/Banimaru)")]
    public static void CreateEnemyPrefabs()
    {
        string basePath = "Assets/Resources/Prefabs/Enemies";
        if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Prefabs")) AssetDatabase.CreateFolder("Assets/Resources", "Prefabs");
        if (!AssetDatabase.IsValidFolder(basePath)) AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Enemies");

        CreateOgrePrefab(basePath + "/Ogre.prefab");
        CreateOgrePrefab(basePath + "/BanimaruBoss.prefab", "Banimaru", 400f, 8f);
        CreateOrcPrefab(basePath + "/Orc.prefab");

        AssetDatabase.Refresh();
        Debug.Log("Enemy prefabs created in Resources/Prefabs/Enemies");
    }

    private static void CreateOgrePrefab(string path, string type = "Ogre", float hp = 200f, float level = 5f)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = type;
        go.transform.localScale = Vector3.one * 2.5f;
        var ogre = go.AddComponent<Ogre>();
        ogre.enemyType = type;
        ogre.health = hp;
        ogre.level = level;
        var lbl = go.AddComponent<SimpleLabel>();
        lbl.SetLabel(type);
        if (go.GetComponent<Collider>() != null) go.GetComponent<Collider>().isTrigger = false;
        // Add Rigidbody for physics
        if (go.GetComponent<Rigidbody>() == null) go.AddComponent<Rigidbody>().isKinematic = true;

        PrefabUtility.SaveAsPrefabAsset(go, path);
        GameObject.DestroyImmediate(go);
    }

    private static void CreateOrcPrefab(string path)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "Orc";
        var se = go.AddComponent<SimpleEnemy>();
        se.health = 80f;
        se.speed = 2.5f;
        se.damage = 8f;
        var lbl = go.AddComponent<SimpleLabel>();
        lbl.SetLabel("Orc");
        if (go.GetComponent<Collider>() != null) go.GetComponent<Collider>().isTrigger = false;
        if (go.GetComponent<Rigidbody>() == null) go.AddComponent<Rigidbody>().isKinematic = true;

        PrefabUtility.SaveAsPrefabAsset(go, path);
        GameObject.DestroyImmediate(go);
    }
}
#endif
