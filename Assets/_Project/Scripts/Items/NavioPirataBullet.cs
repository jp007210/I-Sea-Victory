using UnityEngine;

public class NavioPirataBullet : MonoBehaviour
{
    public int damage = 15;
    public float lifetime = 5f;

    private bool hasHit = false;

    void Start()
    {
        // Destrói automaticamente após o tempo limite
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; // Evita múltiplas chamadas

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }

            hasHit = true;
            Destroy(gameObject);
        }
    }
}
