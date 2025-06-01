using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Boss")) // 👈 Tag correta!
        {
            NavioPirataHealth bossHealth = other.GetComponent<NavioPirataHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // destrói o projétil
        }
    }
}
