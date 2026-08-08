using UnityEngine;

// Магический зверь — может получать имя при взаимодействии (E)
public class MagicalBeast : Enemy
{
    public string displayName = "";
    public string uniqueId = ""; // persistent id for saving
    private bool playerNearby = false;

    private void Start()
    {
        if (string.IsNullOrEmpty(displayName))
            displayName = enemyType;
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = System.Guid.NewGuid().ToString();

        UpdateObjectName();

        // Ensure there's a trigger collider for interaction detection
        var col = GetComponent<Collider>();
        if (col == null)
        {
            var c = gameObject.AddComponent<SphereCollider>();
            c.isTrigger = true;
            ((SphereCollider)c).radius = 1.5f;
        }
    }

    private void UpdateObjectName()
    {
        gameObject.name = string.IsNullOrEmpty(displayName) ? enemyType : displayName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null) playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null) playerNearby = false;
    }

    private void Update()
    {
        // Interaction key handled here (simple approach)
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PromptNameInput();
        }
    }

    public void PromptNameInput()
    {
        UIController.GetInstance()?.ShowNameInput($"Дайте имя {enemyType}", (name) =>
        {
            SetName(string.IsNullOrEmpty(name) ? enemyType : name);
            // register name in world state if manager exists
            StoryEventManager.Instance?.RegisterNamedBeast(uniqueId, displayName, enemyType, transform.position);
        });
    }

    public void SetName(string name)
    {
        displayName = name;
        UpdateObjectName();

        // find label and update if exists
        var label = GetComponentInChildren<SimpleLabel>();
        if (label != null) label.SetLabel(displayName);
    }

    public override void Die()
    {
        UIController.GetInstance()?.ShowNotification($"Погиб: {displayName}");
        base.Die();
    }
}
