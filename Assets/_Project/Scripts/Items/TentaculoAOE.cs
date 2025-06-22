using UnityEngine;

public class TentaculoAOE : MonoBehaviour
{
    public void Activate(float radius, float damage, float pushForce, LayerMask hitLayers, float duration)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, hitLayers);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent(out EnemyHealth eh))
                {
                    eh.TakeDamage(damage);
                }

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(dir * pushForce, ForceMode.Impulse);
                }
            }
        }

        Destroy(gameObject, duration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
