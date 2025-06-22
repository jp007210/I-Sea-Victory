using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 25f;

    [Header("Dano")]
    public float damage = 20f;  // <--- adicione isso!

    [Header("Cooldown")]
    public float cooldown = 1f;
    protected float cooldownTimer = 0f;

    [Header("Impacto")]
    public VisualEffect impactVFX;
    public AudioClip impactClip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public LayerMask hitLayers = ~0; // padrão: colide com tudo

    public struct ShotData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 direction;
    }

    public abstract List<ShotData> GetShots(Vector3 firePointPosition, Transform weaponTransform);

    public bool CanFire()
    {
        return cooldownTimer <= 0f;
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= deltaTime;
    }

    public void ResetCooldown()
    {
        cooldownTimer = cooldown;
    }

    public float GetCooldownTimer()
    {
        return Mathf.Clamp(cooldownTimer, 0f, cooldown);
    }
}