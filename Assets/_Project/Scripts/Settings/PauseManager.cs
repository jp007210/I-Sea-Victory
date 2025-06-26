using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public static bool GameIsPaused = false;

    [Header("Input Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public bool allowClickOutsideToPause = true;

    [Header("Pause Panel")]
    public RectTransform pausePanelRect;

    // Eventos
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        HandlePauseInput();
        HandleClickOutside();
    }

    private void HandlePauseInput()
    {
        // Não processar input se estiver em game over
        if (GameOverScreen.Instance != null && GameOverScreen.Instance.IsGameOver)
            return;

        // Não processar input se outro menu estiver aberto
        if (MenuManager.Instance != null && MenuManager.Instance.IsAnyMenuOpen() && !GameIsPaused)
            return;

        if (Input.GetKeyDown(pauseKey))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void HandleClickOutside()
    {
        if (!allowClickOutsideToPause || !GameIsPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (pausePanelRect != null && !RectTransformUtility.RectangleContainsScreenPoint(pausePanelRect, Input.mousePosition))
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        if (GameIsPaused) return;

        GameIsPaused = true;
        Time.timeScale = 0f;

        // Pausar áudio se existir
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PauseMusic();
        }

        // Abrir painel de pause
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.OpenPause();
        }

        OnGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        if (!GameIsPaused) return;

        GameIsPaused = false;
        Time.timeScale = 1f;

        // Retomar áudio se existir
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnpauseMusic();
        }

        // Fechar todos os painéis
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.CloseAllPanels();
        }

        OnGameResumed?.Invoke();
    }

    public void RestartGame()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.RestartGame();
        }
    }

    public void ReturnToMainMenu()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ReturnToMainMenu();
        }
    }

    public void QuitGame()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.QuitGame();
        }
    }

    // Método para pausar/despausar externamente
    public void TogglePause()
    {
        if (GameIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Método para verificar se o jogo pode ser pausado
    public bool CanPause()
    {
        // Não pode pausar se estiver em game over
        if (GameOverScreen.Instance != null && GameOverScreen.Instance.IsGameOver)
            return false;

        // Não pode pausar se outros menus importantes estiverem abertos
        if (MenuManager.Instance != null && MenuManager.Instance.IsAnyMenuOpen() && !GameIsPaused)
            return false;

        return true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Pausar automaticamente quando o jogo perde o foco
        if (!hasFocus && !GameIsPaused && CanPause())
        {
            PauseGame();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Pausar automaticamente em dispositivos móveis
        if (pauseStatus && !GameIsPaused && CanPause())
        {
            PauseGame();
        }
    }
}