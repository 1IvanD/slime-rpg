using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsUIController : MonoBehaviour
{
    public GameObject rootPanel;
    public Transform listContainer;
    public GameObject entryPrefab; // simple prefab with Text

    private PlayerAbilities abilities;

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
        abilities = FindObjectOfType<Player>()?.GetComponent<PlayerAbilities>();
    }

    public void Refresh()
    {
        if (listContainer == null) return;
        foreach (Transform t in listContainer) Destroy(t.gameObject);

        if (abilities == null) abilities = FindObjectOfType<Player>()?.GetComponent<PlayerAbilities>();
        if (abilities == null)
        {
            var go = new GameObject("NoSkills");
            go.transform.SetParent(listContainer, false);
            var txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = "No skills available.";
            txt.color = Color.white;
            return;
        }

        // Create entries for known ability flags
        AddSkillEntry("Analyze", abilities.canAnalyze);
        AddSkillEntry("Absorb", abilities.canAbsorb);
        AddSkillEntry("Heal", abilities.canHeal);

        // Future: iterate dynamic skill list
    }

    private void AddSkillEntry(string name, bool unlocked)
    {
        if (entryPrefab != null)
        {
            var go = Instantiate(entryPrefab, listContainer);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) text.text = $"{name} - {(unlocked ? "Unlocked" : "Locked")}";
        }
        else
        {
            var go = new GameObject(name);
            go.transform.SetParent(listContainer, false);
            var txt = go.AddComponent<TextMeshProUGUI>();
            txt.text = $"{name} - {(unlocked ? "Unlocked" : "Locked")}";
            txt.color = Color.white;
        }
    }
}
