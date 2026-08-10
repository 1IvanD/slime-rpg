using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/World/DynamicEvent", fileName = "DynamicEventSO")]
public class DynamicEventSO : ScriptableObject
{
    public string id;
    public string displayName = "Event";
    [TextArea] public string description;

    [Tooltip("Base spawn chance per check (0..1)")]
    [Range(0f,1f)] public float spawnChanceDay = 0.05f;
    [Tooltip("Base spawn chance per check (0..1)")]
    [Range(0f,1f)] public float spawnChanceNight = 0.1f;

    [Tooltip("Minimum interval in seconds between occurrences of this event")]
    public float minInterval = 60f;

    [Tooltip("Optional prefab to spawn when event triggers")]
    public GameObject eventPrefab;
}
