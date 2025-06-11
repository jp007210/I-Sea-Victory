using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public int scoreValue = 1;
    public GameObject healItemPrefab;
    public float dropChance = 0.3f;

    private DayProgressMeter dayProgressMeter;

    void Start()
    {
        currentHealth = maxHealth;
        dayProgressMeter = FindObjectOfType<DayProgressMeter>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (dayProgressMeter != null)
        {
            dayProgressMeter.EnemyWasDefeated();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        TryDropHeal();
        Destroy(gameObject);
    }

    void TryDropHeal()
    {
        if (healItemPrefab != null && Random.value < dropChance)
        {
            Instantiate(healItemPrefab, transform.position, Quaternion.identity);
        }
    }
}
