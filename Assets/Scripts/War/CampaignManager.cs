using System.Collections;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance { get; private set; }

    private WarEventSO[] events;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // load events from Resources/CampaignEvents
        events = Resources.LoadAll<WarEventSO>("CampaignEvents");
        if (events == null || events.Length == 0)
        {
            Debug.Log("CampaignManager: no WarEventSO found in Resources/CampaignEvents. Create them via Tools → Tempest → Generate Campaign Events.");
            return;
        }

        // sort by order
        events = events.OrderBy(e => e.order).ToArray();

        StartCoroutine(RunCampaign());
    }

    private IEnumerator RunCampaign()
    {
        Debug.Log($"CampaignManager: running {events.Length} events");
        for (int i = 0; i < events.Length; i++)
        {
            var ev = events[i];
            if (!ev.autoExecute) continue;

            UIController.GetInstance()?.ShowNotification($"Campaign: {ev.displayName}");
            Debug.Log($"CampaignManager: executing event {ev.id} ({ev.displayName}) in {ev.delayBeforeExecute}s");
            yield return new WaitForSeconds(ev.delayBeforeExecute);

            ExecuteEvent(ev);

            // small pause between events so simulation can update
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("CampaignManager: finished all events.");
    }

    private void ExecuteEvent(WarEventSO ev)
    {
        if (ev == null) return;

        // move all armies whose faction is in participantFactions to targetNodeId
        if (ev.participantFactions != null && ev.participantFactions.Length > 0)
        {
            foreach (var a in WarManager.Instance?.armies ?? new System.Collections.Generic.List<ArmyDef>())
            {
                foreach (var f in ev.participantFactions)
                {
                    if (!string.IsNullOrEmpty(a.faction) && a.faction.ToLower().Contains(f.ToLower()))
                    {
                        a.homeNodeId = ev.targetNodeId;
                    }
                }
            }
        }

        // if forced winner, apply direct control
        if (ev.forceWinner && !string.IsNullOrEmpty(ev.winnerFaction))
        {
            WarManager.Instance?.ForceControl(ev.targetNodeId, ev.winnerFaction);
            UIController.GetInstance()?.ShowNotification($"Event '{ev.displayName}' executed: {ev.winnerFaction} gains control of {ev.targetNodeId}");
            return;
        }

        // otherwise run a deterministic simulation for this node
        string winner = WarManager.Instance?.SimulateBattleAtNode(ev.targetNodeId, 1);
        UIController.GetInstance()?.ShowNotification($"Event '{ev.displayName}' resolved: winner = {winner}");
    }
}
