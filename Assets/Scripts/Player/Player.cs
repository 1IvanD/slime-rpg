using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats = new PlayerStats();

    public PlayerStats GetStats()
    {
        return stats;
    }

    private void Start()
    {
        // For testing: ensure name exists
        gameObject.name = "Player";
    }

    // Example public API used by other systems
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
}
