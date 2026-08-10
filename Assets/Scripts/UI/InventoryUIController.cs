using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class InventoryUIController : MonoBehaviour
{
    public GameObject rootPanel;
    public Transform listContainer;
    public GameObject listItemPrefab; // simple prefab with Text component

    private void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
    }

    public void Toggle()
    {
        if (rootPanel == null) return;
        bool active = !rootPanel.activeSelf;
        rootPanel.SetActive(active);
        if (active) Refresh();
    }

    public void Refresh()
    {
        if (listContainer == null || listItemPrefab == null) return;
        foreach (Transform t in listContainer) Destroy(t.gameObject);

        var inv = InventorySystem.Instance;
        if (inv == null) return;
        var data = inv.GetInventory();
        foreach (var kv in data)
        {
            var go = Instantiate(listItemPrefab, listContainer);
            var txt = go.GetComponentInChildren<Text>();
            if (txt != null)
            {
                var item = kv.Value;
                txt.text = $"{item.itemName} x{item.quantity} ({item.rarity})";
            }
        }
    }
}
