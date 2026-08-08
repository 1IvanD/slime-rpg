using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private HashSet<string> unlockedSkills = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool Unlock(string skillId)
    {
        if (string.IsNullOrEmpty(skillId)) return false;
        if (unlockedSkills.Contains(skillId)) return false;
        unlockedSkills.Add(skillId);
        Debug.Log($"SkillManager: unlocked {skillId}");
        // Optionally trigger UI update or save
        return true;
    }

    public bool IsUnlocked(string skillId) => unlockedSkills.Contains(skillId);

    public IEnumerable<string> GetUnlockedSkills() => unlockedSkills;
}
