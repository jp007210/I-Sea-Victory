using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 25;

    void Start()
    {
        Collider myCollider = GetComponent<Collider>();
        Collider[] allColliders = Object.FindObjectsOfType<Collider>();

        foreach (var col in allColliders)
        {
            if (!col.CompareTag("Player"))
            {
                Physics.IgnoreCollision(myCollider, col, true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats ph = other.GetComponent<PlayerStats>();
            if (ph != null)
            {
                ph.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
