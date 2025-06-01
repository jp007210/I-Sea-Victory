using UnityEngine;
using UnityEngine.UI;

public class NavioPirataUI : MonoBehaviour
{
    public NavioPirataHealth bossHealth;   // Referência ao script de vida do boss
    public Slider healthSlider;         // Barra de vida (UI)
    public Text healthText;             // Texto opcional (ex: 100 / 200)

    void Update()
    {
        if (bossHealth == null || healthSlider == null) return;

        // Atualiza barra
        healthSlider.maxValue = bossHealth.maxHealth;
        healthSlider.value = bossHealth.GetCurrentHealth();

        // Atualiza texto (se existir)
        if (healthText != null)
        {
            healthText.text = $"{bossHealth.GetCurrentHealth()} / {bossHealth.maxHealth}";
        }
    }
}