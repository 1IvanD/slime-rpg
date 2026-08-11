using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private Dictionary<string, SkillSO> allSkills = new Dictionary<string, SkillSO>();
    private HashSet<string> learned = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllSkills();
    }

    private void LoadAllSkills()
    {
        allSkills.Clear();
        var arr = Resources.LoadAll<SkillSO>("Skills");
        foreach (var s in arr)
        {
            if (!string.IsNullOrEmpty(s.id)) allSkills[s.id] = s;
        }
        Debug.Log($"SkillManager: loaded {allSkills.Count} skills from Resources/Skills");
    }

    public bool LearnSkill(string skillId)
    {
        if (string.IsNullOrEmpty(skillId)) return false;
        if (!allSkills.ContainsKey(skillId))
        {
            Debug.LogWarning($"SkillManager: skill {skillId} not found");
            return false;
        }
        if (learned.Contains(skillId)) return false;
        learned.Add(skillId);
        UIController.GetInstance()?.ShowNotification($"Навык изучен: {allSkills[skillId].displayName}");
        Debug.Log($"SkillManager: learned {skillId}");
        return true;
    }

    public List<SkillSO> GetLearnedSkills()
    {
        var outList = new List<SkillSO>();
        foreach (var id in learned)
        {
            if (allSkills.TryGetValue(id, out var s)) outList.Add(s);
        }
        return outList;
    }

    public SkillSO GetSkill(string id) => allSkills.TryGetValue(id, out var s) ? s : null;

    public bool HasSkill(string id) => learned.Contains(id);
}
