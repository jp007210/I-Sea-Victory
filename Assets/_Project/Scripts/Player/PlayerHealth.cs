using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Referências da UI")]
    public Slider healthBarSlider;

    [Header("Efeitos de Dano")]
    public Image damageGlowImage;
    public Color damageGlowColor = new Color(0.5f, 0, 0, 0.7f);
    public float glowDuration = 0.4f;
    public float healthAnimationDuration = 0.3f;

    private Coroutine healthAnimationCoroutine;
    private Coroutine glowAnimationCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

        if (damageGlowImage != null)
        {
            damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, 0);
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthAnimationCoroutine != null) StopCoroutine(healthAnimationCoroutine);
        healthAnimationCoroutine = StartCoroutine(AnimateHealthBar());

        if (glowAnimationCoroutine != null) StopCoroutine(glowAnimationCoroutine);
        glowAnimationCoroutine = StartCoroutine(DamageGlowEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator AnimateHealthBar()
    {
        float startValue = healthBarSlider.value;
        float targetValue = currentHealth;
        float timer = 0f;

        while (timer < healthAnimationDuration)
        {
            healthBarSlider.value = Mathf.Lerp(startValue, targetValue, timer / healthAnimationDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        healthBarSlider.value = targetValue;
    }

    IEnumerator DamageGlowEffect()
    {
        if (damageGlowImage == null) yield break;

        damageGlowImage.color = damageGlowColor;

        float timer = 0f;
        while (timer < glowDuration)
        {
            float alpha = Mathf.Lerp(damageGlowColor.a, 0f, timer / glowDuration);
            damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, 0);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (healthAnimationCoroutine != null) StopCoroutine(healthAnimationCoroutine);
        healthAnimationCoroutine = StartCoroutine(AnimateHealthBar());
    }

    void Die()
    {
        if (GameOverScreen.Instance != null)
        {
            GameOverScreen.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverScreen.Instance não encontrado! Certifique-se de que o GameOverScreen está na cena e ativo.");
            Time.timeScale = 0f;
        }

        gameObject.SetActive(false);
    }
}
