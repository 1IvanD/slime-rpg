using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsUIController : MonoBehaviour
{
    public GameObject rootPanel;
    public Transform listContainer;
    public GameObject entryPrefab; // simple prefab with Text

    private void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        if (entryPrefab == null)
        {
            // try load default prefab
            entryPrefab = Resources.Load<GameObject>("Prefabs/ChoiceButton");
        }
    }

    private void Start()
    {
    }

    public void Refresh()
    {
        if (listContainer == null) return;
        foreach (Transform t in listContainer) Destroy(t.gameObject);

        if (SkillManager.Instance == null)
        {
            var go = new GameObject("NoSkills");
            go.transform.SetParent(listContainer, false);
            var txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = "No skills available.";
            txt.color = Color.white;
            return;
        }

        var learned = SkillManager.Instance.GetLearnedSkills();
        if (learned == null || learned.Count == 0)
        {
            var go = new GameObject("NoSkills");
            go.transform.SetParent(listContainer, false);
            var txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = "No skills learned yet.";
            txt.color = Color.white;
            return;
        }

        foreach (var s in learned)
        {
            if (entryPrefab != null)
            {
                var go = Instantiate(entryPrefab, listContainer);
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = $"{s.displayName} - {s.description}";
            }
            else
            {
                var go = new GameObject(s.id);
                go.transform.SetParent(listContainer, false);
                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = $"{s.displayName} - {s.description}";
                txt.color = Color.white;
            }
        }
    }
}
