using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AlchemyStation : MonoBehaviour
{
    [Tooltip("Queue size for pending recipes")]
    public int maxQueue = 8;

    private Queue<RecipeSO> queue = new Queue<RecipeSO>();
    private bool isProcessing = false;

    [Tooltip("Optional: assign an inventory manager or container to receive outputs (not required)")]
    public MonoBehaviour outputReceiver; // placeholder for integration with inventory system

    public void EnqueueRecipe(RecipeSO recipe)
    {
        if (recipe == null) return;
        if (queue.Count >= maxQueue)
        {
            Debug.LogWarning("AlchemyStation: queue full");
            return;
        }
        queue.Enqueue(recipe);
        Debug.Log($"AlchemyStation: Enqueued recipe {recipe.displayName}");
        if (!isProcessing) StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;
        while (queue.Count > 0)
        {
            var r = queue.Dequeue();
            Debug.Log($"AlchemyStation: Starting craft {r.displayName} (time {r.craftTime}s)");
            float t = 0f;
            while (t < r.craftTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            CompleteRecipe(r);
            yield return null;
        }
        isProcessing = false;
    }

    private void CompleteRecipe(RecipeSO r)
    {
        // produce outputs — integration point for inventory
        foreach (var o in r.outputs)
        {
            Debug.Log($"AlchemyStation: Produced {o.amount}x {o.itemId}");
            // TODO: add to inventory or spawn pickup entity
        }
    }
}
