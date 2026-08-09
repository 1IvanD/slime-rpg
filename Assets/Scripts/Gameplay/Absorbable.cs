using UnityEngine;

public class Absorbable : MonoBehaviour
{
    public string resourceId = "hipokute"; // matches ItemsDatabase ids
    public int amount = 1;

    public void OnAbsorbed(PlayerAbilities by)
    {
        if (by == null) return;
        var stomach = by.GetComponent<StomachInventory>();
        if (stomach != null)
        {
            stomach.AddMaterial(resourceId, amount);
            UIController.GetInstance()?.ShowNotification($"Поглощено: {resourceId} x{amount}");
        }
        else
        {
            // fallback: add gold
            EconomySystem.Instance?.AddGold(10f * amount);
            UIController.GetInstance()?.ShowNotification($"Поглощено и конвертировано: {resourceId}");
        }

        Destroy(gameObject);
    }
}
