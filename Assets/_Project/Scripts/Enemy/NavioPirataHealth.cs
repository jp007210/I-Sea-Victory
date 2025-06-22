using UnityEngine;

public class NavioPirataHealth : MonoBehaviour
{
    public int maxHealth = 200;
    private float currentHealth;

    public NavioPirataAttack attackSystem;
    public int GetCurrentHealth()
    {
        return (int)currentHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (attackSystem != null && !attackSystem.isVulnerable)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("MiniBoss derrotado!");
        EnemyManager.Instance.BossDefeated();
    }
}
