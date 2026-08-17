using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

namespace Tempest.Save
{
    /// <summary>
    /// Canonical SaveManager: handles full game state serialization (JSON + versioning).
    /// Replaces legacy PlayerPrefs-only approach.
    /// Integrates with Quest, NPC, War, Settlement, and Player systems.
    /// 
    /// Usage:
    ///   SaveGame(slotName)    - Save entire game state to JSON
    ///   LoadGame(slotName)    - Load and apply entire game state from JSON
    ///   GetSaveFiles()        - Get list of available saves for UI
    ///   DeleteSave(slotName)  - Delete a save file
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        // Save file configuration
        private static readonly string SAVE_DIR = Application.persistentDataPath + "/Saves/";
        private static readonly string SAVE_FILE_TEMPLATE = "SaveGame_{0}.json";
        private static readonly string BACKUP_DIR = Application.persistentDataPath + "/Backups/";
        private const int SAVE_VERSION = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure directories exist
            if (!Directory.Exists(SAVE_DIR))
                Directory.CreateDirectory(SAVE_DIR);

            if (!Directory.Exists(BACKUP_DIR))
                Directory.CreateDirectory(BACKUP_DIR);

            Debug.Log($"[SaveManager] Initialized. Save directory: {SAVE_DIR}");
        }

        #region Public API

        /// <summary>
        /// Save entire game state to JSON file with auto-backup.
        /// </summary>
        /// <param name="slotName">Save slot name (default: "auto")</param>
        /// <returns>True if save succeeded, false otherwise</returns>
        public bool SaveGame(string slotName = "auto")
        {
            try
            {
                WorldState state = GatherWorldState();
                string savePath = Path.Combine(SAVE_DIR, string.Format(SAVE_FILE_TEMPLATE, slotName));

                // Create backup if file exists
                if (File.Exists(savePath))
                {
                    CreateBackup(savePath, slotName);
                }

                // Serialize to JSON with pretty printing
                string json = JsonUtility.ToJson(state, prettyPrint: true);
                File.WriteAllText(savePath, json);

                Debug.Log($"[SaveManager] ✓ Game saved: '{slotName}' → {savePath} ({new System.IO.FileInfo(savePath).Length} bytes)");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveManager] ✗ Failed to save game '{slotName}': {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Load entire game state from JSON file and apply it to all systems.
        /// </summary>
        /// <param name="slotName">Save slot name to load (default: "auto")</param>
        /// <returns>True if load succeeded, false otherwise</returns>
        public bool LoadGame(string slotName = "auto")
        {
            try
            {
                string savePath = Path.Combine(SAVE_DIR, string.Format(SAVE_FILE_TEMPLATE, slotName));

                if (!File.Exists(savePath))
                {
                    Debug.LogWarning($"[SaveManager] Save file not found: '{slotName}' at {savePath}");
                    return false;
                }

                string json = File.ReadAllText(savePath);
                WorldState state = JsonUtility.FromJson<WorldState>(json);

                if (state == null)
                {
                    Debug.LogError($"[SaveManager] Failed to deserialize WorldState from: {slotName}");
                    return false;
                }

                // Check version and migrate if necessary
                if (state.version != SAVE_VERSION)
                {
                    Debug.LogWarning($"[SaveManager] Version mismatch: save v{state.version}, current v{SAVE_VERSION}. Attempting migration...");
                    if (!MigrateWorldState(state, state.version, SAVE_VERSION))
                    {
                        Debug.LogError("[SaveManager] Migration failed. Aborting load.");
                        return false;
                    }
                }

                ApplyWorldState(state);
                Debug.Log($"[SaveManager] ✓ Game loaded: '{slotName}'");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveManager] ✗ Failed to load game '{slotName}': {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// List all available save files with metadata.
        /// </summary>
        /// <returns>List of SaveFileInfo objects</returns>
        public List<SaveFileInfo> GetSaveFiles()
        {
            List<SaveFileInfo> files = new List<SaveFileInfo>();

            if (!Directory.Exists(SAVE_DIR))
            {
                Debug.LogWarning($"[SaveManager] Save directory does not exist: {SAVE_DIR}");
                return files;
            }

            try
            {
                foreach (var filePath in Directory.GetFiles(SAVE_DIR, "*.json"))
                {
                    var fileInfo = new System.IO.FileInfo(filePath);
                    string slotName = Path.GetFileNameWithoutExtension(filePath);

                    files.Add(new SaveFileInfo(
                        slotName,
                        filePath,
                        fileInfo.Length,
                        fileInfo.LastWriteTime
                    ));
                }

                Debug.Log($"[SaveManager] Found {files.Count} save file(s)");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveManager] Error listing save files: {ex.Message}");
            }

            return files;
        }

        /// <summary>
        /// Delete a save file.
        /// </summary>
        /// <param name="slotName">Save slot name to delete</param>
        /// <returns>True if deletion succeeded, false otherwise</returns>
        public bool DeleteSave(string slotName)
        {
            try
            {
                string savePath = Path.Combine(SAVE_DIR, string.Format(SAVE_FILE_TEMPLATE, slotName));

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    Debug.Log($"[SaveManager] ✓ Save deleted: '{slotName}'");
                    return true;
                }

                Debug.LogWarning($"[SaveManager] Save file not found: '{slotName}'");
                return false;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to delete save '{slotName}': {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Private - State Gathering & Application

        /// <summary>
        /// Gather all game systems' current state into WorldState object.
        /// TODO: Integrate with all system managers (Quest, NPC, War, Settlement, Player).
        /// </summary>
        private WorldState GatherWorldState()
        {
            WorldState state = new WorldState
            {
                version = SAVE_VERSION,
                timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                sceneName = SceneManager.GetActiveScene().name
            };

            // TODO: Player state
            // TODO: Quest state
            // TODO: NPC affinity state
            // TODO: War state
            // TODO: Settlement state

            Debug.Log($"[SaveManager] WorldState gathered from scene: {state.sceneName}");
            return state;
        }

        /// <summary>
        /// Apply loaded WorldState to all game systems.
        /// TODO: Restore all systems from the provided state.
        /// </summary>
        private void ApplyWorldState(WorldState state)
        {
            // Load scene if different from current
            if (state.sceneName != SceneManager.GetActiveScene().name)
            {
                Debug.Log($"[SaveManager] Loading scene: {state.sceneName}");
                SceneManager.LoadScene(state.sceneName);
                // Note: Will need to call ApplyWorldState again after scene loads
                return;
            }

            // TODO: Restore player state
            // TODO: Restore quests
            // TODO: Restore NPC affinity
            // TODO: Restore war state
            // TODO: Restore settlements

            Debug.Log("[SaveManager] WorldState applied to all systems");
        }

        #endregion

        #region Private - Versioning & Migration

        /// <summary>
        /// Migrate save data between versions for backwards compatibility.
        /// </summary>
        private bool MigrateWorldState(WorldState state, int fromVersion, int toVersion)
        {
            if (fromVersion == toVersion)
            {
                return true;
            }

            Debug.Log($"[SaveManager] Migrating save data: v{fromVersion} → v{toVersion}");

            // Example v0 → v1 migration
            if (fromVersion < 1 && toVersion >= 1)
            {
                Debug.Log("[SaveManager] Applying v0→v1 migration: adding default race field");

                if (state.playerState != null && string.IsNullOrEmpty(state.playerState.race))
                {
                    state.playerState.race = "human"; // default race
                }
            }

            // Add more migration logic here for future versions
            // if (fromVersion < 2 && toVersion >= 2) { ... }

            state.version = toVersion;
            Debug.Log($"[SaveManager] Migration complete. Version now: {state.version}");
            return true;
        }

        #endregion

        #region Private - Backup Management

        /// <summary>
        /// Create a timestamped backup of the current save file.
        /// </summary>
        private void CreateBackup(string savePath, string slotName)
        {
            try
            {
                string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupPath = Path.Combine(BACKUP_DIR, $"{slotName}_backup_{timestamp}.json");

                File.Copy(savePath, backupPath, overwrite: true);
                Debug.Log($"[SaveManager] Backup created: {backupPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[SaveManager] Failed to create backup: {ex.Message}");
            }
        }

        #endregion
    }
}
