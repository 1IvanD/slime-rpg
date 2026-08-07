using UnityEngine;

public class SimpleEnemy : Enemy
{
    public float speed = 2f;
    public float chaseRange = 6f;
    public float attackRange = 1.2f;
    public float damage = 5f;

    private Transform player;
    private Vector3 pointA;
    private Vector3 pointB;
    private bool goingToB = true;

    private void Start()
    {
        player = FindObjectOfType<Player>()?.transform;
        pointA = transform.position;
        pointB = transform.position + transform.right * 3f;

        // If not initialized by factory, set default health from Enemy.Initialize
        if (health <= 0) health = 20f;
    }

    private void Update()
    {
        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < chaseRange)
        {
            // chase
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            if (dist < attackRange)
            {
                // attack
                player.GetComponent<Player>()?.Damage(damage * Time.deltaTime);
            }
        }
        else
        {
            // patrol
            Vector3 target = goingToB ? pointB : pointA;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.1f) goingToB = !goingToB;
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    public override void Die()
    {
        UIController.GetInstance()?.ShowNotification($"Монстр побеждён: {gameObject.name}");
        base.Die();
    }
}
