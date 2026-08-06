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

    public static UIController GetInstance() => instance;
}
