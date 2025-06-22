using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int progressionStep = 0; // 0 = inimigo tipo 0, 1 = boss tipo 0, 2 = inimigo tipo 1, etc
    public int enemiesDefeated = 0;
    public int enemiesToDefeat = 5;

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
        SceneManager.LoadScene("EnemyStage");
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        if (enemiesDefeated >= enemiesToDefeat)
        {
            enemiesDefeated = 0;
            progressionStep++; // vai para fase de boss
            SceneManager.LoadScene("BossStage");
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

    public bool IsEnemyStage()
    {
        return progressionStep % 2 == 0;
    }

    public int GetCurrentIndex()
    {
        return progressionStep / 2;
    }
}
