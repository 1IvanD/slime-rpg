using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public float Health = 100f;
    public float MaxHealth = 100f;
    public int Level = 1;
    public float Experience = 0f;
    public float ExperienceThreshold = 100f;

    public float Attack = 10f;
    public float Defense = 5f;

    public int AbsorbedEnemies = 0;
    public int UniqueSkillsLearned = 0;

    public PlayerStats() { }

    public void TakeDamage(float amount)
    {
        Health = Mathf.Max(0, Health - amount);
    }

    public void Heal(float amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
    }

    public void AddExperience(float amount)
    {
        Experience += amount;
        while (Experience >= ExperienceThreshold)
        {
            Experience -= ExperienceThreshold;
            Level++;
            MaxHealth += 10f;
            Health = MaxHealth;
            ExperienceThreshold *= 1.2f;
        }
    }
}
