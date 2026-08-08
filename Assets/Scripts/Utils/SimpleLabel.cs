using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class SimpleLabel : MonoBehaviour
{
    [SerializeField] private string _label = "";
    private GameObject labelGO;
    private TextMeshPro labelTMP;

    public string Label { get => _label; set => SetLabel(value); }

    private void Start()
    {
        CreateLabelObject();
    }

    private void CreateLabelObject()
    {
        if (labelGO != null) return;
        labelGO = new GameObject("Label");
        labelGO.transform.SetParent(transform, false);
        labelGO.transform.localPosition = Vector3.up * 1.6f;

        labelTMP = labelGO.AddComponent<TextMeshPro>();
        labelTMP.text = _label;
        labelTMP.fontSize = 2;
        labelTMP.alignment = TextAlignmentOptions.Center;
        labelTMP.color = Color.white;
        labelTMP.enableCulling = true;
    }

    public void SetLabel(string t)
    {
        _label = t;
        if (labelTMP == null) CreateLabelObject();
        labelTMP.text = t;
    }

    private void Update()
    {
        if (labelGO == null) return;
        if (Camera.main != null)
            labelGO.transform.rotation = Quaternion.LookRotation(labelGO.transform.position - Camera.main.transform.position);
    }
}
