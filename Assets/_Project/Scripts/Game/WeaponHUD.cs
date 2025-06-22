using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHUD : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public Slider cooldownSlider;

    public void SetWeaponName(string name)
    {
        if (weaponNameText != null)
            weaponNameText.text = name;
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