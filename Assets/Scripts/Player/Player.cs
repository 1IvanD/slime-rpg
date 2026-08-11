using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerStats stats = new PlayerStats();

    [Header("Inspector Settings")]
    public string displayName = "Player";
    public string race = "Unknown";
    public string difficulty = "Normal";

    [Header("Starting Attributes")]
    public float startingHealth = 100f;
    public float startingAttack = 10f;
    public float startingDefense = 5f;

    // new base attributes for extended stats
    public int startingStrength = 1;
    public int startingDefenseStat = 1;
    public int startingMagic = 1;
    public int startingSpeed = 1;
    public int startingIntelligence = 1;

    private void Awake()
    {
        // Initialize stats from inspector values
        stats.MaxHealth = startingHealth;
        stats.Health = startingHealth;
        stats.Attack = startingAttack;
        stats.Defense = startingDefense;

        // set extended base stats
        stats.Strength = startingStrength;
        stats.DefenseStat = startingDefenseStat;
        stats.Magic = startingMagic;
        stats.Speed = startingSpeed;
        stats.Intelligence = startingIntelligence;
    }

    public PlayerStats GetStats()
    {
        return stats;
    }

    private void Start()
    {
        gameObject.name = displayName;
    }

    public void Damage(float amount)
    {
        stats.TakeDamage(amount);
    }

    public void Heal(float amount)
    {
        stats.Heal(amount);
    }

    public void AddExperience(float amount)
    {
        stats.AddExperience(amount);
    }

    public void ApplyRaceAndDifficulty(GameManager.Race selectedRace, GameManager.Difficulty selectedDifficulty)
    {
        race = selectedRace.ToString();
        difficulty = selectedDifficulty.ToString();

        // Set display name and tweak stats for specific race (example)
        if (selectedRace == GameManager.Race.Slime)
        {
            displayName = "Rimuru";
            startingHealth = 150f;
            startingAttack = 15f;
            startingDefense = 8f;
        }
        else if (selectedRace == GameManager.Race.Human)
        {
            displayName = "Human Adventurer";
            startingHealth = 120f;
            startingAttack = 12f;
            startingDefense = 6f;
        }
        else if (selectedRace == GameManager.Race.Elf)
        {
            displayName = "Elf";
            startingHealth = 100f;
            startingAttack = 14f;
            startingDefense = 5f;
        }
        else if (selectedRace == GameManager.Race.Demon)
        {
            displayName = "Demon";
            startingHealth = 180f;
            startingAttack = 18f;
            startingDefense = 10f;
        }

        // Difficulty adjustments
        if (selectedDifficulty == GameManager.Difficulty.Easy)
        {
            startingHealth *= 1.2f;
            startingAttack *= 1.1f;
        }
        else if (selectedDifficulty == GameManager.Difficulty.Hard)
        {
            startingHealth *= 0.9f;
            startingAttack *= 0.9f;
        }

        // Apply to stats
        stats.MaxHealth = startingHealth;
        stats.Health = startingHealth;
        stats.Attack = startingAttack;
        stats.Defense = startingDefense;

        // Update GameObject name
        gameObject.name = displayName;
    }

    // Reset stats to base inspector values (used before applying equipment bonuses)
    public void ResetStatsToBase()
    {
        stats.MaxHealth = startingHealth;
        stats.Health = Mathf.Min(stats.Health, stats.MaxHealth);
        stats.Attack = startingAttack;
        stats.Defense = startingDefense;

        stats.Strength = startingStrength;
        stats.DefenseStat = startingDefenseStat;
        stats.Magic = startingMagic;
        stats.Speed = startingSpeed;
        stats.Intelligence = startingIntelligence;
    }
}
