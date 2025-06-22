using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private WeaponManager weaponManager;

    void Start()
    {
        weaponManager = Object.FindFirstObjectByType<WeaponManager>();
        if (weaponManager == null)
        {
            Debug.LogError("PlayerShooting: WeaponManager não encontrado na cena!");
            enabled = false;
        }
    }

    void Update()
    {
        if (PauseMenuManager.GameIsPaused)
            return;
    }
}
