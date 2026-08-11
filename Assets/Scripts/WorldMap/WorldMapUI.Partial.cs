using System.Collections;
using UnityEngine;

public partial class WorldMapUI : MonoBehaviour
{
    private Coroutine notificationRoutine;

    public void ShowNotification(string message, float duration = 3f)
    {
        if (panel == null) return;
        panelText.text = message;
        panel.SetActive(true);
        if (notificationRoutine != null) StopCoroutine(notificationRoutine);
        notificationRoutine = StartCoroutine(HideAfter(duration));
    }

    private IEnumerator HideAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        HideConfirm();
        notificationRoutine = null;
    }
}
