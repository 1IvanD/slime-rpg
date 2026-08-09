using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DynamicEventManager : MonoBehaviour
{
    public static DynamicEventManager Instance { get; private set; }

    [Tooltip("List of dynamic events available in the world")]
    public List<DynamicEventSO> events = new List<DynamicEventSO>();

    [Tooltip("How often (seconds) to check for random events")]
    public float checkInterval = 30f;

    [Tooltip("Global multiplier for event spawn chances")]
    public float spawnMultiplier = 1f;

    private float nextCheck = 0f;
    private Dictionary<string, float> lastTriggered = new Dictionary<string, float>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        nextCheck = Time.time + checkInterval * 0.5f;
    }

    private void Update()
    {
        if (Time.time < nextCheck) return;
        nextCheck = Time.time + checkInterval;
        TryTriggerEvents();
    }

    private void TryTriggerEvents()
    {
        bool night = WorldTimeManager.Instance != null && WorldTimeManager.Instance.IsNight();
        var weather = WeatherSystem.Instance != null ? WeatherSystem.Instance.currentWeather : null;

        foreach (var ev in events)
        {
            if (ev == null) continue;
            // rate limit
            if (lastTriggered.TryGetValue(ev.id, out float last) && Time.time - last < ev.minInterval) continue;

            float baseChance = night ? ev.spawnChanceNight : ev.spawnChanceDay;
            float chance = baseChance * spawnMultiplier;

            // simple weather modifier: storms increase ambush chance, decrease merchant
            if (weather != null && ev.id != null)
            {
                if (weather.displayName.ToLower().Contains("storm") && ev.displayName.ToLower().Contains("ambush")) chance *= 1.5f;
                if (weather.displayName.ToLower().Contains("rain") && ev.displayName.ToLower().Contains("merchant")) chance *= 0.6f;
            }

            if (Random.value <= chance)
            {
                TriggerEvent(ev);
                lastTriggered[ev.id] = Time.time;
                // do not trigger more than one event per check for now
                break;
            }
        }
    }

    private void TriggerEvent(DynamicEventSO ev)
    {
        Debug.Log($"DynamicEvent triggered: {ev.displayName}");
        // spawn prefab if provided
        if (ev.eventPrefab != null)
        {
            Instantiate(ev.eventPrefab, Vector3.zero, Quaternion.identity);
        }

        // TODO: integrate with game systems: open dialog, spawn enemies, give merchant inventory, etc.
    }
}
