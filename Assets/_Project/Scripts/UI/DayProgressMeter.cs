using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DayProgressMeter : MonoBehaviour
{
    public static DayProgressMeter Instance { get; private set; }

    [Header("Configurações do Medidor")]
    public Image dayImage;
    public int enemiesToWin = 10;

    [Header("Efeitos Visuais")]
    public Image glowImage;
    public float glowDuration = 0.5f;

    [Header("Animação do Preenchimento")]
    public float fillAnimationDuration = 0.3f;

    private Coroutine fillAnimationCoroutine;

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
        dayImage.fillAmount = 1f;

        if (glowImage != null)
            glowImage.color = new Color(1, 1, 1, 0);

    }

    public void EnemyWasDefeated()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.EnemyDefeated();
        }

        TriggerGlow();

        if (fillAnimationCoroutine != null)
            StopCoroutine(fillAnimationCoroutine);

        fillAnimationCoroutine = StartCoroutine(AnimateFillAmount());
    }

    IEnumerator AnimateFillAmount()
    {
        float startAmount = dayImage.fillAmount;

        int defeated = EnemyManager.Instance != null ? EnemyManager.Instance.enemiesDefeated : 0;
        int toDefeat = EnemyManager.Instance != null ? EnemyManager.Instance.enemiesToDefeat : enemiesToWin;

        float targetAmount = 1.0f - ((float)defeated / toDefeat);
        float timer = 0f;

        while (timer < fillAnimationDuration)
        {
            dayImage.fillAmount = Mathf.Lerp(startAmount, targetAmount, timer / fillAnimationDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        dayImage.fillAmount = targetAmount;
    }

    void WinGame()
    {
        dayImage.fillAmount = 0f;
    }

    void TriggerGlow()
    {
        if (glowImage != null)
        {
            StopAllCoroutines();
            StartCoroutine(GlowEffect());
        }
    }

    IEnumerator GlowEffect()
    {
        glowImage.color = Color.white;

        float timer = 0f;
        while (timer < glowDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / glowDuration);
            glowImage.color = new Color(1, 1, 1, alpha);

            timer += Time.deltaTime;
            yield return null;
        }
        glowImage.color = new Color(1, 1, 1, 0);
    }
}
