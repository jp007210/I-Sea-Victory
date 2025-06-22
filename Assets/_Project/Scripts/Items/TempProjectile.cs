using UnityEngine;
using UnityEngine.VFX;

public class TempProjectile : MonoBehaviour
{
    float damage;
    VisualEffect impactVFX;
    AudioClip impactClip;
    float volume;
    LayerMask hitLayers;
    float lifetime;
    public void SetDamage(float value)
    {
        damage = value;
    }

    public void Init(float dmg, VisualEffect vfx, AudioClip clip, float vol, LayerMask layers, float life)
    {
        damage = dmg;
        impactVFX = vfx;
        impactClip = clip;
        volume = vol;
        hitLayers = layers;
        lifetime = life;

        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) == 0)
            return;

        // Tenta dar dano
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null) eh.TakeDamage(damage);
        }
        else if (other.CompareTag("Boss"))
        {
            NavioPirataHealth boss = other.GetComponent<NavioPirataHealth>();
            if (boss != null) boss.TakeDamage(damage);
        }

        // VFX
        if (impactVFX)
        {
            var impact = Instantiate(impactVFX, transform.position, Quaternion.identity);
            impact.SendEvent("OnImpact");
            Destroy(impact.gameObject, 2f);
        }

        // Som
        if (impactClip)
        {
            AudioSource.PlayClipAtPoint(impactClip, transform.position, volume);
        }

        Destroy(gameObject); // destrói projétil
    }
}