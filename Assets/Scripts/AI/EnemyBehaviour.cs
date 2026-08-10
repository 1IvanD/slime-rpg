using UnityEngine;

[DisallowMultipleComponent]
public class EnemyBehaviour : MonoBehaviour
{
    public float maxHealth = 20f;
    public float currentHealth = 20f;
    public float damage = 5f;
    public float xpReward = 10f;
    public float aggroRange = 6f;
    public float patrolSpeed = 2f;

    private Transform player;
    private Vector3 startPos;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<Player>()?.transform;
        startPos = transform.position;
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;
        float d = Vector3.Distance(transform.position, player.position);
        if (d < aggroRange)
        {
            // chase
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * patrolSpeed * Time.deltaTime;
        }
        else
        {
            // simple idle/patrol around startPos
            transform.position = Vector3.MoveTowards(transform.position, startPos + Vector3.right * Mathf.Sin(Time.time) * 1f, patrolSpeed * 0.5f * Time.deltaTime);
        }
    }

    public void ApplyDamage(float amount, GameObject attacker = null)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0f) Die(attacker);
    }

    private void Die(GameObject killer)
    {
        isDead = true;
        // award xp to player if killer is player
        if (killer != null)
        {
            var p = killer.GetComponent<Player>();
            if (p != null) p.AddExperience(xpReward);
        }
        // spawn loot via LootSpawner
        LootSpawner.Instance?.SpawnLoot(transform.position);
        UIController.GetInstance()?.ShowNotification($"Монстр побеждён: {gameObject.name}");
        Destroy(gameObject);
    }
}
