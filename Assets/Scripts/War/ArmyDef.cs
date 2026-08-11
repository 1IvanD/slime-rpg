using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/War/Army", fileName = "ArmyDef")]
public class ArmyDef : ScriptableObject
{
    public string id;
    public string faction;
    public string homeNodeId;
    public int troopCount = 0;
    public int averageLevel = 1;
    public Color color = Color.gray;
}
