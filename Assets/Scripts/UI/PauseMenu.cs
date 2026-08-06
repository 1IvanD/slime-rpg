using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;
    private bool isPaused = false;

    private void Start()
    {
        CreatePauseMenu();
    }

    private void CreatePauseMenu()
    {
        // Найти Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;
        
        pauseMenuPanel = new GameObject("PauseMenu");
        pauseMenuPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform pauseRect = pauseMenuPanel.AddComponent<RectTransform>();
        pauseRect.anchorMin = Vector2.zero;
        pauseRect.anchorMax = Vector2.one;
        pauseRect.offsetMin = Vector2.zero;
        pauseRect.offsetMax = Vector2.zero;
        
        Image pauseBg = pauseMenuPanel.AddComponent<Image>();
        pauseBg.color = new Color(0, 0, 0, 0.7f);
        
        // Панель меню
        GameObject menuPanelObj = new GameObject("MenuPanel");
        menuPanelObj.transform.SetParent(pauseMenuPanel.transform, false);
        
        RectTransform menuPanelRect = menuPanelObj.AddComponent<RectTransform>();
        menuPanelRect.anchorMin = new Vector2(0.3f, 0.2f);
        menuPanelRect.anchorMax = new Vector2(0.7f, 0.8f);
        menuPanelRect.offsetMin = Vector2.zero;
        menuPanelRect.offsetMax = Vector2.zero;
        
        Image menuPanelImage = menuPanelObj.AddComponent<Image>();
        menuPanelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        
        VerticalLayoutGroup layoutGroup = menuPanelObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 15;
        layoutGroup.padding = new RectOffset(20, 20, 20, 20);
        layoutGroup.childForceExpandWidth = true;
        
        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(menuPanelObj.transform, false);
        
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 60);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "PAUSED";
        titleText.fontSize = 50;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.red;
        
        // Кнопка продолжить
        CreateMenuButtonForPause(menuPanelObj.transform, "Resume [P]", () => {
            ResumeGame();
        });
        
        // Кнопка выхода
        CreateMenuButtonForPause(menuPanelObj.transform, "Quit to Menu [Esc]", () => {
            GameManager.Instance.QuitGame();
        });
        
        pauseMenuPanel.SetActive(false);
    }
    
    private void CreateMenuButtonForPause(Transform parent, string label, System.Action onClick)
    {
        GameObject buttonObj = new GameObject(label);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.anchorMin = Vector2.zero;
        buttonRect.anchorMax = Vector2.one;
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;
        buttonRect.sizeDelta = new Vector2(0, 50);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.3f, 0.5f, 0.8f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.colors = new ColorBlock()
        {
            normalColor = new Color(0.3f, 0.3f, 0.5f, 0.8f),
            highlightedColor = new Color(0.5f, 0.5f, 0.7f, 1),
            pressedColor = new Color(0.2f, 0.2f, 0.4f, 1),
            disabledColor = new Color(0.3f, 0.3f, 0.5f, 0.5f),
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };
        
        button.onClick.AddListener(() => onClick?.Invoke());
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = label;
        buttonText.fontSize = 32;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }
}
