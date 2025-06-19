using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public VisualEffect harpoonVFX;
    public float bulletSpeed = 25f;

    public AudioClip shootClip;
    public AudioSource audioSource;

    void Update()
    {
        if (PauseMenuManager.GameIsPaused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || harpoonVFX == null || audioSource == null || shootClip == null)
        {
            Debug.LogWarning("PlayerShooting: Algumas referências importantes estão faltando no Inspector! " +
                             "Certifique-se de que Bullet Prefab, Fire Point, Harpoon VFX, Audio Source e Shoot Clip estão atribuídos.");
            return;
        }

        VisualEffect instantiatedMuzzleVFX = Instantiate(harpoonVFX, firePoint.position, firePoint.rotation);
        instantiatedMuzzleVFX.Play();
        Destroy(instantiatedMuzzleVFX.gameObject, 2f);

        audioSource.PlayOneShot(shootClip);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, firePoint.position);
        Vector3 direction = Vector3.forward;
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            direction = (targetPoint - firePoint.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            VisualEffect bulletTrailVFX = Instantiate(harpoonVFX);
            bulletScript.harpoonVFX = bulletTrailVFX;
            bulletScript.firePoint = firePoint;
        }

        Destroy(bullet, 3f);
    }
}
