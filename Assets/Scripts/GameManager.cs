using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PauseMenu pauseMenu;
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
    }

    private void Start()
    {
        // PauseMenu будет добавлен на CanvasSetup, поэтому на старте сцены он уже должен существовать
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenu == null) pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null) pauseMenu.PauseGame();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenu == null) pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null) pauseMenu.ResumeGame();
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    public void QuitGame()
    {
        // Попытаться загрузить сцену MainMenu, если она есть в билд-сеттингах. Иначе выйти из приложения.
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        // Попытка загрузить сцену MainMenu — если её нет, то просто выйти
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Application.Quit();
        }
    }
}
