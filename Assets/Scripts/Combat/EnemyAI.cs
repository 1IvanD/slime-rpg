using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chasing, Attacking }

    [Header("AI Settings")]
    public State initialState = State.Patrol;
    public float moveSpeed = 2f;
    public float aggroRange = 6f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public bool loopPatrol = true;

    private int currentPatrolIndex = 0;
    private State state;
    private EnemyStats stats;
    private GameObject targetPlayer;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        state = initialState;
    }

    private void Update()
    {
        // simple detection of player
        if (targetPlayer == null)
        {
            var p = FindClosestPlayerInRange(aggroRange);
            if (p != null)
            {
                targetPlayer = p;
                state = State.Chasing;
            }
        }

        switch (state)
        {
            case State.Idle:
                // idle behaviour
                break;
            case State.Patrol:
                HandlePatrol();
                break;
            case State.Chasing:
                HandleChase();
                break;
            case State.Attacking:
                HandleAttack();
                break;
        }
    }

    private GameObject FindClosestPlayerInRange(float range)
    {
        // naive approach: find object tagged "Player"
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null) return null;
        float d = Vector3.Distance(transform.position, playerGO.transform.position);
        if (d <= range) return playerGO;
        return null;
    }

    private void HandlePatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        var target = patrolPoints[currentPatrolIndex];
        MoveTowards(target.position);
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                if (loopPatrol) currentPatrolIndex = 0;
                else { state = State.Idle; }
            }
        }
    }

    private void HandleChase()
    {
        if (targetPlayer == null) { state = State.Patrol; return; }
        float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (dist <= attackRange)
        {
            state = State.Attacking;
            return;
        }
        MoveTowards(targetPlayer.transform.position);
    }

    private void HandleAttack()
    {
        if (targetPlayer == null) { state = State.Patrol; return; }
        float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (dist > attackRange + 0.2f)
        {
            state = State.Chasing;
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            DoAttack();
        }
    }

    private void DoAttack()
    {
        if (targetPlayer == null) return;
        // Try to find a damage receiver on player
        var statsPlayer = targetPlayer.GetComponent<PlayerStats>();
        if (statsPlayer != null)
        {
            // send damage via CombatManager
            if (CombatManager.Instance != null)
            {
                var es = GetComponent<EnemyStats>();
                CombatManager.Instance.ApplyDamage(gameObject, es, es != null ? es.attackPower : 5f);
                // Note: this ApplyDamage expects attacker and EnemyStats victim – but for player we would need a different method.
                // For now just log
                Debug.Log($"{name} attacked player (placeholder) — implement player damage path.");
            }
        }
        else
        {
            Debug.Log($"{name} attacked (no player stats component found). Implement player damage reception.");
        }
    }

    private void MoveTowards(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        dir.Normalize();
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, dir, 0.2f);
    }
}
