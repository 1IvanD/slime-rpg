using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Data/Skill", fileName = "SkillSO")]
public class SkillSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Learn requirements")]
    public int requiredLevel = 0;
    public float learnChance = 1f; // default 100%
}
