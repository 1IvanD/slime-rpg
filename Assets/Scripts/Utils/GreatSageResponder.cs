using System.Collections.Generic;
using UnityEngine;

// Very small rule-based GreatSage responder used by ChatSystem
public class GreatSageResponder : MonoBehaviour
{
    public static GreatSageResponder Instance { get; private set; }

    private Dictionary<string, string> replies = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitReplies();
    }

    private void InitReplies()
    {
        replies["skill"] = "Великий Мудрец: Этот навык можно изучить путём анализа и практики.";
        replies["potion"] = "Великий Мудрец: Зелья готовятся из экстрактов и магического активационного агента.";
        replies["hipokute"] = "Великий Мудрец: Хипокуте — лекарственная трава, можно извлечь эссенцию.";
        replies["magic"] = "Великий Мудрец: Магическая руда может быть рафинирована в магистил.";
    }

    public string Ask(string question, bool hasGreatSage)
    {
        if (string.IsNullOrEmpty(question)) return "...";
        string q = question.ToLower();
        foreach (var kv in replies)
        {
            if (q.Contains(kv.Key))
            {
                return kv.Value + (hasGreatSage ? " (доп. анализ предоставлен Великим Мудрецом)." : "");
            }
        }

        // not found
        if (hasGreatSage && Random.value < 0.6f) return "Великий Мудрец делится наблюдениями: возможен частичный ответ.";
        return "Великий Мудрец: Я не могу ответить на этот вопрос прямо сейчас.";
    }
}
