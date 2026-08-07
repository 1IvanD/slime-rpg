using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayer(PlayerStats stats)
    {
        PlayerPrefs.SetFloat("player_health", stats.Health);
        PlayerPrefs.SetFloat("player_maxHealth", stats.MaxHealth);
        PlayerPrefs.SetInt("player_level", stats.Level);
        PlayerPrefs.SetFloat("player_experience", stats.Experience);
        PlayerPrefs.Save();
    }

    public void LoadPlayer(PlayerStats stats)
    {
        stats.Health = PlayerPrefs.GetFloat("player_health", stats.Health);
        stats.MaxHealth = PlayerPrefs.GetFloat("player_maxHealth", stats.MaxHealth);
        stats.Level = PlayerPrefs.GetInt("player_level", stats.Level);
        stats.Experience = PlayerPrefs.GetFloat("player_experience", stats.Experience);
    }
}
