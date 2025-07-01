using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
public class WeaponInfo
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public float cooldown = 1f;
    public float damage = 10f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    public VisualEffect impactVFX;
    public AudioClip impactClip;
    public float volume = 1f;
    public LayerMask hitLayers;
    public float projectileLifetime = 5f;

    [HideInInspector] public float cooldownTimer = 0f;

    public bool CanFire() { return cooldownTimer <= 0; }
    public void ResetCooldown() { cooldownTimer = cooldown; }
    public float GetCooldownTimer() { return cooldownTimer; }

    public List<ShotData> GetShots(Vector3 position, Transform firePoint)
    {
        var shots = new List<ShotData>();
        shots.Add(new ShotData
        {
            position = position,
            direction = firePoint.forward,
            rotation = firePoint.rotation
        });
        return shots;
    }

    [System.Serializable]
    public struct ShotData
    {
        public Vector3 position;
        public Vector3 direction;
        public Quaternion rotation;
    }
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public Transform firePoint;
    public List<WeaponInfo> allWeapons = new List<WeaponInfo>();
    private List<WeaponInfo> unlockedWeapons = new List<WeaponInfo>();
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
        {
            if (w.weaponPrefab != null)
                w.weaponPrefab.SetActive(false);
        }

        if (unlockedWeapons.Count > 0 && unlockedWeapons[currentWeaponIndex].weaponPrefab != null)
        {
            unlockedWeapons[currentWeaponIndex].weaponPrefab.SetActive(true);
        }

        UpdateHUD();
    }

    void Update()
    {
        if (PauseMenuManager.GameIsPaused) return;

        HandleInput();

        WeaponInfo currentWeapon = GetCurrentWeapon();
        if (currentWeapon == null) return;

        if (currentWeapon.cooldownTimer > 0)
        {
            currentWeapon.cooldownTimer -= Time.deltaTime;
            if (currentWeapon.cooldownTimer < 0)
                currentWeapon.cooldownTimer = 0;
        }

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

        if (unlockedWeapons[currentWeaponIndex].weaponPrefab != null)
            unlockedWeapons[currentWeaponIndex].weaponPrefab.SetActive(false);

        currentWeaponIndex = index;

        if (unlockedWeapons[currentWeaponIndex].weaponPrefab != null)
            unlockedWeapons[currentWeaponIndex].weaponPrefab.SetActive(true);

        SaveWeaponData();
        UpdateHUD();
    }

    void FireWeapon()
    {
        WeaponInfo weapon = GetCurrentWeapon();
        List<WeaponInfo.ShotData> shots = weapon.GetShots(firePoint.position, firePoint);

        foreach (var shot in shots)
        {
            SpawnProjectile(weapon.projectilePrefab, shot, weapon.projectileSpeed, weapon);
        }

        weapon.ResetCooldown();
    }

    void SpawnProjectile(GameObject prefab, WeaponInfo.ShotData shot, float speed, WeaponInfo weapon)
    {
        Vector3 spawnPos = shot.position;
        if (spawnPos.y < 1f) spawnPos.y = 1f;

        GameObject bullet = Instantiate(prefab, spawnPos, shot.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = shot.direction * speed;

        TempProjectile proj = bullet.GetComponent<TempProjectile>();
        if (proj != null)
        {
            proj.Init(weapon.damage, weapon.impactVFX, weapon.impactClip, weapon.volume, weapon.hitLayers, weapon.projectileLifetime);
        }

        Collider playerCol = GameObject.FindWithTag("Player")?.GetComponent<Collider>();
        Collider bulletCol = bullet.GetComponent<Collider>();
        if (playerCol != null && bulletCol != null)
            Physics.IgnoreCollision(bulletCol, playerCol);

        Destroy(bullet, 5f);
    }

    void UpdateHUD()
    {
        if (weaponHUD != null)
        {
            WeaponInfo currentWeapon = GetCurrentWeapon();
            if (currentWeapon != null)
            {
                weaponHUD.SetWeaponImage(currentWeapon.weaponIcon);
            }
        }
    }

    public WeaponInfo GetCurrentWeapon()
    {
        if (unlockedWeapons.Count == 0) return null;
        return unlockedWeapons[currentWeaponIndex];
    }

    public void UnlockNextWeapon()
    {
        int nextIndex = unlockedWeapons.Count;
        if (nextIndex < allWeapons.Count)
        {
            WeaponInfo newWeapon = allWeapons[nextIndex];
            unlockedWeapons.Add(newWeapon);
            SaveWeaponData();
            Debug.Log("Desbloqueou arma: " + newWeapon.weaponName);
        }
    }

    void SaveWeaponData()
    {
        List<int> indices = new List<int>();
        foreach (WeaponInfo w in unlockedWeapons)
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