using UnityEngine;

[DisallowMultipleComponent]
public class NPCDialogueController : MonoBehaviour
{
    public DialogueTreeSO dialogue;
    public string startNodeId;

    public bool showActionMenuOnApproach = true;
    public string[] actionOptions = new string[] { "Greet", "Talk", "Attack" };

    private void OnTriggerStay(Collider other)
    {
        if (!showActionMenuOnApproach) return;
        if (other.CompareTag("Player"))
        {
            // Here we would show a simple action menu — for now, press E to open Talk
            if (Input.GetKeyDown(KeyCode.E))
            {
                // show action selection (simple implementation)
                ShowActionMenu();
            }
        }
    }

    private void ShowActionMenu()
    {
        // For MVP: open dialogue tree immediately mapped to Talk option
        // Future: show UI overlay with options
        if (dialogue != null)
            DialogueManager.Instance?.StartDialogue(dialogue, startNodeId);
    }
}
