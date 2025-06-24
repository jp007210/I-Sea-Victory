using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text retryPromptText;
    public Button yesButton;
    public Button noButton;

    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        yesButton.onClick.AddListener(RestartGame);
        noButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverText.text = "GAME OVER";
        retryPromptText.text = "RETRY?";
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}