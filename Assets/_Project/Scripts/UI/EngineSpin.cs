using UnityEngine;

public class EngineSpin : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float baseRotationSpeed = 100f;
    public float maxSpeedMultiplier = 3f;

    private float _currentSpeed;

    void Update()
    {
        float healthPercent = playerHealth.currentHealth / (float)playerHealth.maxHealth;

        _currentSpeed = baseRotationSpeed * (1 + (maxSpeedMultiplier * (1 - healthPercent)));

        transform.Rotate(Vector3.forward * _currentSpeed * Time.deltaTime);
    }
}
