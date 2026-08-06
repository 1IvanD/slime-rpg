using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemonWorldUI : MonoBehaviour
{
    private GameObject demonWorldPanel;
    private bool isDemonWorldOpen = false;

    private void Start()
    {
        CreateDemonWorldPanel();
    }

    private void CreateDemonWorldPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        demonWorldPanel = new GameObject("DemonWorldPanel");
        demonWorldPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = demonWorldPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelBg = demonWorldPanel.AddComponent<Image>();
        panelBg.color = new Color(0.05f, 0.02f, 0.08f, 0.95f);

        VerticalLayoutGroup layoutGroup = demonWorldPanel.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 20;
        layoutGroup.padding = new RectOffset(30, 30, 30, 30);
        layoutGroup.childForceExpandWidth = true;

        // Заголовок
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(demonWorldPanel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 70);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "🔥 МИР ДЕМОНОВ 🔥\n";
        titleText.fontSize = 52;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(1, 0.3f, 0);

        // Информация о регионах
        CreateRegionsList(demonWorldPanel.transform);

        // Информация о лордах демонов
        CreateDemonLordsList(demonWorldPanel.transform);

        demonWorldPanel.SetActive(false);
    }

    private void CreateRegionsList(Transform parent)
    {
        GameObject regionsObj = new GameObject("RegionsList");
        regionsObj.transform.SetParent(parent, false);

        RectTransform regionsRect = regionsObj.AddComponent<RectTransform>();
        regionsRect.sizeDelta = new Vector2(0, 250);

        VerticalLayoutGroup layoutGroup = regionsObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 8;
        layoutGroup.childForceExpandWidth = true;

        TextMeshProUGUI headerText = regionsObj.AddComponent<TextMeshProUGUI>();
        headerText.text = "РЕГИОНЫ АДА:";
        headerText.fontSize = 32;
        headerText.color = Color.yellow;

        var regions = DemonWorldManager.Instance.GetAllDemonRegions();
        foreach (var regionEntry in regions)
        {
            var region = regionEntry.Value;
            CreateRegionButton(regionsObj.transform, region);
        }
    }

    private void CreateRegionButton(Transform parent, DemonWorldManager.DemonWorldRegion region)
    {
        GameObject buttonObj = new GameObject(region.regionName);
        buttonObj.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(0, 40);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(region.dangerLevel / 10f, 0, 1 - (region.dangerLevel / 10f), 0.7f);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = $"{region.regionName} (Опасность: {region.dangerLevel}/10)";
        buttonText.fontSize = 18;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }

    private void CreateDemonLordsList(Transform parent)
    {
        GameObject lordsObj = new GameObject("DemonLordsList");
        lordsObj.transform.SetParent(parent, false);

        RectTransform lordsRect = lordsObj.AddComponent<RectTransform>();
        lordsRect.sizeDelta = new Vector2(0, 250);

        VerticalLayoutGroup layoutGroup = lordsObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 8;
        layoutGroup.childForceExpandWidth = true;

        TextMeshProUGUI headerText = lordsObj.AddComponent<TextMeshProUGUI>();
        headerText.text = "ДЕМОНИЧЕСКИЕ ЛОРДЫ:";
        headerText.fontSize = 32;
        headerText.color = new Color(1, 0.2f, 0.8f);

        var demons = DemonManager.Instance.GetAllDemons();
        int count = 0;
        foreach (var demonEntry in demons)
        {
            if (count >= 6) break;
            var demon = demonEntry.Value;
            CreateDemonLordButton(lordsObj.transform, demon);
            count++;
        }
    }

    private void CreateDemonLordButton(Transform parent, DemonLord demon)
    {
        GameObject buttonObj = new GameObject(demon.name);
        buttonObj.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(0, 40);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = GetRankColor(demon.rank);

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = $"👿 {demon.name} - {demon.title} [Сила: {demon.power}]";
        buttonText.fontSize = 16;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
    }

    private Color GetRankColor(DemonRank rank)
    {
        switch (rank)
        {
            case DemonRank.LowerDemon: return new Color(0.5f, 0.5f, 0.7f);
            case DemonRank.MidDemon: return new Color(0.7f, 0.3f, 0.7f);
            case DemonRank.UpperDemon: return new Color(1, 0.1f, 0.1f);
            case DemonRank.ArcDemon: return new Color(1, 0.3f, 0);
            case DemonRank.PrimordialDemon: return new Color(1, 0.7f, 0);
            case DemonRank.DemonLord: return new Color(1, 0, 0);
            default: return Color.white;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleDemonWorldPanel();
        }
    }

    private void ToggleDemonWorldPanel()
    {
        isDemonWorldOpen = !isDemonWorldOpen;
        demonWorldPanel.SetActive(isDemonWorldOpen);
        
        if (isDemonWorldOpen)
        {
            Time.timeScale = 0f;
            Debug.Log("Demon world panel opened");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Demon world panel closed");
        }
    }
}
