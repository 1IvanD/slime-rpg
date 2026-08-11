using UnityEngine;

[CreateAssetMenu(menuName = "Tempest/War/WarEvent", fileName = "WarEvent")]
public class WarEventSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    [Tooltip("Order in campaign (0 = first)")]
    public int order = 0;

    [Tooltip("MapNode id to which this event is targeted")]
    public string targetNodeId;

    [Tooltip("Factions that participate (evaluated by ArmyDef.faction and EnemyDef.faction)")]
    public string[] participantFactions;

    [Tooltip("If true, the event forces a winnerFaction instead of running the simulation")]
    public bool forceWinner = false;

    [Tooltip("When forceWinner is true, this faction will be set as winner for the event")]
    public string winnerFaction;

    [Tooltip("Delay in seconds before executing the event when it becomes active")]
    public float delayBeforeExecute = 2f;

    [Tooltip("If true the CampaignManager will auto‑execute this event in sequence")]
    public bool autoExecute = true;
}
