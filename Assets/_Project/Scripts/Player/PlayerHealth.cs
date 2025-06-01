using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;

    public GameObject loseScreen;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    void Start()
    {
        currentHealth = maxHealth;
        if (loseScreen == null)
        {
            GameObject found = GameObject.Find("LoseScreen");
            if (found != null)
            {
                loseScreen = found;
                loseScreen.SetActive(false);
            }
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage. Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        if (loseScreen != null)
            loseScreen.SetActive(true);
        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}
