using UnityEngine;

public class VeldoraNPC : MonoBehaviour
{
    public bool isSealed = true;
    public string npcName = "Veldora";

    private bool befriended = false;

    private void Start()
    {
        // Create simple visual if none
        if (GetComponent<Renderer>() == null)
        {
            var rend = gameObject.AddComponent<MeshRenderer>();
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = CreateCubeMesh();
        }
    }

    private Mesh CreateCubeMesh()
    {
        return GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            // Show interaction prompt
            UIController.GetInstance()?.ShowNotification($"{npcName}: Я слышу тебя, маленькая слизь...");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation(player);
        }
    }

    private void StartConversation(Player player)
    {
        string msg = "О, маленькая слизь... Ты не боишься меня?\nЯ запечатан уже сотни лет. Если хочешь — поговори со мной.";
        string[] options = new string[] { "Поговорить", "Изучить барьер", "Уйти" };
        DialogueSystem.Instance.ShowDialog(msg, options, (idx) => {
            if (idx == 0) OnTalk(player);
            else if (idx == 1) OnInspectBarrier(player);
            else UIController.GetInstance()?.ShowNotification("Вы отходите от Вельдоры.");
        });
    }

    private void OnTalk(Player player)
    {
        string[] opts = new string[] { "Заключить договор дружбы", "Отказаться" };
        DialogueSystem.Instance.ShowDialog("Вельдора: Я одинок и хочу друга. Поможешь ли ты мне?", opts, (i) => {
            if (i == 0) BecomeFriend(player);
            else Decline(player);
        });
    }

    private void OnInspectBarrier(Player player)
    {
        UIController.GetInstance()?.ShowNotification("Барьер выглядит древним. Требуется источник магической энергии.");
    }

    private void BecomeFriend(Player player)
    {
        befriended = true;
        // Apply benefits: simple regen buff via Player component
        player.startingHealth *= 1.2f;
        player.GetStats().MaxHealth = player.startingHealth;
        player.GetStats().Health = player.startingHealth;
        UIController.GetInstance()?.ShowNotification("Вы заключили договор с Вельдорой. Регенерация усилена. Открылся доступ к особым квестам.");
        QuestManager.Instance?.AddQuest("FindMagicSource", "Найти источник магических колебаний в пещере");
    }

    private void Decline(Player player)
    {
        UIController.GetInstance()?.ShowNotification("Вы отказались помогать Вельдоре. Он остаётся в пещере.");
        QuestManager.Instance?.AddQuest("FindMagicSource", "Найти источник магических колебаний в пещере");
        // Mark world to be more dangerous (flag in GameManager)
        // For now just log
        Debug.Log("Player declined friendship: world will be more dangerous later.");
    }
}
