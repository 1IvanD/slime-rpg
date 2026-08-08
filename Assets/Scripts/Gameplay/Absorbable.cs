using UnityEngine;

public class Absorbable : MonoBehaviour
{
    public string resourceName = "Organic Material";
    public float resourceValue = 10f; // fallback value to reward
    public int amount = 1;

    // Called by PlayerAbilities.TryAbsorb
    public void OnAbsorbed(PlayerAbilities by)
    {
        // Simple placeholder: convert absorbed resource to gold via EconomySystem
        if (EconomySystem.Instance != null)
        {
            EconomySystem.Instance.AddGold(resourceValue * amount);
        }

        // Optionally trigger other logic (inventory, quests)
        Destroy(gameObject);
    }
}
