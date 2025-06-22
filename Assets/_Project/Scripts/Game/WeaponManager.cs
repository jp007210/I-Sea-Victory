using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex = 0;

    public Transform firePoint;

    public WeaponHUD weaponHUD; // Referência ao HUD

    void Start()
    {
        if (weaponHUD != null)
        {
            weaponHUD.UpdateWeaponInfo(GetCurrentWeapon());
        }
    }

    void Update()
    {
        if (weapons.Length == 0 || firePoint == null) return;

        Weapon currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.UpdateCooldown(Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && currentWeapon.CanFire())
        {
            List<Weapon.ShotData> shots = currentWeapon.GetShots(firePoint.position, transform);
            foreach (var shot in shots)
            {
                SpawnProjectile(currentWeapon.projectilePrefab, shot, currentWeapon.projectileSpeed);
            }
            currentWeapon.ResetCooldown();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextWeapon();
        }
    }

    void SpawnProjectile(GameObject prefab, Weapon.ShotData shot, float speed)
    {
        Vector3 spawnPos = shot.position;
        if (spawnPos.y < 1.5f)
            spawnPos.y = 1.5f;

        GameObject bullet = Instantiate(prefab, spawnPos, shot.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = shot.direction * speed;

        TempProjectile proj = bullet.GetComponent<TempProjectile>();
        if (proj != null)
        {
            Weapon current = GetCurrentWeapon();
            proj.Init(
                current.damage,
                current.impactVFX,
                current.impactClip,
                current.volume,
                current.hitLayers,
                5f
            );
        }

        // Ignora colisão com jogador
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider playerCol = player.GetComponent<Collider>();
            Collider bulletCol = bullet.GetComponent<Collider>();
            if (playerCol != null && bulletCol != null)
                Physics.IgnoreCollision(bulletCol, playerCol);
        }

        Destroy(bullet, 5f);
    }

    public Weapon GetCurrentWeapon()
    {
        if (weapons == null || weapons.Length == 0)
            return null;
        return weapons[currentWeaponIndex];
    }

    public void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        if (weaponHUD != null)
        {
            weaponHUD.UpdateWeaponInfo(GetCurrentWeapon());
        }
    }
}