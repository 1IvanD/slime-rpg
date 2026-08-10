using System;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldTimeManager : MonoBehaviour
{
    public static WorldTimeManager Instance { get; private set; }

    [Header("Config")]
    public TimeOfDayConfigSO config;

    [Header("Runtime")]
    [Range(0f,24f)]
    public float hourOfDay = 8f; // 0..24

    public event Action OnDayStarted;
    public event Action OnNightStarted;
    public event Action<float> OnHourChanged; // current hour

    private bool wasNight = false;
    private float secondsPerHour;
    private float accumulatedSeconds = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (config == null)
        {
            Debug.LogWarning("WorldTimeManager: no TimeOfDayConfigSO assigned.");
            return;
        }
        secondsPerHour = config.dayLengthSeconds / 24f;
        wasNight = IsNight();
    }

    private void Update()
    {
        if (config == null || !config.autoAdvance) return;
        float delta = Time.deltaTime * Mathf.Max(0.0001f, config.timeScale);
        accumulatedSeconds += delta;

        while (accumulatedSeconds >= secondsPerHour)
        {
            accumulatedSeconds -= secondsPerHour;
            hourOfDay += 1f;
            if (hourOfDay >= 24f) hourOfDay -= 24f;
            OnHourChanged?.Invoke(hourOfDay);
            CheckDayNightTransitions();
        }
    }

    private void CheckDayNightTransitions()
    {
        bool nowNight = IsNight();
        if (nowNight != wasNight)
        {
            wasNight = nowNight;
            if (nowNight)
            {
                OnNightStarted?.Invoke();
                Debug.Log("Night started");
            }
            else
            {
                OnDayStarted?.Invoke();
                Debug.Log("Day started");
            }
        }
    }

    public bool IsNight()
    {
        if (config == null) return false;
        // consider night outside sunrise..sunset
        if (config.sunriseHour < config.sunsetHour)
            return !(hourOfDay >= config.sunriseHour && hourOfDay < config.sunsetHour);
        else
            return (hourOfDay >= config.sunsetHour && hourOfDay < config.sunriseHour);
    }

    public float GetNormalizedTime() => hourOfDay / 24f;
}
