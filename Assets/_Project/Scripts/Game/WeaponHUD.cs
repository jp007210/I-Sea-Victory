using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHUD : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public Slider cooldownSlider;

    private Weapon currentWeapon;

    void Update()
    {
        if (currentWeapon != null)
        {
            cooldownSlider.maxValue = currentWeapon.cooldown;
            cooldownSlider.value = currentWeapon.GetCooldownTimer();
        }
    }

    public void UpdateWeaponInfo(Weapon weapon)
    {
        currentWeapon = weapon;

        if (weapon != null)
        {
            weaponNameText.text = weapon.name;
            cooldownSlider.maxValue = weapon.cooldown;
            cooldownSlider.value = weapon.GetCooldownTimer();
        }
        else
        {
            weaponNameText.text = "Sem arma";
            cooldownSlider.value = 0;
        }
    }
}