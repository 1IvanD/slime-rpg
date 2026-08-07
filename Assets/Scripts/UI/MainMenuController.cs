using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject racePanel;
    public GameObject difficultyPanel;

    [Header("Main Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Race UI")]
    public Dropdown raceDropdown;
    public Button raceNextButton;

    [Header("Difficulty UI")]
    public Dropdown difficultyDropdown;
    public Button startButton;

    private void Start()
    {
        // Wire up buttons
        if (playButton != null) playButton.onClick.AddListener(ShowRacePanel);
        if (exitButton != null) exitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        if (raceNextButton != null) raceNextButton.onClick.AddListener(ShowDifficultyPanel);
        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);

        // Populate dropdowns if empty
        if (raceDropdown != null && raceDropdown.options.Count == 0)
        {
            raceDropdown.options.Add(new Dropdown.OptionData("Slime"));
            raceDropdown.options.Add(new Dropdown.OptionData("Human"));
            raceDropdown.options.Add(new Dropdown.OptionData("Elf"));
            raceDropdown.options.Add(new Dropdown.OptionData("Demon"));
            raceDropdown.value = 0;
        }

        if (difficultyDropdown != null && difficultyDropdown.options.Count == 0)
        {
            difficultyDropdown.options.Add(new Dropdown.OptionData("Easy"));
            difficultyDropdown.options.Add(new Dropdown.OptionData("Normal"));
            difficultyDropdown.options.Add(new Dropdown.OptionData("Hard"));
            difficultyDropdown.value = 1;
        }

        ShowMainPanel();
    }

    private void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (racePanel != null) racePanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
    }

    private void ShowRacePanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (racePanel != null) racePanel.SetActive(true);
    }

    private void ShowDifficultyPanel()
    {
        // Save selected race
        if (raceDropdown != null)
        {
            GameManager.Race selected = (GameManager.Race)raceDropdown.value;
            GameManager.Instance.SetRace(selected);
        }

        if (racePanel != null) racePanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(true);
    }

    private void OnStartClicked()
    {
        if (difficultyDropdown != null)
        {
            GameManager.Difficulty d = (GameManager.Difficulty)difficultyDropdown.value;
            GameManager.Instance.SetDifficulty(d);
        }

        // Start the game
        GameManager.Instance.StartGame();
    }
}
