using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    public Image weaponImage;      // A imagem da arma atual
    public Slider cooldownSlider;  // O slider de cooldown (já existia)

    public void SetWeaponImage(Sprite weaponSprite)
    {
        if (weaponImage != null && weaponSprite != null)
        {
            weaponImage.sprite = weaponSprite;
            weaponImage.preserveAspect = true; // Mantém a proporção da imagem
        }
    }

    public void UpdateCooldown(float current, float max)
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = max;
            cooldownSlider.value = current;
        }
    }
}