using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public VisualEffect harpoonVFX;
    public Transform firePoint;
    public int damage = 20;

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
        }

        if (other.CompareTag("Boss"))
        {
            VisualEffect impact = Instantiate(harpoonVFX, transform.position, Quaternion.identity);
            impact.SendEvent("OnImpact");
            Destroy(gameObject);
            NavioPirataHealth boss = other.GetComponent<NavioPirataHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                hit = true;
            }
        }

        if (hit && harpoonVFX != null)
        {
            harpoonVFX.transform.position = transform.position;
            harpoonVFX.SendEvent("OnImpact");
        }

        Destroy(gameObject);
    }
}