using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int scoreToWin = 10; // Exemplo: 10 pontos para ativar o boss
    public string bossSceneName = "NavioPirata"; // Nome exato da cena do boss

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score >= scoreToWin)
        {
            LoadBossScene();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    void LoadBossScene()
    {
        Debug.Log("Carregando cena do boss...");
        SceneManager.LoadScene(bossSceneName);
    }
}
