using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject loseScreen;
    public Slider healthBarSlider;

    void Start()
    {
        currentHealth = maxHealth;

        if (loseScreen == null)
        {
            GameObject found = GameObject.Find("LosePanel");
            if (found != null)
            {
                loseScreen = found;
                loseScreen.SetActive(false);
            }
        }

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;
    }

    void Die()
    {
        if (loseScreen != null)
            loseScreen.SetActive(true);
        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}
