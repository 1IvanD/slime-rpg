using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StomachPanel : MonoBehaviour
{
    public RectTransform materialsContent;
    public RectTransform recipesContent;
    public GameObject textRowPrefab;

    private StomachInventory stomach;
    private StomachCraftingSystem scs;
    private ItemsDatabase db;

    private void Start()
    {
        stomach = FindObjectOfType<StomachInventory>();
        scs = FindObjectOfType<StomachCraftingSystem>();
        db = ItemsDatabase.Instance;
        RefreshAll();
    }

    public void RefreshAll()
    {
        RefreshMaterials();
        RefreshRecipes();
    }

    public void RefreshMaterials()
    {
        if (materialsContent == null || stomach == null) return;
        foreach (Transform t in materialsContent) Destroy(t.gameObject);
        foreach (var s in stomach.GetAll())
        {
            var go = Instantiate(textRowPrefab, materialsContent);
            var txt = go.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = $"{s.id} x{s.count}";
        }
    }

    public void RefreshRecipes()
    {
        if (recipesContent == null || scs == null) return;
        foreach (Transform t in recipesContent) Destroy(t.gameObject);
        var list = scs.GetCraftableRecipes(true);
        foreach (var r in list)
        {
            var go = Instantiate(textRowPrefab, recipesContent);
            var txt = go.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = r.displayName + " (Craft)";
            var btn = go.AddComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(() => { scs.Craft(r); RefreshAll(); });
        }
    }
}
