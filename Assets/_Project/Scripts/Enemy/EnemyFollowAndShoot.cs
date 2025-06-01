using UnityEngine;

public class EnemyFollowAndShoot : MonoBehaviour
{
    public enum EnemyAttackType
    {
        Ranged,
        Melee
    }
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 10f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireCooldown = 2f;
    public float bulletSpeed = 15f;

    public float meleeDamage = 10f;
    public float meleeCooldown = 1f;

    public EnemyAttackType attackType = EnemyAttackType.Ranged;

    private float fireTimer;
    private float meleeTimer;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Movimentação
        if (attackType == EnemyAttackType.Ranged)
        {
            if (distance > stopDistance)
            {
                MoveTowardsPlayer();
            }
        }
        else if (attackType == EnemyAttackType.Melee)
        {
            MoveTowardsPlayer();
        }

        // Rotacionar para o player
        LookAtPlayer();

        // Ataque
        if (attackType == EnemyAttackType.Ranged && distance <= stopDistance)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireCooldown;
            }
        }

        if (attackType == EnemyAttackType.Melee && distance <= 1.5f) // ajuste a distância de contato
        {
            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0f)
            {
                MeleeAttack();
                meleeTimer = meleeCooldown;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void LookAtPlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        Vector3 dir = (player.position - shootPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = dir * bulletSpeed;
    }

    void MeleeAttack()
    {
        Debug.Log("Melee Attack on player!");
        // Aqui você aplica o dano no player
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
    }
}