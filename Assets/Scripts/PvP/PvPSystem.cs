using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PvPRating
{
    public string playerId;
    public int wins;
    public int losses;
    public float winRate;
    public int rating;
    public string rank;
}

public class PvPSystem : MonoBehaviour
{
    public static PvPSystem Instance { get; private set; }

    private PvPRating playerRating;
    private List<PvPRating> leaderboard = new List<PvPRating>();
    private Dictionary<string, int> pvpModes = new Dictionary<string, int>();

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
        InitializePlayerRating();
        InitializePvPModes();
    }

    private void InitializePlayerRating()
    {
        playerRating = new PvPRating
        {
            playerId = "Player",
            wins = 0,
            losses = 0,
            winRate = 0f,
            rating = 1000,
            rank = "Новичок"
        };
    }

    private void InitializePvPModes()
    {
        pvpModes["1v1 Дуэль"] = 0;
        pvpModes["3v3 Командный бой"] = 0;
        pvpModes["5v5 Арена"] = 0;
        pvpModes["Королевская битва"] = 0;
    }

    public void WinPvPMatch(int ratingGain = 25)
    {
        playerRating.wins++;
        playerRating.rating += ratingGain;
        UpdateRank();
        EconomySystem.Instance.AddGold(100);
        Debug.Log($"🏆 Победа в PvP! +{ratingGain} рейтинга. Всего побед: {playerRating.wins}");
    }

    public void LosePvPMatch(int ratingLoss = 15)
    {
        playerRating.losses++;
        playerRating.rating = Mathf.Max(0, playerRating.rating - ratingLoss);
        UpdateRank();
        Debug.Log($"❌ Поражение в PvP! -{ratingLoss} рейтинга. Всего поражений: {playerRating.losses}");
    }

    private void UpdateRank()
    {
        playerRating.winRate = playerRating.wins > 0 ? 
            (float)playerRating.wins / (playerRating.wins + playerRating.losses) * 100 : 0;

        if (playerRating.rating >= 3000) playerRating.rank = "Легенда";
        else if (playerRating.rating >= 2500) playerRating.rank = "Король";
        else if (playerRating.rating >= 2000) playerRating.rank = "Чемпион";
        else if (playerRating.rating >= 1500) playerRating.rank = "Мастер";
        else if (playerRating.rating >= 1000) playerRating.rank = "Эксперт";
        else if (playerRating.rating >= 500) playerRating.rank = "Опытный";
        else playerRating.rank = "Новичок";
    }

    public void EnterPvPArena(string modeType)
    {
        Debug.Log($"🔥 Вы вошли в: {modeType}");
        Debug.Log($"📊 Ваш рейтинг: {playerRating.rating} ({playerRating.rank})");
        Debug.Log($"📈 Процент побед: {playerRating.winRate:F1}%");
    }

    public PvPRating GetPlayerRating() => playerRating;
    public Dictionary<string, int> GetPvPModes() => pvpModes;
}
