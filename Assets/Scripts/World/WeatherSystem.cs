using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance { get; private set; }

    [Tooltip("List of available weather definitions (ScriptableObjects)")]
    public List<WeatherSO> availableWeathers = new List<WeatherSO>();

    public WeatherSO currentWeather { get; private set; }

    public event Action<WeatherSO> OnWeatherChanged;

    private float weatherEndTime = 0f;
    private System.Random rnd = new System.Random();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (WorldTimeManager.Instance != null)
        {
            WorldTimeManager.Instance.OnDayStarted += OnDayOrNightChanged;
            WorldTimeManager.Instance.OnNightStarted += OnDayOrNightChanged;
        }
        ChooseInitialWeather();
    }

    private void Update()
    {
        if (currentWeather == null) return;
        if (Time.time >= weatherEndTime)
        {
            ChooseNextWeather();
        }
    }

    private void OnDayOrNightChanged()
    {
        // small chance to change weather at day/night boundaries
        if (UnityEngine.Random.value < 0.25f)
            ChooseNextWeather();
    }

    private void ChooseInitialWeather()
    {
        if (availableWeathers.Count == 0) return;
        currentWeather = availableWeathers[0];
        ScheduleEndForCurrent();
        OnWeatherChanged?.Invoke(currentWeather);
    }

    private void ChooseNextWeather()
    {
        var list = availableWeathers;
        if (list == null || list.Count == 0) return;

        float total = 0f;
        bool night = WorldTimeManager.Instance != null && WorldTimeManager.Instance.IsNight();
        foreach (var w in list)
        {
            total += night ? w.weightNight : w.weightDay;
        }
        if (total <= 0f) return;

        float r = (float)(rnd.NextDouble() * total);
        float s = 0f;
        foreach (var w in list)
        {
            s += night ? w.weightNight : w.weightDay;
            if (r <= s)
            {
                if (w != currentWeather)
                {
                    currentWeather = w;
                    OnWeatherChanged?.Invoke(currentWeather);
                }
                break;
            }
        }
        ScheduleEndForCurrent();
    }

    private void ScheduleEndForCurrent()
    {
        if (currentWeather == null) return;
        var range = currentWeather.durationRange;
        float dur = UnityEngine.Random.Range(range.x, range.y);
        weatherEndTime = Time.time + Mathf.Max(5f, dur);
    }

    public void ForceWeather(WeatherSO weather, float durationSeconds = -1f)
    {
        currentWeather = weather;
        if (durationSeconds > 0f) weatherEndTime = Time.time + durationSeconds;
        else ScheduleEndForCurrent();
        OnWeatherChanged?.Invoke(currentWeather);
    }
}
