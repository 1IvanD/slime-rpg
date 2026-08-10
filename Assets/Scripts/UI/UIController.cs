using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    
    private TextMeshProUGUI healthText;
    private Image healthBarFill;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI experienceText;
    private TextMeshProUGUI enemyNameText;
    private TextMeshProUGUI attackText;
    private TextMeshProUGUI defenseText;
    private TextMeshProUGUI absorbedText;
    private TextMeshProUGUI skillsCountText;
    private Transform skillsListTransform;
    private Transform notificationPanelTransform;
    
    private Player player;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        FindUIElements();
        player = FindObjectOfType<Player>();
        
        if (player != null)
        {
            playerStats = player.GetStats();
        }
    }

    private void FindUIElements()
    {
        // Поиск элементов HUD
        Transform hudPanel = FindChildByName(transform, "HUDPanel");
        if (hudPanel != null)
        {
            healthText = FindChildByName(hudPanel, "HealthDisplay/HealthText")?.GetComponent<TextMeshProUGUI>();
            Transform healthBar = FindChildByName(hudPanel, "HealthDisplay/HealthBar/Fill");
            healthBarFill = healthBar?.GetComponent<Image>();
            
            levelText = FindChildByName(hudPanel, "LevelDisplay/LevelText")?.GetComponent<TextMeshProUGUI>();
            experienceText = FindChildByName(hudPanel, "ExperienceDisplay/ExperienceText")?.GetComponent<TextMeshProUGUI>();
            enemyNameText = FindChildByName(hudPanel, "EnemyNameDisplay/EnemyText")?.GetComponent<TextMeshProUGUI>();
        }
        
        // Поиск панели статистики
        Transform statsPanel = FindChildByName(transform, "StatsPanel");
        if (statsPanel != null)
        {
            attackText = FindChildByName(statsPanel, "ATKStat/Value")?.GetComponent<TextMeshProUGUI>();
            defenseText = FindChildByName(statsPanel, "DEFStat/Value")?.GetComponent<TextMeshProUGUI>();
            absorbedText = FindChildByName(statsPanel, "AbsorbedStat/Value")?.GetComponent<TextMeshProUGUI>();
            skillsCountText = FindChildByName(statsPanel, "SkillsStat/Value")?.GetComponent<TextMeshProUGUI>();
        }
        
        // Поиск панели умений
        skillsListTransform = FindChildByName(transform, "SkillsPanel/SkillsList");
        
        // Поиск панели уведомлений
        notificationPanelTransform = FindChildByName(transform, "NotificationPanel");
    }

    private Transform FindChildByName(Transform parent, string path)
    {
        string[] parts = path.Split('/');
        Transform current = parent;
        
        foreach (string part in parts)
        {
            current = current.Find(part);
            if (current == null) return null;
        }
        
        return current;
    }

    private void Update()
    {
        if (playerStats != null)
        {
            UpdateHUD();
            UpdateStats();
        }
        
        HandleKeyboardInput();
    }

    private void UpdateHUD()
    {
        if (healthText != null)
            healthText.text = $"HP: {playerStats.Health:F0}/{playerStats.MaxHealth:F0}";
        
        if (healthBarFill != null)
            healthBarFill.fillAmount = playerStats.Health / playerStats.MaxHealth;
        
        if (levelText != null)
            levelText.text = $"Level: {playerStats.Level}";
        
        if (experienceText != null)
            experienceText.text = $"Exp: {playerStats.Experience:F0}/{playerStats.ExperienceThreshold:F0}";
    }

    private void UpdateStats()
    {
        if (attackText != null)
            attackText.text = playerStats.Attack.ToString("F1");
        
        if (defenseText != null)
            defenseText.text = playerStats.Defense.ToString("F1");
        
        if (absorbedText != null)
            absorbedText.text = playerStats.AbsorbedEnemies.ToString();
        
        if (skillsCountText != null)
            skillsCountText.text = playerStats.UniqueSkillsLearned.ToString();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.PauseGame();
            ShowNotification("Game Paused");
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.QuitGame();
        }

        // quick keys: I = inventory, K = skills
        if (Input.GetKeyDown(KeyCode.I)) ToggleInventoryUI();
        if (Input.GetKeyDown(KeyCode.K)) ToggleSkillsUI();
    }

    public void ToggleInventoryUI()
    {
        var invPanel = FindChildByName(this.transform, "InventoryPanel");
        if (invPanel != null)
        {
            invPanel.gameObject.SetActive(!invPanel.gameObject.activeSelf);
            // refresh if showing
            if (invPanel.gameObject.activeSelf)
            {
                var invCtrl = invPanel.GetComponent<InventoryUIController>() ?? invPanel.GetComponentInChildren<InventoryUIController>();
                invCtrl?.Refresh();
            }
        }
        else
        {
            Debug.LogWarning("ToggleInventoryUI: InventoryPanel not found on canvas.");
        }
    }

    public void ToggleSkillsUI()
    {
        var skillsPanel = FindChildByName(this.transform, "SkillsPanel");
        if (skillsPanel != null)
        {
            skillsPanel.gameObject.SetActive(!skillsPanel.gameObject.activeSelf);
            if (skillsPanel.gameObject.activeSelf)
            {
                var ctrl = skillsPanel.GetComponent<SkillsUIController>() ?? skillsPanel.GetComponentInChildren<SkillsUIController>();
                ctrl?.Refresh();
            }
        }
        else
        {
            Debug.LogWarning("ToggleSkillsUI: SkillsPanel not found on canvas.");
        }
    }

    public void ShowNotification(string message)
    {
        if (notificationPanelTransform == null) return;
        
        GameObject notifObj = new GameObject("Notification");
        notifObj.transform.SetParent(notificationPanelTransform);
        
        RectTransform notifRect = notifObj.AddComponent<RectTransform>();
        notifRect.anchorMin = Vector2.zero;
        notifRect.anchorMax = Vector2.one;
        notifRect.offsetMin = Vector2.zero;
        notifRect.offsetMax = Vector2.zero;
        notifRect.sizeDelta = new Vector2(0, 40);
        
        Image notifBg = notifObj.AddComponent<Image>();
        notifBg.color = new Color(0.2f, 0.5f, 0.2f, 0.8f);
        
        TextMeshProUGUI notifText = notifObj.AddComponent<TextMeshProUGUI>();
        notifText.text = message;
        notifText.fontSize = 24;
        notifText.alignment = TextAlignmentOptions.Center;
        notifText.color = Color.white;
        
        StartCoroutine(RemoveNotificationAfterDelay(notifObj, 3f));
    }
    
    private System.Collections.IEnumerator RemoveNotificationAfterDelay(GameObject notif, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(notif);
    }

    // Name input UI for MagicalBeast naming (interaction-based)
    public void ShowNameInput(string prompt, System.Action<string> onSubmit)
    {
        // Use the main canvas (this.transform is the canvas when UIController is attached to MainCanvas)
        Transform canvasTransform = this.transform;
        if (canvasTransform == null)
        {
            Debug.LogWarning("UIController: Canvas transform not found for name input.");
            return;
        }

        GameObject panel = new GameObject("NameInputPanel");
        panel.transform.SetParent(canvasTransform, false);
        RectTransform pr = panel.AddComponent<RectTransform>();
        pr.sizeDelta = new Vector2(420, 140);
        pr.anchorMin = new Vector2(0.5f, 0.1f);
        pr.anchorMax = new Vector2(0.5f, 0.1f);
        pr.anchoredPosition = new Vector2(0, 80);

        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.85f);

        // Prompt text
        var promptGO = new GameObject("Prompt");
        promptGO.transform.SetParent(panel.transform, false);
        var promptText = promptGO.AddComponent<TextMeshProUGUI>();
        promptText.text = prompt;
        promptText.alignment = TextAlignmentOptions.TopLeft;
        promptText.color = Color.white;
        var pRT = promptGO.GetComponent<RectTransform>();
        pRT.anchorMin = new Vector2(0.05f, 0.6f);
        pRT.anchorMax = new Vector2(0.95f, 0.95f);
        pRT.offsetMin = pRT.offsetMax = Vector2.zero;

        // Input field container
        var inputGO = new GameObject("Input");
        inputGO.transform.SetParent(panel.transform, false);
        var inputRT = inputGO.AddComponent<RectTransform>();
        inputRT.anchorMin = new Vector2(0.05f, 0.25f);
        inputRT.anchorMax = new Vector2(0.95f, 0.55f);
        inputRT.offsetMin = inputRT.offsetMax = Vector2.zero;

        var inputField = inputGO.AddComponent<TMP_InputField>();
        var textGO = new GameObject("Text");
        textGO.transform.SetParent(inputGO.transform, false);
        var textComp = textGO.AddComponent<TextMeshProUGUI>();
        textComp.fontSize = 20;
        textComp.color = Color.white;
        textComp.alignment = TextAlignmentOptions.Left;
        inputField.textComponent = textComp;

        // OK button
        var okGO = new GameObject("OK");
        okGO.transform.SetParent(panel.transform, false);
        var okImg = okGO.AddComponent<Image>();
        okImg.color = new Color(0.2f, 0.6f, 0.2f, 1f);
        var okBtn = okGO.AddComponent<Button>();
        var okText = new GameObject("Text");
        okText.transform.SetParent(okGO.transform, false);
        var okTMP = okText.AddComponent<TextMeshProUGUI>();
        okTMP.text = "OK";
        okTMP.alignment = TextAlignmentOptions.Center;
        okTMP.color = Color.white;
        var okRT = okGO.GetComponent<RectTransform>();
        okRT.anchorMin = new Vector2(0.55f, 0.05f);
        okRT.anchorMax = new Vector2(0.9f, 0.2f);
        okRT.offsetMin = okRT.offsetMax = Vector2.zero;

        // Cancel button
        var cancelGO = new GameObject("Cancel");
        cancelGO.transform.SetParent(panel.transform, false);
        var cancelImg = cancelGO.AddComponent<Image>();
        cancelImg.color = new Color(0.6f, 0.2f, 0.2f, 1f);
        var cancelBtn = cancelGO.AddComponent<Button>();
        var cancelText = new GameObject("Text");
        cancelText.transform.SetParent(cancelGO.transform, false);
        var cancelTMP = cancelText.AddComponent<TextMeshProUGUI>();
        cancelTMP.text = "Cancel";
        cancelTMP.alignment = TextAlignmentOptions.Center;
        cancelTMP.color = Color.white;
        var cancelRT = cancelGO.GetComponent<RectTransform>();
        cancelRT.anchorMin = new Vector2(0.1f, 0.05f);
        cancelRT.anchorMax = new Vector2(0.45f, 0.2f);
        cancelRT.offsetMin = cancelRT.offsetMax = Vector2.zero;

        okBtn.onClick.AddListener(() => { onSubmit?.Invoke(inputField.text); Destroy(panel); });
        cancelBtn.onClick.AddListener(() => { Destroy(panel); });
    }

    public static UIController GetInstance() => instance;
}
