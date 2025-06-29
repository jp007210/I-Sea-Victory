using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject slotSelectionPanel;
    public GameObject logoPanel;

    public float animationDuration = 0.3f;
    public AnimationCurve animationCurve = null;

    public string mainMenuSceneName = "MainMenu";
    public string gameSceneName = "GameScene";

    private GameObject currentActivePanel;
    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            OpenMainMenu();
            if (logoPanel) SetPanelState(logoPanel, true);
        }
        else
        {
            CloseAllPanels();
        }
    }

    public void OpenPanel(GameObject panel)
    {
        if (isTransitioning || panel == null) return;
        StartCoroutine(SwitchPanel(panel));
        DeselectAllButtons();
    }

    private IEnumerator SwitchPanel(GameObject newPanel)
    {
        isTransitioning = true;

        if (currentActivePanel != null)
        {
            yield return StartCoroutine(AnimatePanel(currentActivePanel, false));
            currentActivePanel.SetActive(false);
        }

        // Só desativa o LogoPanel se não for o menu principal
        if (logoPanel && newPanel != mainMenuPanel)
            SetPanelState(logoPanel, false);

        currentActivePanel = newPanel;
        newPanel.SetActive(true);
        yield return StartCoroutine(AnimatePanel(newPanel, true));

        isTransitioning = false;
    }

    private IEnumerator AnimatePanel(GameObject panel, bool fadeIn)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        canvasGroup.interactable = fadeIn;
        canvasGroup.blocksRaycasts = fadeIn;

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        canvasGroup.alpha = startAlpha;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / animationDuration;
            float curveValue = animationCurve != null ? animationCurve.Evaluate(progress) : progress;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, curveValue);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = fadeIn;
        canvasGroup.blocksRaycasts = fadeIn;
    }

    public void CloseAllPanels()
    {
        if (mainMenuPanel) SetPanelState(mainMenuPanel, false);
        if (optionsPanel) SetPanelState(optionsPanel, false);
        if (audioPanel) SetPanelState(audioPanel, false);
        if (videoPanel) SetPanelState(videoPanel, false);
        if (controlsPanel) SetPanelState(controlsPanel, false);
        if (creditsPanel) SetPanelState(creditsPanel, false);
        if (pausePanel) SetPanelState(pausePanel, false);
        if (gameOverPanel) SetPanelState(gameOverPanel, false);
        if (slotSelectionPanel) SetPanelState(slotSelectionPanel, false);
        if (logoPanel) SetPanelState(logoPanel, false);
        currentActivePanel = null;
    }

    private void SetPanelState(GameObject panel, bool open)
    {
        if (panel == null) return;
        panel.SetActive(true); // Garante que está ativo antes de mexer no CanvasGroup
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = open ? 1f : 0f;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
        if (!open)
            panel.SetActive(false); // Só desativa depois de garantir alpha 0
    }

    public void OpenMainMenu()
    {
        OpenPanel(mainMenuPanel);
        if (logoPanel) SetPanelState(logoPanel, true);
        DeselectAllButtons();
    }
    public void OpenOptions() => OpenPanel(optionsPanel);
    public void OpenAudio() => OpenPanel(audioPanel);
    public void OpenVideo() => OpenPanel(videoPanel);
    public void OpenControls() => OpenPanel(controlsPanel);
    public void OpenCredits() => OpenPanel(creditsPanel);
    public void OpenPause() => OpenPanel(pausePanel);
    public void OpenGameOver() => OpenPanel(gameOverPanel);
    public void OpenSlotSelection() => OpenPanel(slotSelectionPanel);

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.ResetWeapons();
        }
        ResetPowerUps();
        SceneManager.LoadScene("EnemyStage");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OpenPause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
    }

    public bool IsAnyMenuOpen()
    {
        return currentActivePanel != null && currentActivePanel.activeInHierarchy;
    }

    private void DeselectAllButtons()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    public void ResetPowerUps()
    {
        PlayerPrefs.DeleteKey("UnlockedPowerUps");
        PlayerPrefs.Save();
        Debug.Log("Todos os power ups foram resetados.");
    }
}
