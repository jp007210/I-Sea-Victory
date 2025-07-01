using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHUD : MonoBehaviour
{
    [Header("Referências UI")]
    public Image weaponImage;      // Ícone da arma equipada
    public Slider cooldownSlider;  // Barra de cooldown
    public TextMeshProUGUI weaponNameText;    // Nome da arma

    /// <summary>
    /// Atualiza o nome da arma na HUD
    /// </summary>
    public void SetWeaponName(string name)
    {
        if (weaponNameText != null)
            weaponNameText.text = name;
    }

    /// <summary>
    /// Atualiza a imagem da arma na HUD
    /// </summary>
    public void SetWeaponImage(Sprite weaponSprite)
    {
        if (weaponImage != null)
        {
            if (weaponSprite != null)
            {
                weaponImage.sprite = weaponSprite;
                weaponImage.enabled = true;
                weaponImage.preserveAspect = true;
            }
            else
            {
                weaponImage.enabled = false; // Esconde a imagem se não houver sprite
            }
        }
    }

    /// <summary>
    /// Atualiza o cooldown da arma na HUD
    /// </summary>
    public void UpdateCooldown(float current, float max)
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = max;
            cooldownSlider.value = current;
        }
    }
}