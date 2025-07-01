using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public Transform firePoint;
    public List<Weapon> allWeapons = new List<Weapon>();
    private List<Weapon> unlockedWeapons = new List<Weapon>();
    private int currentWeaponIndex = 0;

    public WeaponHUD weaponHUD;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadWeaponData();

        foreach (var w in allWeapons)
            w.gameObject.SetActive(false);

        if (unlockedWeapons.Count > 0)
        {
            unlockedWeapons[currentWeaponIndex].gameObject.SetActive(true);
        }

        UpdateHUD();
    }

    void Update()
    {
        if (PauseManager.GameIsPaused) return;

        HandleInput();

        Weapon currentWeapon = GetCurrentWeapon();
        if (currentWeapon == null) return;

        currentWeapon.UpdateCooldown(Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && currentWeapon.CanFire())
        {
            FireWeapon();
        }

        if (weaponHUD)
        {
            weaponHUD.UpdateCooldown(currentWeapon.GetCooldownTimer(), currentWeapon.cooldown);
        }
    }

    void HandleInput()
    {
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToWeapon(i);
            }
        }
    }

    void SwitchToWeapon(int index)
    {
        if (index < 0 || index >= unlockedWeapons.Count || index == currentWeaponIndex) return;

        unlockedWeapons[currentWeaponIndex].gameObject.SetActive(false);
        currentWeaponIndex = index;
        unlockedWeapons[currentWeaponIndex].gameObject.SetActive(true);

        SaveWeaponData();
        UpdateHUD();
    }

    void FireWeapon()
    {
        Weapon weapon = GetCurrentWeapon();
        List<Weapon.ShotData> shots = weapon.GetShots(firePoint.position, firePoint);

        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        int extra = stats != null ? stats.extraProjectiles : 0;
        float angleStep = 10f;

        // Tiros principais
        foreach (var shot in shots)
        {
            SpawnProjectile(weapon.projectilePrefab, shot, weapon.projectileSpeed);

            // Projéteis extras simétricos
            for (int i = 1; i <= extra; i++)
            {
                // Roda à direita e à esquerda do tiro principal
                var right = new Weapon.ShotData
                {
                    position = shot.position,
                    rotation = shot.rotation * Quaternion.Euler(0, angleStep * i, 0),
                    direction = shot.rotation * Quaternion.Euler(0, angleStep * i, 0) * Vector3.forward
                };
                var left = new Weapon.ShotData
                {
                    position = shot.position,
                    rotation = shot.rotation * Quaternion.Euler(0, -angleStep * i, 0),
                    direction = shot.rotation * Quaternion.Euler(0, -angleStep * i, 0) * Vector3.forward
                };

                SpawnProjectile(weapon.projectilePrefab, right, weapon.projectileSpeed);
                SpawnProjectile(weapon.projectilePrefab, left, weapon.projectileSpeed);
            }
        }

        weapon.ResetCooldown();
    }

    void SpawnProjectile(GameObject prefab, Weapon.ShotData shot, float speed)
    {
        Vector3 spawnPos = shot.position;
        if (spawnPos.y < 1f) spawnPos.y = 1f;

        GameObject bullet = Instantiate(prefab, spawnPos, shot.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = shot.direction * speed;

        // Aplica escala baseada no PlayerStats
        PlayerStats stats = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();
        if (stats != null)
        {
            bullet.transform.localScale *= stats.projectileSizeMultiplier;
        }

        Weapon current = GetCurrentWeapon();

        // Instancia VFX no disparo
        if (current.muzzleVFX != null)
        {
            var muzzle = Instantiate(current.muzzleVFX, spawnPos, shot.rotation);
            muzzle.SendEvent("OnShoot");
            Destroy(muzzle.gameObject, 2f); // tempo de vida do VFX
        }

        // Reproduz som de disparo
        if (current.shootClip != null)
        {
            AudioSource.PlayClipAtPoint(current.shootClip, spawnPos, current.volume);
        }

        // Inicializa projétil
        TempProjectile proj = bullet.GetComponent<TempProjectile>();
        if (proj != null)
        {
            proj.Init(current.damage, current.impactVFX, null, current.volume, current.hitLayers, current.projectileLifetime);
            // impactClip foi retirado pois agora o som toca no disparo, não no impacto
        }

        // Ignora colisão com o player
        Collider playerCol = GameObject.FindWithTag("Player")?.GetComponent<Collider>();
        Collider bulletCol = bullet.GetComponent<Collider>();
        if (playerCol != null && bulletCol != null)
            Physics.IgnoreCollision(bulletCol, playerCol);

        Destroy(bullet, 5f);
    }

    void UpdateHUD()
    {
        Weapon currentWeapon = GetCurrentWeapon();

        if (weaponHUD != null && currentWeapon != null)
        {
            weaponHUD.SetWeaponName(currentWeapon.name);
            weaponHUD.SetWeaponImage(currentWeapon.weaponSprite); // Atualiza a imagem da arma
        }
    }

    public Weapon GetCurrentWeapon()
    {
        if (unlockedWeapons.Count == 0) return null;
        return unlockedWeapons[currentWeaponIndex];
    }

    public void UnlockNextWeapon()
    {
        int nextIndex = unlockedWeapons.Count;
        if (nextIndex < allWeapons.Count)
        {
            Weapon newWeapon = allWeapons[nextIndex];
            unlockedWeapons.Add(newWeapon);
            SaveWeaponData();
            Debug.Log("Desbloqueou arma: " + newWeapon.name);
        }
    }

    void SaveWeaponData()
    {
        List<int> indices = new List<int>();
        foreach (Weapon w in unlockedWeapons)
        {
            int index = allWeapons.IndexOf(w);
            if (index != -1)
                indices.Add(index);
        }

        string unlockedStr = string.Join(";", indices);
        PlayerPrefs.SetString("UnlockedWeapons", unlockedStr);
        PlayerPrefs.SetInt("CurrentWeaponIndex", currentWeaponIndex);
        PlayerPrefs.Save();
    }

    void LoadWeaponData()
    {
        unlockedWeapons.Clear();
        string unlockedStr = PlayerPrefs.GetString("UnlockedWeapons", "0");
        string[] tokens = unlockedStr.Split(';');

        foreach (string token in tokens)
        {
            if (int.TryParse(token, out int index) && index >= 0 && index < allWeapons.Count)
            {
                unlockedWeapons.Add(allWeapons[index]);
            }
        }

        currentWeaponIndex = PlayerPrefs.GetInt("CurrentWeaponIndex", 0);
        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, unlockedWeapons.Count - 1);
    }

    public void ResetWeapons()
    {
        PlayerPrefs.DeleteKey("UnlockedWeapons");
        PlayerPrefs.DeleteKey("CurrentWeaponIndex");
    }
}