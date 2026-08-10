using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAbilities : MonoBehaviour
{
    public bool canAnalyze = false;
    public bool canAbsorb = false;
    public bool canHeal = false;

    public float analyzeRange = 3f;
    public float absorbRange = 2f;
    public int absorbAmount = 10;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void UnlockAnalyze() => canAnalyze = true;
    public void UnlockAbsorb() => canAbsorb = true;
    public void UnlockHeal() => canHeal = true;

    private void Update()
    {
        // Simple input hooks for testing
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryAnalyze();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TryAbsorb();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TryHeal();
        }
    }

    public void TryAnalyze()
    {
        if (!canAnalyze) return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, analyzeRange))
        {
            var stone = hit.collider.GetComponent<MagicStone>();
            if (stone != null)
            {
                UIController.GetInstance()?.ShowNotification($"Анализ: {stone.description}");
                return;
            }

            UIController.GetInstance()?.ShowNotification("Анализ: ничего интересного.");
        }
        else
        {
            UIController.GetInstance()?.ShowNotification("Нечего анализировать вблизи.");
        }
    }

    public void TryAbsorb()
    {
        if (!canAbsorb) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, absorbRange);
        foreach (var c in hits)
        {
            var absor = c.GetComponent<Absorbable>();
            if (absor != null)
            {
                absor.OnAbsorbed(this);
                UIController.GetInstance()?.ShowNotification($"Поглощено: {absor.resourceId}");
                return;
            }
        }
        UIController.GetInstance()?.ShowNotification("Нечего поглощать рядом.");
    }

    public void TryHeal()
    {
        if (!canHeal) return;
        player.Heal(10f);
        UIController.GetInstance()?.ShowNotification("Исцеление: +10 HP");
    }
}
