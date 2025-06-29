using UnityEngine;

public class KrakenHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 300;
    private float currentHealth;

    [Header("Vulnerabilidade por Distância")]
    public Transform krakenCenter;
    public float vulnerableRadius = 10f;
    public LayerMask playerLayer;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsPlayerInVulnerableRange())
        {
            Debug.Log("Player está fora do alcance para causar dano ao Kraken.");
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    bool IsPlayerInVulnerableRange()
    {
        Vector3 center = krakenCenter != null ? krakenCenter.position : transform.position;

        Collider[] players = Physics.OverlapSphere(center, vulnerableRadius, playerLayer);
        foreach (Collider col in players)
        {
            if (col.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void Die()
    {
        Debug.Log("Kraken derrotado!");
        Destroy(gameObject);

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.BossDefeated();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (krakenCenter == null) krakenCenter = transform;
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(krakenCenter.position, vulnerableRadius);
    }
}