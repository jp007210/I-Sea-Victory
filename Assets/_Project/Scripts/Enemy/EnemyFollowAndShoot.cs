using UnityEngine;

public class EnemyFollowAndShoot : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 10f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireCooldown = 2f;
    public float bulletSpeed = 15f;

    private float fireTimer;

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

        // Calcular dist�ncia do player
        float distance = Vector3.Distance(transform.position, player.position);

        // Movimentar em dire��o ao player se estiver longe
        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Olhar para o player
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f; // evitar inclina��o vertical
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        // Atirar se dentro da dist�ncia de ataque
        if (distance <= stopDistance)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireCooldown;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        Vector3 dir = (player.position - shootPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;
    }
}
