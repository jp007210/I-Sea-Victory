using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public float cooldown = 1f;
    public float damage = 10f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;

    [HideInInspector] public float cooldownTimer = 0f;

    public bool CanFire() { return cooldownTimer <= 0; }
    public void ResetCooldown() { cooldownTimer = cooldown; }
    public float GetCooldownTimer() { return cooldownTimer; }
}