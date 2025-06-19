using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup slotSelectionCanvasGroup;
    public CanvasGroup loadingScreenCanvasGroup;

    // Refer�ncias aos Textos de status de cada slot
    public Text[] slotStatusTexts;

    // Nome da cena principal do seu jogo
    public string gameSceneName = "MainScene";

    // Dura��o da anima��o de fade em segundos
    public float fadeDuration = 0.5f; // Meio segundo para o fade

    void Start()
    {
        // Garante que o menu principal esteja ativo e os outros pain�is desativados ao iniciar
        // Inicializa o estado dos CanvasGroups
        logoCanvasGroup.alpha = 1;
        logoCanvasGroup.interactable = true;
        logoCanvasGroup.blocksRaycasts = true;

        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.interactable = true;
        mainMenuCanvasGroup.blocksRaycasts = true;

        slotSelectionCanvasGroup.alpha = 0; // Come�a invis�vel
        slotSelectionCanvasGroup.interactable = false; // N�o interage
        slotSelectionCanvasGroup.blocksRaycasts = false; // N�o bloqueia cliques

        if (loadingScreenCanvasGroup != null) // Para evitar erro se ainda n�o criou
        {
            loadingScreenCanvasGroup.alpha = 0;
            loadingScreenCanvasGroup.interactable = false;
            loadingScreenCanvasGroup.blocksRaycasts = false;
        }

        // Atualiza a informa��o dos slots no in�cio para o caso de algum fade subsequente
        UpdateSlotInformation();
    }

    // --- Fun��es para Gerenciar a Visibilidade dos Pain�is com Fade ---

    // Fun��o gen�rica para realizar o fade de um CanvasGroup
    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, bool enableInteraction)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null; // Espera um frame
        }
        canvasGroup.alpha = endAlpha; // Garante que atinge o valor final

        // Ap�s o fade, ajusta interactable e blocksRaycasts
        canvasGroup.interactable = enableInteraction;
        canvasGroup.blocksRaycasts = enableInteraction;
    }

    public void ShowMainMenuWithFade()
    {
        // Fazer LogoPanel e ButtonContainer aparecerem
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 1, fadeDuration, true));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 1, fadeDuration, true));

        // Fazer SlotSelectionPanel desaparecer
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 0, fadeDuration, false));

        // Certifica-se que a tela de loading est� invis�vel
        if (loadingScreenCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(loadingScreenCanvasGroup, loadingScreenCanvasGroup.alpha, 0, fadeDuration, false));
        }
    }

    public void ShowSlotSelectionWithFade()
    {
        // Fazer LogoPanel e ButtonContainer desaparecerem
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));

        // Fazer SlotSelectionPanel aparecer
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 1, fadeDuration, true));

        // Atualiza a informa��o de cada slot quando o painel � exibido
        UpdateSlotInformation();
    }

    public void ShowLoadingScreenWithFade()
    {
        // Faz todos os outros pain�is desaparecerem
        StartCoroutine(FadeCanvasGroup(logoCanvasGroup, logoCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, mainMenuCanvasGroup.alpha, 0, fadeDuration, false));
        StartCoroutine(FadeCanvasGroup(slotSelectionCanvasGroup, slotSelectionCanvasGroup.alpha, 0, fadeDuration, false));

        // Faz a tela de loading aparecer
        if (loadingScreenCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(loadingScreenCanvasGroup, loadingScreenCanvasGroup.alpha, 1, fadeDuration, true));
        }
    }


    // --- L�gica dos Slots ---
    void UpdateSlotInformation()
    {
        for (int i = 0; i < slotStatusTexts.Length; i++)
        {
            int slotIndex = i + 1;
            string savedKey = "Slot" + slotIndex + "_HasSave";

            if (PlayerPrefs.HasKey(savedKey))
            {
                int deaths = PlayerPrefs.GetInt("Slot" + slotIndex + "_Deaths", 0);
                int maxDays = PlayerPrefs.GetInt("Slot" + slotIndex + "_MaxDays", 0);
                slotStatusTexts[i].text = $"Slot {slotIndex} (Mortes: {deaths}, Dias: {maxDays})";
            }
            else
            {
                slotStatusTexts[i].text = $"Slot {slotIndex} (NOVO JOGO)";
            }
        }
    }

    public void SelectSlot(int slotNumber)
    {
        Debug.Log($"Slot {slotNumber} selecionado. Carregando jogo...");
        PlayerPrefs.SetInt("CurrentSelectedSlot", slotNumber);
        PlayerPrefs.Save();

        // Mostra a tela de loading com fade e depois carrega a cena
        // Note que o carregamento da cena ir� destruir este objeto de menu,
        // ent�o o fade da tela de loading pode n�o terminar completamente antes da cena carregar.
        // Para transi��es mais suaves, voc� usaria SceneManager.LoadSceneAsync e esperaria o fade.
        StartCoroutine(LoadGameAfterFade(gameSceneName));
    }

    IEnumerator LoadGameAfterFade(string sceneName)
    {
        ShowLoadingScreenWithFade(); // Inicia o fade da tela de loading
        yield return new WaitForSeconds(fadeDuration); // Espera o fade terminar

        SceneManager.LoadScene(sceneName);
    }


    // --- Fun��es dos Bot�es do Menu Principal ---

    public void StartGameButton() // Chamada pelo bot�o "Start Game"
    {
        ShowSlotSelectionWithFade();
    }

    public void OpenOptions()
    {
        Debug.Log("Abrindo op��es...");
        // Para implementar com fade, voc� faria algo como:
        // StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, 1, 0, fadeDuration, false));
        // StartCoroutine(FadeCanvasGroup(optionsCanvasGroup, 0, 1, fadeDuration, true));
    }

    public void OpenCredits()
    {
        Debug.Log("Abrindo cr�ditos...");
        // Exemplo:
        // StartCoroutine(FadeCanvasGroup(mainMenuCanvasGroup, 1, 0, fadeDuration, false));
        // StartCoroutine(FadeCanvasGroup(creditsCanvasGroup, 0, 1, fadeDuration, true));
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Saindo do jogo (apenas no build final)");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
