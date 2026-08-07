using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventManager : MonoBehaviour
{
    public static StoryEventManager Instance { get; private set; }

    [System.Serializable]
    public class WorldState
    {
        public bool isSettlementCreated = false;
        public bool goblinsSaved = false;
        public bool goblinVillageDestroyed = false;
        public bool wolvesAllegiance = false;
        public bool orcArmySpawned = false;
        public bool falmosAttackTriggered = false;
        public string playerEvolution = "None";
        public string settlementName = "";
    }

    public WorldState state = new WorldState();

    // Resource path for enemy prefabs (same convention as DungeonManager)
    public string enemiesResourcePath = "Prefabs/Enemies";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadState();
    }

    public void OnGoblinsHelped()
    {
        state.goblinsSaved = true;
        UIController.GetInstance()?.ShowNotification("Гоблины спасены. Они будут помогать вам.");
        // Give quest to build settlement
        QuestManager.Instance?.AddQuest("CreateSettlement", "Построй поселение в Лесу Джуры (Темпест)");
        SaveState();
    }

    public void OnGoblinsRefused()
    {
        state.goblinsSaved = false;
        UIController.GetInstance()?.ShowNotification("Вы отказались помочь гоблинам. Их деревня может погибнуть.");
        // schedule village destruction in 60 seconds for prototype
        Invoke(nameof(DestroyGoblinVillage), 60f);
        SaveState();
    }

    private void DestroyGoblinVillage()
    {
        state.goblinVillageDestroyed = true;
        UIController.GetInstance()?.ShowNotification("Деревня гоблинов уничтожена волками.");
        // Make world more dangerous: boost wolf spawn or set faction enemy
        var fm = FindObjectOfType<FactionManager>();
        fm?.AddEnemy("Goblins");
        SaveState();
    }

    public void OnWolfLeaderDefeated(bool acceptedAllegiance)
    {
        state.wolvesAllegiance = acceptedAllegiance;
        if (acceptedAllegiance)
        {
            UIController.GetInstance()?.ShowNotification("Волки присягнули вам на верность.");
            SkillManager.Instance?.Unlock("FenrirCry");
        }
        else
        {
            UIController.GetInstance()?.ShowNotification("Волки вознесли обиду — они станут более враждебными.");
            var fm = FindObjectOfType<FactionManager>();
            fm?.AddEnemy("Wolves");
        }
        SaveState();
    }

    public void OnSettlementCreated(string name)
    {
        state.isSettlementCreated = true;
        state.settlementName = name;
        UIController.GetInstance()?.ShowNotification($"Создано поселение: {name}");
        // Spawn faction NPCs (placeholders)
        SpawnSettlementFactions();
        // Schedule orc army (for prototype, after 90 seconds)
        Invoke(nameof(SpawnOrcArmy), 90f);
        SaveState();
    }

    private void SpawnSettlementFactions()
    {
        // Simple placeholders: create a few NPC gameobjects to represent gnomes, lizards, spirits
        Vector3 basePos = new Vector3(15f, 0f, 5f);
        CreateFactionPlaceholder("Gnomes", basePos + new Vector3(2,0,2));
        CreateFactionPlaceholder("Lizards", basePos + new Vector3(-2,0,2));
        CreateFactionPlaceholder("Spirits", basePos + new Vector3(0,0,4));

        UIController.GetInstance()?.ShowNotification("Гномы, ящеры и духи замечены рядом с поселением.");
    }

    private void CreateFactionPlaceholder(string name, Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = $"Faction_{name}";
        go.transform.position = pos;
        var label = go.AddComponent<SimpleLabel>();
        label.label = name;
    }

    public void SpawnOrcArmy()
    {
        if (state.orcArmySpawned) return;
        state.orcArmySpawned = true;
        UIController.GetInstance()?.ShowNotification("Орки собираются в армии. Орочья армия приближается!");

        // spawn some orc enemies near forest (simple implementation)
        Vector3 spawnCenter = new Vector3(-10f, 0f, 0f);
        int count = 12;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = spawnCenter + Random.insideUnitSphere * 8f;
            pos.y = 1f;
            GameObject prefab = Resources.Load<GameObject>($"{enemiesResourcePath}/Orc");
            if (prefab != null)
            {
                var go = Instantiate(prefab, pos, Quaternion.identity);
                var e = go.GetComponent<Enemy>();
                if (e != null) { e.Initialize("Orc", 3); }
                else { var se = go.AddComponent<SimpleEnemy>(); se.Initialize("Orc", 3); }
            }
            else
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.transform.position = pos;
                go.name = "Orc_Fallback";
                var se = go.AddComponent<SimpleEnemy>();
                se.Initialize("Orc", 3);
            }
        }

        UIController.GetInstance()?.ShowNotification("Орки прибыли к границам Джуры.");
        SaveState();
    }

    public void SpawnOgres(int count = 2, Vector3? center = null)
    {
        Vector3 spawnCenter = center ?? new Vector3(-5f, 0f, 10f);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = spawnCenter + Random.insideUnitSphere * 4f;
            pos.y = 1f;
            GameObject prefab = Resources.Load<GameObject>($"{enemiesResourcePath}/Ogre");
            if (prefab != null)
            {
                var go = Instantiate(prefab, pos, Quaternion.identity);
                var e = go.GetComponent<Enemy>();
                if (e != null) e.Initialize("Ogre", 5);
                else { var og = go.AddComponent<Ogre>(); og.Initialize("Ogre", 5); }
            }
            else
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.localScale = Vector3.one * 2.5f;
                go.transform.position = pos;
                go.name = "Ogre_Fallback";
                var og = go.AddComponent<Ogre>();
                og.Initialize("Ogre", 5);
            }
        }

        UIController.GetInstance()?.ShowNotification($"Появились огры в округе ({count}).");
    }

    public void TriggerFalmosAttack()
    {
        if (!state.isSettlementCreated) return;
        if (state.falmosAttackTriggered) return;
        state.falmosAttackTriggered = true;
        UIController.GetInstance()?.ShowNotification("Фалмос отправил армию против Темпеста!");
        // For prototype: spawn strong enemies near settlement
        SpawnOrcArmy();
        SaveState();
    }

    public void TriggerEvolution(string evoType)
    {
        state.playerEvolution = evoType;
        UIController.GetInstance()?.ShowNotification($"Игрок эволюционировал: {evoType}");
        // unlock associated skills
        if (evoType == "Divine")
        {
            SkillManager.Instance?.Unlock("DivineRegeneration");
            SkillManager.Instance?.Unlock("StormForm");
        }
        else if (evoType == "Demon")
        {
            SkillManager.Instance?.Unlock("Corruption");
        }
        SaveState();
    }

    // Simple Save/Load via PlayerPrefs JSON
    public void SaveState()
    {
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString("WorldState", json);
        PlayerPrefs.Save();
    }

    public void LoadState()
    {
        if (PlayerPrefs.HasKey("WorldState"))
        {
            string json = PlayerPrefs.GetString("WorldState");
            try { state = JsonUtility.FromJson<WorldState>(json); }
            catch { state = new WorldState(); }
        }
    }
}
