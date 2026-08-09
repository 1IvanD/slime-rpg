using UnityEngine;

public class Companion : MonoBehaviour
{
    public string companionName = "Servant";
    public Transform followTarget;
    public bool isFollower = true;
    public string ownerPlayerName = "Player";

    private void Update()
    {
        if (isFollower && followTarget != null)
        {
            // simple follow behavior
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, Time.deltaTime * 3f);
        }
    }

    public void ReceiveTelepathicMessage(string msg)
    {
        Debug.Log($"{companionName} received telepathy: {msg}");
        UIController.GetInstance()?.ShowNotification($"{companionName}: Принял сообщение.");

        var m = msg.ToLower();
        if (m.Contains("follow")) { isFollower = true; }
        else if (m.Contains("stay")) { isFollower = false; }
        else if (m.StartsWith("bring "))
        {
            // bring <materialId>
            string[] parts = msg.Split(' ');
            if (parts.Length >= 2) { string mat = parts[1]; TryBring(mat); }
        }
        else if (m.StartsWith("craft "))
        {
            string[] parts = msg.Split(' ');
            if (parts.Length >= 2) { string recipeId = parts[1]; TryCraft(recipeId); }
        }
    }

    private void TryBring(string materialId)
    {
        // naive: look for GameObjects with Absorbable matching id nearby and bring one
        var all = FindObjectsOfType<Absorbable>();
        foreach (var a in all)
        {
            if (a.resourceId == materialId && Vector3.Distance(a.transform.position, transform.position) < 8f)
            {
                var player = FindObjectOfType<Player>();
                var stomach = player?.GetComponent<StomachInventory>();
                if (stomach != null)
                {
                    stomach.AddMaterial(materialId, a.amount);
                    UIController.GetInstance()?.ShowNotification($"{companionName} принес: {materialId}");
                    Destroy(a.gameObject);
                    return;
                }
            }
        }
        UIController.GetInstance()?.ShowNotification($"{companionName}: Не нашёл {materialId} поблизости.");
    }

    private void TryCraft(string recipeId)
    {
        var player = FindObjectOfType<Player>();
        var sc = player?.GetComponent<StomachCraftingSystem>();
        if (sc == null) sc = FindObjectOfType<StomachCraftingSystem>();
        var db = ItemsDatabase.Instance;
        if (sc != null && db != null)
        {
            var r = db.GetRecipe(recipeId);
            if (r != null && sc.CanCraft(r))
            {
                sc.Craft(r);
                UIController.GetInstance()?.ShowNotification($"{companionName} скрафтил {r.displayName}");
                return;
            }
        }
        UIController.GetInstance()?.ShowNotification($"{companionName}: Не могу скрафтить {recipeId}");
    }
}
