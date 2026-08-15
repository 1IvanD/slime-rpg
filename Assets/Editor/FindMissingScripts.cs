#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class FindMissingScripts
{
    [MenuItem("Tools/Tempest/Find Missing Scripts (Scenes & Prefabs)")]
    public static void FindMissing()
    {
        var report = new List<string>();
        int total = 0;

        // Search open scenes and all scenes in Assets/Scenes
        var scenePaths = new List<string>();
        for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            var sp = EditorSceneManager.GetSceneByBuildIndex(i).path;
            if (!string.IsNullOrEmpty(sp)) scenePaths.Add(sp);
        }

        // Also include all scenes found under Assets/Scenes
        var guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        foreach (var g in guids)
        {
            var p = AssetDatabase.GUIDToAssetPath(g);
            if (!scenePaths.Contains(p)) scenePaths.Add(p);
        }

        // Process scenes
        foreach (var scenePath in scenePaths.Distinct())
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                FindMissingInGameObject(root, scenePath, ref total, report);
            }
        }

        // Process prefabs under Assets
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (var pg in prefabGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(pg);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            FindMissingInPrefab(prefab, path, ref total, report);
        }

        // Write report
        string outPath = "Assets/missing_scripts_report.txt";
        File.WriteAllLines(outPath, report);
        AssetDatabase.Refresh();

        Debug.Log($"FindMissingScripts: found {total} objects with missing scripts. Report written to {outPath}");
        if (total > 0)
        {
            EditorUtility.RevealInFinder(Path.GetFullPath(outPath));
        }
    }

    private static void FindMissingInPrefab(GameObject prefab, string prefabPath, ref int total, List<string> report)
    {
        var gos = prefab.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject).ToArray();
        foreach (var go in gos)
        {
            var comps = go.GetComponents<Component>();
            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    total++;
                    string path = GetGameObjectPath(go.transform);
                    string line = $"Prefab: {prefabPath} | GameObject: {path} | Missing component at index {i}";
                    report.Add(line);
                }
            }
        }
    }

    private static void FindMissingInGameObject(GameObject go, string scenePath, ref int total, List<string> report)
    {
        var gos = go.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject).ToArray();
        foreach (var child in gos)
        {
            var comps = child.GetComponents<Component>();
            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    total++;
                    string path = GetGameObjectPath(child.transform);
                    string line = $"Scene: {scenePath} | GameObject: {path} | Missing component at index {i}";
                    report.Add(line);
                }
            }
        }
    }

    private static string GetGameObjectPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
#endif
