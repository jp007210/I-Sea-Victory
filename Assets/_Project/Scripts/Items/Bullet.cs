using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public VisualEffect harpoonVFX;
    public Transform firePoint;
    public int damage = 20;
    public AudioClip impactClip;
    public AudioSource audioSource;

    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player").GetComponent<Collider>());
        if (harpoonVFX != null && firePoint != null)
        {
            harpoonVFX.transform.position = firePoint.position;
            harpoonVFX.transform.forward = firePoint.forward;
            harpoonVFX.Play(); // ativa clarão + trilha
        }
    }

    void OnTriggerEnter(Collider other)
    {
        bool hit = false;

        if (other.CompareTag("Enemy"))
        {
            VisualEffect impact = Instantiate(harpoonVFX, transform.position, Quaternion.identity);
            impact.SendEvent("OnImpact");
            Destroy(gameObject);
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
                hit = true;
            }
            PlayImpactSound();
        }

        if (other.CompareTag("Boss"))
        {
            if (audioSource && impactClip)
                AudioSource.PlayClipAtPoint(impactClip, transform.position);
            VisualEffect impact = Instantiate(harpoonVFX, transform.position, Quaternion.identity);
            impact.SendEvent("OnImpact");
            Destroy(gameObject);
            NavioPirataHealth boss = other.GetComponent<NavioPirataHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                hit = true;
            }
            PlayImpactSound();
        }

        if (hit && harpoonVFX != null)
        {
            harpoonVFX.transform.position = transform.position;
            harpoonVFX.SendEvent("OnImpact");
        }

        Destroy(gameObject);
    }
    void PlayImpactSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                // Cria um objeto temporário para tocar o som
                GameObject tempAudio = new GameObject("ImpactSound");
                tempAudio.transform.position = transform.position;

                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = audioSource.clip;
                tempSource.volume = audioSource.volume;
                tempSource.Play();

                Destroy(tempAudio, tempSource.clip.length); // destrói o som após terminar
            }
        }
    }
}