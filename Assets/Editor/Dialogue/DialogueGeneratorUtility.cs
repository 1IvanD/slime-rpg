#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class DialogueGeneratorUtility
{
    private const string dir = "Assets/Data/Dialogue";

    [MenuItem("Tools/Tempest/Generate Example Dialogues")]
    public static void GenerateExample()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");
        if (!AssetDatabase.IsValidFolder(dir))
            AssetDatabase.CreateFolder("Assets/Data", "Dialogue");

        // create nodes
        var node1 = ScriptableObject.CreateInstance<DialogueNodeSO>();
        node1.id = "start";
        node1.speakerName = "Veldora";
        node1.text = "Что привело тебя в моё логово, странник?";

        var node2 = ScriptableObject.CreateInstance<DialogueNodeSO>();
        node2.id = "ask_help";
        node2.speakerName = "Veldora";
        node2.text = "Если ты ищешь силу, докажи своё достоинство. Я дам тебе задание.";
        node2.onEnterEffects = new DialogueEffect[] { new DialogueEffect { type = DialogueEffect.EffectType.StartQuest, paramId = "quest_veldora_test" } };

        var node3 = ScriptableObject.CreateInstance<DialogueNodeSO>();
        node3.id = "bye";
        node3.speakerName = "Veldora";
        node3.text = "Убирайся, путник.";

        // choices for node1
        node1.choices = new DialogueChoice[] {
            new DialogueChoice{ text = "I want power", targetNodeId = "ask_help", grantItem = false },
            new DialogueChoice{ text = "Just passing", targetNodeId = "bye" }
        };

        // create tree
        var tree = ScriptableObject.CreateInstance<DialogueTreeSO>();
        tree.id = "veldora_intro";
        tree.characterName = "Veldora";
        tree.nodes = new DialogueNodeSO[] { node1, node2, node3 };

        // save assets
        AssetDatabase.CreateAsset(node1, Path.Combine(dir, "Veldora_Node_Start.asset"));
        AssetDatabase.CreateAsset(node2, Path.Combine(dir, "Veldora_Node_AskHelp.asset"));
        AssetDatabase.CreateAsset(node3, Path.Combine(dir, "Veldora_Node_Bye.asset"));
        AssetDatabase.CreateAsset(tree, Path.Combine(dir, "Veldora_Tree.asset"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Generated example dialogue assets under Assets/Data/Dialogue. Attach DialogueTreeSO to NPC's NPCDialogueController component to test.");
    }
}
#endif
