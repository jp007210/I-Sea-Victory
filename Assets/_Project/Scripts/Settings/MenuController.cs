using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup slotSelectionCanvasGroup;
    public CanvasGroup loadingScreenCanvasGroup;
    public CanvasGroup optionsPanelCanvasGroup;
    public CanvasGroup audioOptionsCanvasGroup;
    public CanvasGroup videoOptionsCanvasGroup;
    public CanvasGroup controlsOptionsCanvasGroup;
    public CanvasGroup creditsPanelCanvasGroup;

    public TMP_Text[] slotStatusTexts;

    public string gameSceneName = "GameScene";
    public float fadeDuration = 0.5f;

    void Start()
    {
        if (logoCanvasGroup != null)
        {
            logoCanvasGroup.alpha = 1;
            logoCanvasGroup.interactable = true;
            logoCanvasGroup.blocksRaycasts = true;
        }
        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.alpha = 1;
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }
        if (slotSelectionCanvasGroup != null)
        {
            slotSelectionCanvasGroup.alpha = 0;
            slotSelectionCanvasGroup.interactable = false;
            slotSelectionCanvasGroup.blocksRaycasts = false;
        }
        if (loadingScreenCanvasGroup != null)
        {
            loadingScreenCanvasGroup.alpha = 0;
            loadingScreenCanvasGroup.interactable = false;
            loadingScreenCanvasGroup.blocksRaycasts = false;
        }
        if (optionsPanelCanvasGroup != null)
        {
            optionsPanelCanvasGroup.alpha = 0;
            optionsPanelCanvasGroup.interactable = false;
            optionsPanelCanvasGroup.blocksRaycasts = false;
        }
        if (audioOptionsCanvasGroup != null)
        {
            audioOptionsCanvasGroup.alpha = 0;
            audioOptionsCanvasGroup.interactable = false;
            audioOptionsCanvasGroup.blocksRaycasts = false;
        }
        if (videoOptionsCanvasGroup != null)
        {
            videoOptionsCanvasGroup.alpha = 0;
            videoOptionsCanvasGroup.interactable = false;
            videoOptionsCanvasGroup.blocksRaycasts = false;
        }
        if (controlsOptionsCanvasGroup != null)
        {
            controlsOptionsCanvasGroup.alpha = 0;
            controlsOptionsCanvasGroup.interactable = false;
            controlsOptionsCanvasGroup.blocksRaycasts = false;
        }
        if (creditsPanelCanvasGroup != null)
        {
            creditsPanelCanvasGroup.alpha = 0;
            creditsPanelCanvasGroup.interactable = false;
            creditsPanelCanvasGroup.blocksRaycasts = false;
        }

        UpdateSlotInformation();
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, bool enableInteraction)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning($"CanvasGroup para fade é nulo. Operação abortada.");
            yield break;
        }
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = enableInteraction;
        canvasGroup.blocksRaycasts = enableInteraction;
    }

    public void ShowMainMenuWithFade()
    {
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 1, fadeDuration, true));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 1, fadeDuration, true));
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(creditsPanelCanvasGroup, creditsPanelCanvasGroup.alpha, 0, fadeDuration, false));
    }

    public void ShowSlotSelectionWithFade()
    {
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 1, fadeDuration, true));
        UpdateSlotInformation();
    }

    public void ShowLoadingScreenWithFade()
    {
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(loadingScreenCanvasGroup, loadingScreenCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void OpenOptionsPanel()
    {
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void OpenCreditsPanel()
    {
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(creditsPanelCanvasGroup, creditsPanelCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void ReturnToOptionsPanel()
    {
        StartCoroutine(FadeCanvasGroup(audioOptionsCanvasGroup, audioOptionsCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(videoOptionsCanvasGroup, videoOptionsCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(controlsOptionsCanvasGroup, controlsOptionsCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void ShowAudioOptionsPanel()
    {
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(audioOptionsCanvasGroup, audioOptionsCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void ShowVideoOptionsPanel()
    {
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(videoOptionsCanvasGroup, videoOptionsCanvasGroup.alpha, 1, fadeDuration, true));
    }

    public void ShowControlsOptionsPanel()
    {
        StartCoroutine(FadeCanvasGroup(optionsPanelCanvasGroup, optionsPanelCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(controlsOptionsCanvasGroup, controlsOptionsCanvasGroup.alpha, 1, fadeDuration, true));
    }

    void UpdateSlotInformation()
    {
        for (int i = 0; i < slotStatusTexts.Length; i++)
        {
            if (slotStatusTexts[i] == null)
            {
                continue;
            }
            int slotIndex = i + 1;
            string hasSaveKey = "Slot" + slotIndex + "_HasSave";
            if (PlayerPrefs.HasKey(hasSaveKey) && PlayerPrefs.GetInt(hasSaveKey) == 1)
            {
                int maxDay = PlayerPrefs.GetInt("Slot" + slotIndex + "_MaxDay", 0);
                int deaths = PlayerPrefs.GetInt("Slot" + slotIndex + "_Deaths", 0);
                slotStatusTexts[i].text = $"Melhor Dia: {maxDay}     Mortes: {deaths}";
            }
            else
            {
                slotStatusTexts[i].text = "NOVO JOGO";
            }
        }
    }

    public void SelectSlot(int slotNumber)
    {
        PlayerPrefs.SetInt("CurrentSelectedSlot", slotNumber);
        string hasSaveKey = "Slot" + slotNumber + "_HasSave";
        if (!PlayerPrefs.HasKey(hasSaveKey) || PlayerPrefs.GetInt(hasSaveKey) == 0)
        {
            PlayerPrefs.SetInt("Slot" + slotNumber + "_HasSave", 0);
            PlayerPrefs.SetInt("Slot" + slotNumber + "_MaxDay", 0);
            PlayerPrefs.SetInt("Slot" + slotNumber + "_Deaths", 0);
        }
        PlayerPrefs.Save();
        StartCoroutine(LoadGameAfterFade(gameSceneName));
    }

    IEnumerator LoadGameAfterFade(string sceneName)
    {
        ShowLoadingScreenWithFade();
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene("EnemyStage");
    }

    public void StartGameButton()
    {
        ShowSlotSelectionWithFade();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
