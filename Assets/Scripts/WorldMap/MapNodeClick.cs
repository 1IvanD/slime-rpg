using UnityEngine;

// Simple click handler for MapNode objects in WorldMap scene. On click it shows travel confirmation via WorldMapUI.
[RequireComponent(typeof(MapNode))]
public class MapNodeClick : MonoBehaviour
{
    MapNode node;

    private void Awake()
    {
        node = GetComponent<MapNode>();
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<SphereCollider>();
            col.isTrigger = false;
            col.radius = 0.5f;
        }
    }

    private void OnMouseDown()
    {
        if (node == null) return;
        WorldMapUI.Instance?.ShowConfirm(node);
    }
}
