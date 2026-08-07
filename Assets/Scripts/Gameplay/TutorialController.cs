using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private Player player;
    private float timer = 0f;
    private int step = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        StartTutorial();
    }

    private void StartTutorial()
    {
        UIController.GetInstance()?.ShowNotification("Пробуждение... Используйте WASD, пробел — чтобы прыгнуть. Используйте F — Анализ, G — Поглощение, H — Исцеление.");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (step == 0 && timer > 5f)
        {
            UIController.GetInstance()?.ShowNotification("Попробуйте проанализировать предмет: подпрыгните и нажмите F.");
            step = 1;
        }
    }
}
