using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Vida: " + playerHealth.currentHealth + " / " + playerHealth.maxHealth;
        }
    }
}