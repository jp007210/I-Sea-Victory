using UnityEngine;
using System.Collections;

public class OptionsPanelController : MonoBehaviour
{
    [Header("Panel References")]
    public CanvasGroup mainOptionsPanel;
    public CanvasGroup audioOptionsPanel;
    public CanvasGroup videoOptionsPanel;
    public CanvasGroup controlsOptionsPanel;

    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;

    public void ShowAudioPanel()
    {
        StartCoroutine(FadePanels(mainOptionsPanel, audioOptionsPanel));
    }

    public void ShowVideoPanel()
    {
        StartCoroutine(FadePanels(mainOptionsPanel, videoOptionsPanel));
    }

    public void ShowControlsPanel()
    {
        StartCoroutine(FadePanels(mainOptionsPanel, controlsOptionsPanel));
    }

    public void ReturnToMainOptions()
    {
        if (audioOptionsPanel.alpha > 0) StartCoroutine(FadePanels(audioOptionsPanel, mainOptionsPanel));
        if (videoOptionsPanel.alpha > 0) StartCoroutine(FadePanels(videoOptionsPanel, mainOptionsPanel));
        if (controlsOptionsPanel.alpha > 0) StartCoroutine(FadePanels(controlsOptionsPanel, mainOptionsPanel));
    }

    private IEnumerator FadePanels(CanvasGroup from, CanvasGroup to)
    {
        from.interactable = false;
        from.blocksRaycasts = false;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            from.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        from.alpha = 0;

        to.interactable = true;
        to.blocksRaycasts = true;
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            to.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        to.alpha = 1;
    }
}
