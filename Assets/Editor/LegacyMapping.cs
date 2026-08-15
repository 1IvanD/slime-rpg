#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Utility mapping legacy -> canonical for manual/assisted fixes.
public static class LegacyMapping
{
    // Map legacy class names to canonical types (by name). This is used by the AutoReplace tool (not enabled by default).
    public static readonly Dictionary<string, string> Map = new Dictionary<string, string>
    {
        { "QuestManager_Legacy", "QuestManager" },
        { "WorldMapUI_Legacy", "WorldMapUI" },
        { "SettlementManager_Legacy", "SettlementManager" },
        { "EnemySpawner_Legacy", "EnemySpawner" },
        { "WarManager_Legacy", "WarManager" }
    };
}
#endif
