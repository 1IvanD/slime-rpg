using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueUIController : MonoBehaviour
{
    public GameObject rootPanel;
    public Text speakerNameText;
    public Image speakerIconImage;
    public Text dialogueText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab; // simple button prefab with Text inside

    private List<GameObject> spawnedChoices = new List<GameObject>();

    private void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
    }

    public void ShowNode(DialogueNodeSO node)
    {
        if (rootPanel == null) return;
        rootPanel.SetActive(true);
        speakerNameText.text = node.speakerName;
        speakerIconImage.sprite = node.speakerIcon;
        dialogueText.text = node.text;

        // clear old choices
        foreach (var c in spawnedChoices) Destroy(c);
        spawnedChoices.Clear();

        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var ch in node.choices)
            {
                var go = Instantiate(choiceButtonPrefab, choicesContainer);
                var txt = go.GetComponentInChildren<Text>();
                txt.text = ch.text;
                var btn = go.GetComponent<UnityEngine.UI.Button>();
                btn.onClick.AddListener(() => OnChoiceClicked(ch));
                spawnedChoices.Add(go);
            }
        }
        else
        {
            // no choices: show single continue button
            var go = Instantiate(choiceButtonPrefab, choicesContainer);
            var txt = go.GetComponentInChildren<Text>();
            txt.text = "Continue";
            var btn = go.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(() => OnContinue());
            spawnedChoices.Add(go);
        }
    }

    private void OnChoiceClicked(DialogueChoice ch)
    {
        DialogueManager.Instance?.ChooseChoice(ch);
    }

    private void OnContinue()
    {
        DialogueManager.Instance?.EndDialogue();
    }

    public void Hide()
    {
        if (rootPanel == null) return;
        rootPanel.SetActive(false);
        foreach (var c in spawnedChoices) Destroy(c);
        spawnedChoices.Clear();
    }
}
