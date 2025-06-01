using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 15;

    private Transform player;
    private NavMeshAgent agent;
    private float attackTimer;
    private PlayerHealth playerHealth;
    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
                Debug.LogError("PlayerHealth não encontrado no Player!");
        }
        else
        {
            Debug.LogError("Player não encontrado! Verifique a tag 'Player'.");
        }

        attackTimer = 0f;
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }

        RotateTowards(player.position);
    }

    void Attack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"{name} atacou o player causando {damage} de dano!");
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}