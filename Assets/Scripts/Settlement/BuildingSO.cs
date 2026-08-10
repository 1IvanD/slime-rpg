using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/Settlement/Building", fileName = "BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public string id;
    public string displayName;
    public int level = 1;
    public int maxLevel = 5;
    public float buildTime = 30f;
    public string[] workerRoles; // e.g., "Gatherer", "Blacksmith"
}
