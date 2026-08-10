using UnityEngine;

[DisallowMultipleComponent]
public class PredatorForm : MonoBehaviour
{
    public bool inPredatorForm = false;
    public float predatorDuration = 10f;
    public float damageMultiplier = 1.5f;

    private float timer = 0f;
    private PlayerAbilities abilities;

    private void Start()
    {
        abilities = GetComponent<PlayerAbilities>();
    }

    private void Update()
    {
        if (inPredatorForm)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) ExitForm();
        }
    }

    public void EnterForm(float duration)
    {
        inPredatorForm = true;
        timer = duration > 0 ? duration : predatorDuration;
        // simple effect: unlock absorb if not
        if (abilities != null) abilities.UnlockAbsorb();
        UIController.GetInstance()?.ShowNotification("Entered Predator Form");
    }

    public void ExitForm()
    {
        inPredatorForm = false;
        UIController.GetInstance()?.ShowNotification("Predator Form ended");
    }

    // Absorb target: if target is EnemyBehaviour, gain XP/materials
    public void Absorb(GameObject target)
    {
        if (target == null) return;
        var enemy = target.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            // grant XP and small chance to learn skill
            var player = GetComponent<Player>();
            if (player != null) player.AddExperience(enemy.xpReward * 0.5f);
            Destroy(enemy.gameObject);
            UIController.GetInstance()?.ShowNotification("Absorbed creature: gained essence");
        }
    }
}
