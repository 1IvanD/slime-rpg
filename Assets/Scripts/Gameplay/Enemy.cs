using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string enemyType = "Generic";
    public float health = 20f;
    public float level = 1f;

    // Initialize enemy with type/level
    public virtual void Initialize(string type, int lvl)
    {
        enemyType = type;
        level = lvl;
        // adjust base stats by level if needed
        health = Mathf.Max(1f, 10f + lvl * 5f);
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    public virtual void Die()
    {
        UIController.GetInstance()?.ShowNotification($"Враг повержен: {enemyType}");
        Destroy(gameObject);
    }
}
