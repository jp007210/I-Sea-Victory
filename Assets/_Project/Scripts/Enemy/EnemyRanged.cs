using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    public float stopDistance = 8f;
    public float attackCooldown = 2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 15f;

    private Transform player;
    private NavMeshAgent agent;
    private float attackTimer;
    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player não encontrado! Certifique-se que o player tenha tag 'Player'.");

        attackTimer = 0f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
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
                Shoot();
                attackTimer = attackCooldown;
            }
        }

        RotateTowards(player.position);
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        Vector3 direction = (player.position - shootPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed;
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
    }
}