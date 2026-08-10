using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/World/TimeOfDayConfig", fileName = "TimeOfDayConfigSO")]
public class TimeOfDayConfigSO : ScriptableObject
{
    [Tooltip("Length of full day in real seconds (default: 1200 = 20 minutes)")]
    public float dayLengthSeconds = 1200f;

    [Range(0f,24f)] public float sunriseHour = 6f;
    [Range(0f,24f)] public float sunsetHour = 18f;

    [Tooltip("If true, time will progress automatically at runtime.")]
    public bool autoAdvance = true;

    [Tooltip("Time multiplier applied to Time.deltaTime when advancing time.")]
    public float timeScale = 1f;
}
