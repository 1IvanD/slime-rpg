using UnityEngine;

public class Absorbable : MonoBehaviour
{
    public string resourceId = "hipokute"; // matches ItemsDatabase ids
    public int amount = 1;

    [Header("Optional skill reward")]
    public string skillId = ""; // id of SkillSO in Resources/Skills
    [Range(0f,1f)] public float skillChance = 0f;

    public void OnAbsorbed(PlayerAbilities by)
    {
        if (by == null) return;
        var stomach = by.GetComponent<StomachInventory>();
        if (stomach != null)
        {
            stomach.AddMaterial(resourceId, amount);
            UIController.GetInstance()?.ShowNotification($"Поглощено: {resourceId} x{amount}");
        }
        else
        {
            // fallback: add gold
            EconomySystem.Instance?.AddGold(10f * amount);
            UIController.GetInstance()?.ShowNotification($"Поглощено и конвертировано: {resourceId}");
        }

        // chance to learn a skill
        if (!string.IsNullOrEmpty(skillId) && Random.value <= skillChance)
        {
            if (SkillManager.Instance != null)
            {
                bool learned = SkillManager.Instance.LearnSkill(skillId);
                if (learned)
                {
                    // optional extra reward: increment unique skills count
                    var p = by.GetComponent<Player>();
                    if (p != null) p.stats.UniqueSkillsLearned += 1;
                }
            }
        }

        Destroy(gameObject);
    }
}
