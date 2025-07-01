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
        if (other.CompareTag("Bullet"))
            return;

        if (((1 << other.gameObject.layer) & hitLayers) == 0)
        {
            Debug.Log("Camada não está no hitLayers: " + other.gameObject.name);
            return;
        }

        Debug.Log("Colidiu com: " + other.gameObject.name);

        // Enemy comum
        EnemyHealth eh = other.GetComponent<EnemyHealth>();
        if (eh != null)
        {
            Debug.Log("Acertou EnemyHealth");
            eh.TakeDamage(damage);
        }

        // Mini Boss
        NavioPirataHealth boss = other.GetComponent<NavioPirataHealth>();
        if (boss != null)
        {
            Debug.Log("Acertou NavioPirataHealth");
            boss.TakeDamage(damage);
        }

        // Kraken
        KrakenHealth kraken = other.GetComponentInParent<KrakenHealth>();
        if (kraken != null)
        {
            Debug.Log("Acertou KrakenHealth");
            kraken.TakeDamage(damage);
        }

        // VFX
        if (impactVFX)
        {
            var impact = Instantiate(impactVFX, transform.position, Quaternion.identity);
            impact.SendEvent("OnImpact");
            Destroy(impact.gameObject, 2f);
        }

        // SFX
        if (impactClip)
        {
            AudioSource.PlayClipAtPoint(impactClip, transform.position, volume);
        }

        Destroy(gameObject);
    }
}