using System.IO;
using UnityEditor;
using UnityEngine;

public static class WorldSetupUtility
{
    private const string assetsFolder = "Assets/Data/World";

    [MenuItem("Tools/Tempest/Generate World Assets (Time/Weather/Events)")]
    public static void GenerateWorldAssets()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder(assetsFolder))
            AssetDatabase.CreateFolder("Assets/Data", "World");

        // Time config
        var timePath = Path.Combine(assetsFolder, "TimeOfDayConfigSO.asset");
        var timeConfig = ScriptableObject.CreateInstance<TimeOfDayConfigSO>();
        timeConfig.dayLengthSeconds = 1200f;
        timeConfig.sunriseHour = 6f;
        timeConfig.sunsetHour = 18f;
        timeConfig.autoAdvance = true;
        AssetDatabase.CreateAsset(timeConfig, timePath);

        // Weather: Clear
        var clear = ScriptableObject.CreateInstance<WeatherSO>();
        clear.id = "clear";
        clear.displayName = "Clear";
        clear.description = "Sunny/clear weather.";
        clear.weightDay = 5f;
        clear.weightNight = 3f;
        clear.durationRange = new Vector2(120f, 600f);
        AssetDatabase.CreateAsset(clear, Path.Combine(assetsFolder, "Weather_Clear.asset"));

        // Rain
        var rain = ScriptableObject.CreateInstance<WeatherSO>();
        rain.id = "rain";
        rain.displayName = "Rain";
        rain.description = "Rainy weather — affects merchant spawns and some monsters.";
        rain.weightDay = 2f;
        rain.weightNight = 2f;
        rain.durationRange = new Vector2(60f, 300f);
        AssetDatabase.CreateAsset(rain, Path.Combine(assetsFolder, "Weather_Rain.asset"));

        // Fog
        var fog = ScriptableObject.CreateInstance<WeatherSO>();
        fog.id = "fog";
        fog.displayName = "Fog";
        fog.description = "Foggy weather — reduces sight range for some systems.";
        fog.weightDay = 1f;
        fog.weightNight = 2f;
        fog.durationRange = new Vector2(60f, 240f);
        AssetDatabase.CreateAsset(fog, Path.Combine(assetsFolder, "Weather_Fog.asset"));

        // Storm
        var storm = ScriptableObject.CreateInstance<WeatherSO>();
        storm.id = "storm";
        storm.displayName = "Storm";
        storm.description = "Thunderstorm — increases dangerous spawns.";
        storm.weightDay = 0.5f;
        storm.weightNight = 1.5f;
        storm.durationRange = new Vector2(30f, 180f);
        AssetDatabase.CreateAsset(storm, Path.Combine(assetsFolder, "Weather_Storm.asset"));

        // Dynamic events: Merchant
        var merchant = ScriptableObject.CreateInstance<DynamicEventSO>();
        merchant.id = "merchant";
        merchant.displayName = "Wandering Merchant";
        merchant.description = "A traveling merchant appears and offers goods.";
        merchant.spawnChanceDay = 0.08f;
        merchant.spawnChanceNight = 0.02f;
        merchant.minInterval = 600f;
        AssetDatabase.CreateAsset(merchant, Path.Combine(assetsFolder, "Event_Merchant.asset"));

        // Ambush
        var ambush = ScriptableObject.CreateInstance<DynamicEventSO>();
        ambush.id = "ambush";
        ambush.displayName = "Bandit Ambush";
        ambush.description = "Hostile ambush — enemies will spawn and attack.";
        ambush.spawnChanceDay = 0.04f;
        ambush.spawnChanceNight = 0.12f;
        ambush.minInterval = 300f;
        AssetDatabase.CreateAsset(ambush, Path.Combine(assetsFolder, "Event_Ambush.asset"));

        // Forest spirit
        var spirit = ScriptableObject.CreateInstance<DynamicEventSO>();
        spirit.id = "forest_spirit";
        spirit.displayName = "Forest Spirit";
        spirit.description = "A neutral/beneficial spirit appears — may grant a boon.";
        spirit.spawnChanceDay = 0.03f;
        spirit.spawnChanceNight = 0.03f;
        spirit.minInterval = 900f;
        AssetDatabase.CreateAsset(spirit, Path.Combine(assetsFolder, "Event_ForestSpirit.asset"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = timeConfig;

        Debug.Log("Tempest: Generated Time/Weather/DynamicEvent SO assets in Assets/Data/World.\nNext: add WorldTimeManager, WeatherSystem and DynamicEventManager gameobjects to your scene and assign the created assets in the inspector.");
    }
}
