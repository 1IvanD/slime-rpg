using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class InventoryUIController : MonoBehaviour
{
    public GameObject rootPanel;
    public Transform listContainer;
    public GameObject listItemPrefab; // optional

    private void Awake()
    {
        if (rootPanel != null) rootPanel.SetActive(false);
        if (listItemPrefab == null)
        {
            listItemPrefab = Resources.Load<GameObject>("Prefabs/ChoiceButton");
        }
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
        if (listContainer == null) return;
        foreach (Transform t in listContainer) Destroy(t.gameObject);

        var inv = InventorySystem.Instance;
        if (inv == null) return;
        var data = inv.GetInventory();

        foreach (var kv in data)
        {
            var item = kv.Value;
            // create row
            GameObject row = new GameObject("ItemRow");
            row.transform.SetParent(listContainer, false);
            HorizontalLayoutGroup h = row.AddComponent<HorizontalLayoutGroup>();
            h.childForceExpandHeight = false;
            h.childForceExpandWidth = false;
            h.spacing = 8;

            // Name/Text
            GameObject nameGO = new GameObject("Name"); nameGO.transform.SetParent(row.transform, false);
            var txt = nameGO.AddComponent<TextMeshProUGUI>();
            txt.text = $"{item.itemName} x{item.quantity} ({item.rarity})";
            txt.color = Color.white;

            // If equipment, add Equip button
            if (item.category == ItemCategory.Artifact || item.category == ItemCategory.Weapon || item.category == ItemCategory.Armor)
            {
                GameObject btnGO = new GameObject("EquipBtn"); btnGO.transform.SetParent(row.transform, false);
                var img = btnGO.AddComponent<Image>(); img.color = new Color(0.2f, 0.4f, 0.2f, 0.9f);
                var btn = btnGO.AddComponent<Button>();
                RectTransform rt = btnGO.GetComponent<RectTransform>(); rt.sizeDelta = new Vector2(120, 28);

                GameObject btxt = new GameObject("Text"); btxt.transform.SetParent(btnGO.transform, false);
                var bt = btxt.AddComponent<TextMeshProUGUI>(); bt.text = "Equip"; bt.alignment = TextAlignmentOptions.Center; bt.color = Color.white; bt.fontSize = 14;

                // Capture local item id
                string id = item.itemId;
                btn.onClick.AddListener(() => {
                    bool ok = EquipmentManager.Instance != null && EquipmentManager.Instance.Equip(id);
                    if (ok) {
                        // remove one from inventory
                        InventorySystem.Instance?.RemoveItem(id, 1);
                        Refresh();
                    }
                });
            }
            else
            {
                // For consumables, add Use button
                if (item.category == ItemCategory.Potion || item.category == ItemCategory.Consumable)
                {
                    GameObject btnGO = new GameObject("UseBtn"); btnGO.transform.SetParent(row.transform, false);
                    var img = btnGO.AddComponent<Image>(); img.color = new Color(0.2f, 0.2f, 0.6f, 0.9f);
                    var btn = btnGO.AddComponent<Button>();
                    RectTransform rt = btnGO.GetComponent<RectTransform>(); rt.sizeDelta = new Vector2(120, 28);

                    GameObject btxt = new GameObject("Text"); btxt.transform.SetParent(btnGO.transform, false);
                    var bt = btxt.AddComponent<TextMeshProUGUI>(); bt.text = "Use"; bt.alignment = TextAlignmentOptions.Center; bt.color = Color.white; bt.fontSize = 14;

                    string id = item.itemId;
                    btn.onClick.AddListener(() => {
                        // Use effect: heal or other
                        if (InventorySystem.Instance != null)
                        {
                            var used = InventorySystem.Instance.UseItem(id);
                            if (used) UIController.GetInstance()?.ShowNotification($"Used: {item.itemName}");
                            Refresh();
                        }
                    });
                }
                else
                {
                    // simple placeholder: Sell button
                    GameObject btnGO = new GameObject("SellBtn"); btnGO.transform.SetParent(row.transform, false);
                    var img = btnGO.AddComponent<Image>(); img.color = new Color(0.4f, 0.2f, 0.2f, 0.9f);
                    var btn = btnGO.AddComponent<Button>();
                    RectTransform rt = btnGO.GetComponent<RectTransform>(); rt.sizeDelta = new Vector2(120, 28);

                    GameObject btxt = new GameObject("Text"); btxt.transform.SetParent(btnGO.transform, false);
                    var bt = btxt.AddComponent<TextMeshProUGUI>(); bt.text = "Sell"; bt.alignment = TextAlignmentOptions.Center; bt.color = Color.white; bt.fontSize = 14;

                    string id = item.itemId;
                    btn.onClick.AddListener(() => {
                        InventorySystem.Instance?.SellItem(id, 1);
                        Refresh();
                    });
                }
            }
        }
    }
}
