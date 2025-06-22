using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats ph = other.GetComponent<PlayerStats>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}