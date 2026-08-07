using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Race { Slime, Human, Elf, Demon }
    public enum Difficulty { Easy, Normal, Hard }

    public Race SelectedRace = Race.Slime;
    public Difficulty SelectedDifficulty = Difficulty.Normal;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetRace(Race race)
    {
        SelectedRace = race;
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        SelectedDifficulty = difficulty;
    }

    public void StartGame()
    {
        // Load the main world map (scene should be named "WorldMap")
        SceneManager.LoadScene("WorldMap");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "WorldMap")
        {
            // Try to load player prefab from Resources/Prefabs/Player
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Player");
            if (prefab == null)
            {
                Debug.LogWarning("Player prefab not found in Resources/Prefabs/Player. Please create it.");
                return;
            }

            // Find spawn point
            GameObject spawnObj = GameObject.Find("StartDungeonSpawn");
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;
            if (spawnObj != null)
            {
                spawnPos = spawnObj.transform.position;
                spawnRot = spawnObj.transform.rotation;
            }

            GameObject player = Instantiate(prefab, spawnPos, spawnRot);

            // Apply race/difficulty to the player if Player component exists
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.ApplyRaceAndDifficulty(SelectedRace, SelectedDifficulty);
            }
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame(); else PauseGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
