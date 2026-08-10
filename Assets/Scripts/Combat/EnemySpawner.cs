using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Enemy prefabs to spawn (prefabs should contain EnemyStats/EnemyAI)")]
    public GameObject[] enemyPrefabs;

    [Tooltip("Number of enemies to spawn in this ambush")]
    public int spawnCount = 3;

    [Tooltip("Spawn radius around this spawner")]
    public float spawnRadius = 3f;

    public bool autoSpawnOnStart = true;

    private void Start()
    {
        if (autoSpawnOnStart)
            Spawn();
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: no enemy prefabs assigned.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
