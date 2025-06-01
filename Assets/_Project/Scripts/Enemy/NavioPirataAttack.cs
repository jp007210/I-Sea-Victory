using UnityEngine;
using System.Collections;

public class NavioPirataAttack : MonoBehaviour
{
    public Transform[] firePoints;               
    public GameObject warningPrefab;
    public GameObject projectilePrefab;

    public float timeBetweenAttacks = 2f;
    public float warningDuration = 0.7f;
    public float projectileSpeed = 20f;

    public int shotsBeforeCooldown = 3;
    public float cooldownDuration = 4f;

    private int shotsFired = 0;
    private float attackTimer = 0f;
    private bool isCoolingDown = false;

    public bool isVulnerable { get; private set; } = false;

    void Update()
    {
        if (isCoolingDown)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= cooldownDuration)
            {
                attackTimer = 0f;
                isCoolingDown = false;
                isVulnerable = false;
                shotsFired = 0;
            }
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            attackTimer = 0f;
            StartCoroutine(FireAllWithWarning());
        }
    }

    IEnumerator FireAllWithWarning()
    {
        // ?? Mostra aviso em todos os fire points
        foreach (Transform firePoint in firePoints)
        {
            GameObject warning = Instantiate(warningPrefab, firePoint.position, firePoint.rotation);
            Destroy(warning, warningDuration);
        }

        yield return new WaitForSeconds(warningDuration);

        // ?? Dispara de todos os fire points
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }
        }

        shotsFired++;
        if (shotsFired >= shotsBeforeCooldown)
        {
            isCoolingDown = true;
            isVulnerable = true;
        }
    }
}
