using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int progressionStep = 0;
    public int enemiesDefeated = 0;
    public int enemiesToDefeat = 5;

    private bool awaitingPowerUpChoice = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        progressionStep = 0;
        enemiesDefeated = 0;
        enemiesToDefeat = 5;
        awaitingPowerUpChoice = false;
        SceneManager.LoadScene("EnemyStage");
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;

        if (enemiesDefeated >= enemiesToDefeat && !awaitingPowerUpChoice)
        {
            awaitingPowerUpChoice = true; // trava a progressão até o player escolher
            enemiesDefeated = 0;

            PowerUpManager.Instance.ShowPowerUpChoices(() =>
            {
                awaitingPowerUpChoice = false;
                progressionStep++;

                SceneManager.LoadScene("BossStage");
            });
        }
    }

    public void BossDefeated()
    {
        progressionStep++;
        enemiesToDefeat += 2;

        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.UnlockNextWeapon();
        }

        SceneManager.LoadScene("EnemyStage");
    }

    public bool IsEnemyStage() => progressionStep % 2 == 0;

    public int GetCurrentIndex() => progressionStep / 2;
}