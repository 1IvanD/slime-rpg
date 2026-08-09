using System.Collections.Generic;
using UnityEngine;

public class StomachInventory : MonoBehaviour
{
    [System.Serializable]
    public class Stack { public string id; public int count; }

    private List<Stack> materials = new List<Stack>();

    // unlimited by default
    public bool unlimited = true;
    public int slotLimit = 1000; // if unlimited==false

    public void AddMaterial(string id, int count = 1)
    {
        if (string.IsNullOrEmpty(id) || count <= 0) return;
        var s = materials.Find(x => x.id == id);
        if (s != null) s.count += count;
        else
        {
            if (!unlimited && materials.Count >= slotLimit) { Debug.LogWarning("Stomach full"); return; }
            materials.Add(new Stack { id = id, count = count });
        }
        Debug.Log($"Stomach: added {count}x {id}");
    }

    public bool HasMaterial(string id, int count)
    {
        var s = materials.Find(x => x.id == id);
        return s != null && s.count >= count;
    }

    public bool ConsumeMaterial(string id, int count)
    {
        var s = materials.Find(x => x.id == id);
        if (s == null || s.count < count) return false;
        s.count -= count;
        if (s.count <= 0) materials.Remove(s);
        return true;
    }

    public List<Stack> GetAll() => new List<Stack>(materials);

    public void Clear() { materials.Clear(); }
}
