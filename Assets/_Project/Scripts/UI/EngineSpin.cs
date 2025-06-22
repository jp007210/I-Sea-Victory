using UnityEngine;

public class EngineSpin : MonoBehaviour
{
    public PlayerStats playerStats;  // Agora refere-se ao PlayerStats
    public float baseRotationSpeed = 100f;
    public float maxSpeedMultiplier = 3f;

    private float _currentSpeed;

    void Update()
    {
        if (playerStats == null) return;

        float healthPercent = playerStats.currentHealth / (float)playerStats.baseMaxHealth;

        _currentSpeed = baseRotationSpeed * (1 + (maxSpeedMultiplier * (1 - healthPercent)));

        transform.Rotate(Vector3.forward * _currentSpeed * Time.deltaTime);
    }
}