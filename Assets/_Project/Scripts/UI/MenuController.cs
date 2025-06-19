using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MenuController : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup slotSelectionCanvasGroup;
    public CanvasGroup loadingScreenCanvasGroup;

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

        UpdateSlotInformation();
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, bool enableInteraction)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning($"CanvasGroup para fade é nulo. Operação abortada. Objeto: {canvasGroup?.gameObject.name}");
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

        if (loadingScreenCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(loadingScreenCanvasGroup, loadingScreenCanvasGroup.alpha, 0, fadeDuration, false));
        }
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

        if (loadingScreenCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(loadingScreenCanvasGroup, loadingScreenCanvasGroup.alpha, 1, fadeDuration, true));
        }
    }

    void UpdateSlotInformation()
    {
        for (int i = 0; i < slotStatusTexts.Length; i++)
        {
            if (slotStatusTexts[i] == null)
            {
                Debug.LogWarning($"SlotStatusText no índice {i} é nulo. Verifique as referências no Inspector.");
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
        Debug.Log($"Slot {slotNumber} selecionado. Preparando para carregar jogo...");

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

        SceneManager.LoadScene(sceneName);
    }

    public void StartGameButton()
    {
        ShowSlotSelectionWithFade();
    }

    public void OpenOptions()
    {
        Debug.Log("Abrindo opções... (Implementação futura de painel com fade)");
    }

    public void OpenCredits()
    {
        Debug.Log("Abrindo créditos... (Implementação futura de painel com fade)");
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}