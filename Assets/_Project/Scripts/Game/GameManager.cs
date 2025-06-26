using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int score = 0;
    public int scoreToWin = 10;
    public string bossSceneName = "NavioPirata";

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;

    [Header("Game State")]
    public int playerLives = 3;
    public float gameTime = 0f;
    public bool gameRunning = true;

    // Eventos para comunicação com outros sistemas
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action OnGameWon;
    public static event Action OnGameLost;
    public static event Action<float> OnTimeChanged;

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

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (gameRunning && !PauseManager.GameIsPaused)
        {
            UpdateGameTime();
        }
    }

    private void InitializeGame()
    {
        // Inicializar UI
        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimeUI();

        // Notificar outros sistemas sobre o estado inicial
        OnScoreChanged?.Invoke(score);
        OnLivesChanged?.Invoke(playerLives);
        OnTimeChanged?.Invoke(gameTime);
    }

    #region Score System
    public void AddScore(int amount)
    {
        if (!gameRunning) return;

        score += amount;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(score);

        // Verificar condição de vitória
        if (score >= scoreToWin)
        {
            WinGame();
        }
    }

    public void RemoveScore(int amount)
    {
        if (!gameRunning) return;

        score = Mathf.Max(0, score - amount);
        UpdateScoreUI();
        OnScoreChanged?.Invoke(score);
    }

    public void SetScore(int newScore)
    {
        score = Mathf.Max(0, newScore);
        UpdateScoreUI();
        OnScoreChanged?.Invoke(score);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
    #endregion

    #region Lives System
    public void LoseLife()
    {
        if (!gameRunning) return;

        playerLives--;
        UpdateLivesUI();
        OnLivesChanged?.Invoke(playerLives);

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void AddLife()
    {
        if (!gameRunning) return;

        playerLives++;
        UpdateLivesUI();
        OnLivesChanged?.Invoke(playerLives);
    }

    public void SetLives(int newLives)
    {
        playerLives = Mathf.Max(0, newLives);
        UpdateLivesUI();
        OnLivesChanged?.Invoke(playerLives);

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + playerLives.ToString();
        }
    }
    #endregion

    #region Time System
    private void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
        UpdateTimeUI();
        OnTimeChanged?.Invoke(gameTime);
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ResetTime()
    {
        gameTime = 0f;
        UpdateTimeUI();
        OnTimeChanged?.Invoke(gameTime);
    }
    #endregion

    #region Game State Management
    public void WinGame()
    {
        if (!gameRunning) return;

        gameRunning = false;
        OnGameWon?.Invoke();

        Debug.Log("Carregando cena do boss...");

        // Pequeno delay antes de carregar a cena do boss
        Invoke(nameof(LoadBossScene), 1f);
    }

    public void GameOver()
    {
        if (!gameRunning) return;

        gameRunning = false;
        OnGameLost?.Invoke();

        // Mostrar tela de game over
        if (GameOverScreen.Instance != null)
        {
            GameOverScreen.Instance.ShowGameOver();
        }
    }

    public void RestartGame()
    {
        // Resetar estado do jogo
        score = 0;
        playerLives = 3;
        gameTime = 0f;
        gameRunning = true;

        // Atualizar UI
        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimeUI();

        // Notificar outros sistemas
        OnScoreChanged?.Invoke(score);
        OnLivesChanged?.Invoke(playerLives);
        OnTimeChanged?.Invoke(gameTime);

        // Recarregar cena atual
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.RestartGame();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void PauseGame()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.PauseGame();
        }
    }

    public void ResumeGame()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.ResumeGame();
        }
    }

    private void LoadBossScene()
    {
        SceneManager.LoadScene(bossSceneName);
    }
    #endregion

    #region Utility Methods
    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public bool IsGamePaused()
    {
        return PauseManager.GameIsPaused;
    }

    public void SetGameRunning(bool running)
    {
        gameRunning = running;
    }

    // Método para salvar progresso (pode ser expandido)
    public void SaveGameProgress()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.SetInt("LastLives", playerLives);
        PlayerPrefs.SetFloat("LastTime", gameTime);
        PlayerPrefs.Save();
    }

    // Método para carregar progresso (pode ser expandido)
    public void LoadGameProgress()
    {
        score = PlayerPrefs.GetInt("LastScore", 0);
        playerLives = PlayerPrefs.GetInt("LastLives", 3);
        gameTime = PlayerPrefs.GetFloat("LastTime", 0f);

        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimeUI();
    }
    #endregion

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && gameRunning)
        {
            SaveGameProgress();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && gameRunning)
        {
            SaveGameProgress();
        }
    }
}