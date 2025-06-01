using UnityEngine;

public class NavioPirataMovement : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationRadius = 15f;
    public float rotationSpeed = 30f;

    public NavioPirataAttack attackSystem; // <-- Referência ao ataque para checar vulnerabilidade

    private float currentAngle;

    void Update()
    {
        // ❌ Se estiver vulnerável, não se move
        if (attackSystem != null && attackSystem.isVulnerable)
            return;

        if (centerPoint == null) return;

        // Movimento circular
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * rotationRadius;
        transform.position = centerPoint.position + offset;

        // Sempre olha para o centro
        Vector3 lookDir = centerPoint.position - transform.position;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}