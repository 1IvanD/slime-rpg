using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI historyText;

    private ItemsDatabase db;

    private void Start()
    {
        db = ItemsDatabase.Instance;
    }

    public void OnSend()
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text)) return;
        var msg = inputField.text.Trim();
        inputField.text = "";

        if (msg.StartsWith("/t "))
        {
            // telepathy
            var player = FindObjectOfType<Player>();
            bool hasTele = SkillManager.Instance?.IsUnlocked("Telepathy") ?? false;
            if (!hasTele)
            {
                AddHistoryLine("Система: У вас нет навыка телепатии.");
                return;
            }
            string body = msg.Substring(3);
            var comps = FindObjectsOfType<Companion>();
            foreach (var c in comps)
            {
                // optional: filter by owner or distance
                c.ReceiveTelepathicMessage(body);
            }
            AddHistoryLine($"(telepathy) you -> servants: {body}");
        }
        else
        {
            // ask Great Sage
            var hasGreat = SkillManager.Instance?.IsUnlocked("GreatSage") ?? false;
            string reply = GreatSageResponder.Instance.Ask(msg, hasGreat);
            AddHistoryLine("You: " + msg);
            AddHistoryLine("GreatSage: " + reply);
        }
    }

    private void AddHistoryLine(string line)
    {
        if (historyText == null) return;
        historyText.text += "\n" + line;
    }
}
