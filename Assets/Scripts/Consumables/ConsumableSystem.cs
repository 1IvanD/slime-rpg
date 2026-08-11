using UnityEngine;

[DisallowMultipleComponent]
public class ConsumableSystem : MonoBehaviour
{
    public static ConsumableSystem Instance { get; private set; }

    public AudioClip consumeSound;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool UseConsumable(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return false;
        var db = ItemsDatabase.Instance;
        var inv = InventorySystem.Instance;
        var player = FindObjectOfType<Player>();
        if (db == null || inv == null || player == null) return false;

        var so = db.GetItem(itemId);
        if (so == null) return false;

        if (so.effectType == EffectType.Heal && so.healAmount > 0f)
        {
            player.Heal(so.healAmount);
            inv.RemoveItem(itemId, 1);
            UIController.GetInstance()?.ShowNotification($"Использовано: {so.displayName} (+{so.healAmount} HP)");
            PlayConsumeSound();
            return true;
        }

        if (so.effectType == EffectType.Buff)
        {
            // placeholder: future buff implementation
            inv.RemoveItem(itemId, 1);
            UIController.GetInstance()?.ShowNotification($"Использовано (буфф): {so.displayName}");
            PlayConsumeSound();
            return true;
        }

        return false;
    }

    private void PlayConsumeSound()
    {
        if (consumeSound != null)
        {
            AudioSource.PlayClipAtPoint(consumeSound, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
        else
        {
            // try load from Resources
            var clip = Resources.Load<AudioClip>("Sounds/consume");
            if (clip != null) AudioSource.PlayClipAtPoint(clip, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
    }
}
