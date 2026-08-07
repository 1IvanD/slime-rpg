using System.Collections;
using UnityEngine;

public class InternalVoice : MonoBehaviour
{
    public float initialDelay = 1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(initialDelay);

        // System messages at awakening
        UIController ui = UIController.GetInstance();
        if (ui != null)
        {
            ui.ShowNotification("Вы переродились в неизвестной форме...\nАнализ... Завершено.\nТекущая форма: Слизь.");
            yield return new WaitForSeconds(3f);
            ui.ShowNotification("Доступны способности: Анализ, Поглощение, Исцеление. Используйте их, чтобы исследовать окружение.");
        }

        // Grant simple abilities via PlayerAbilities if player exists
        Player p = FindObjectOfType<Player>();
        if (p != null)
        {
            PlayerAbilities abilities = p.GetComponent<PlayerAbilities>();
            if (abilities == null) abilities = p.gameObject.AddComponent<PlayerAbilities>();
            abilities.UnlockAnalyze();
            abilities.UnlockAbsorb();
            abilities.UnlockHeal();
        }
    }
}
