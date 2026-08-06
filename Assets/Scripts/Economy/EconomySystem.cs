using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    public static EconomySystem Instance { get; private set; }

    private float playerGold = 1000f; // Начальное количество золота
    private float playerExperiencePoints = 0f;
    private int playerLevel = 1;
    private float totalWealthGenerated = 0f;

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public float price;
        public string description;
        public ItemType type;
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Skill,
        Summon
    }

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

    public void AddGold(float amount)
    {
        playerGold += amount;
        totalWealthGenerated += amount;
        Debug.Log($"Gold added: +{amount}. Total: {playerGold}");
    }

    public bool SpendGold(float amount)
    {
        if (playerGold >= amount)
        {
            playerGold -= amount;
            Debug.Log($"Gold spent: -{amount}. Remaining: {playerGold}");
            return true;
        }
        Debug.Log($"Not enough gold! Need: {amount}, Have: {playerGold}");
        return false;
    }

    public void AddExperiencePoints(float amount)
    {
        playerExperiencePoints += amount;
        Debug.Log($"Experience added: +{amount}");

        // Проверка повышения уровня
        if (playerExperiencePoints >= 100 * playerLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        playerLevel++;
        playerExperiencePoints = 0;
        AddGold(100 * playerLevel); // Бонус золота за повышение уровня
        Debug.Log($"Level Up! New level: {playerLevel}");
    }

    public float GetGold() => playerGold;
    public float GetExperiencePoints() => playerExperiencePoints;
    public int GetLevel() => playerLevel;
    public float GetTotalWealthGenerated() => totalWealthGenerated;
}
