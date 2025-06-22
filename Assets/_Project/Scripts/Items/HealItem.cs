using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 25;

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
