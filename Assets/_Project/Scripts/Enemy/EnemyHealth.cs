using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public int maxHealth = 50;
    public float currentHealth;

    [Header("Recompensas")]
    public int scoreValue = 1;
    public GameObject healItemPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.3f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
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
        if (DayProgressMeter.Instance != null)
        {
            DayProgressMeter.Instance.EnemyWasDefeated();
        }

        TryDropHeal();

        Destroy(gameObject);
    }
    void TryDropHeal()
    {
        if (healItemPrefab != null && Random.value < dropChance)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1f; // Eleva meio metro
            Instantiate(healItemPrefab, spawnPos, Quaternion.identity);
        }
    }
}