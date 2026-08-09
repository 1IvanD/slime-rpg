using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/World/Weather", fileName = "WeatherSO")]
public class WeatherSO : ScriptableObject
{
    public string id;
    public string displayName = "Weather";
    [TextArea] public string description;

    [Tooltip("Relative weight during day (higher = more likely)")]
    public float weightDay = 1f;
    [Tooltip("Relative weight during night (higher = more likely)")]
    public float weightNight = 1f;

    [Tooltip("Average duration in seconds for this weather (if zero, system default used)")]
    public Vector2 durationRange = new Vector2(60f, 300f);

    [Tooltip("Optional visual prefab/pfx to spawn for this weather (set in Editor)")]
    public GameObject weatherVFXPrefab;
}
