using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillDefinition
{
    public string id;
    public string displayName;
    [TextArea]
    public string description;
    public bool isPassive = true;
    public int tier = 1;
    public List<string> prerequisites = new List<string>();

    public SkillDefinition() { }

    public SkillDefinition(string id, string displayName, string description = "", bool isPassive = true, int tier = 1)
    {
        this.id = id;
        this.displayName = displayName;
        this.description = description;
        this.isPassive = isPassive;
        this.tier = tier;
    }
}
