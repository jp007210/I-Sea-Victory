using UnityEngine;

public class NavioPirataMovement : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationRadius = 15f;
    public float rotationSpeed = 30f;
    public float height = 1.5f; // Altura fixa do navio acima do "mar"

    public NavioPirataAttack attackSystem;

    private float currentAngle;

    void Update()
    {
        if (attackSystem != null && attackSystem.isVulnerable)
            return;

        if (centerPoint == null) return;

        // Movimento circular
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * rotationRadius;

        // Aplica a altura ao movimento
        Vector3 newPos = centerPoint.position + offset;
        newPos.y = height; // fixa o Y

        transform.position = newPos;

        // Rota o navio com a lateral voltada para o centro
        Vector3 toCenter = centerPoint.position - transform.position;
        toCenter.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(toCenter);
        transform.rotation = lookRotation * Quaternion.Euler(0f, 90f, 0f);
    }
}