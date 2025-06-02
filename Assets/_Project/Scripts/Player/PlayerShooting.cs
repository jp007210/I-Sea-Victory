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
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        harpoonVFX.transform.position = firePoint.position;
        harpoonVFX.transform.rotation = firePoint.rotation;
        harpoonVFX.Play();
        Debug.Log("Tentando atirar...");

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("bulletPrefab ou firePoint está NULO.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("BALA INSTANCIADA COM SUCESSO");
        if (bulletPrefab == null || firePoint == null || harpoonVFX == null)
        {
            Debug.LogWarning("Referência ausente em PlayerShooting!");
            return;
        }

        // Calcula direção do mouse no plano Y
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, firePoint.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - firePoint.position).normalized;

            // Instancia a bala

            // Aplica velocidade
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = direction * bulletSpeed;

            // Configura o VFX
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                VisualEffect vfx = Instantiate(harpoonVFX);
                bulletScript.harpoonVFX = vfx;
                bulletScript.firePoint = firePoint;
            }
            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);

            Destroy(bullet, 3f);
        }
    }
}