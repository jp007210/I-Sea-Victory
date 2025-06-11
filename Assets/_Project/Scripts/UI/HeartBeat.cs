using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float minBPM = 40f;
    public float maxBPM = 180f;
    public float pulseScale = 1.2f;

    Vector3 initialScale;
    float timer = 0f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        float healthPercent = playerHealth.currentHealth / (float)playerHealth.maxHealth;
        float bpm = Mathf.Lerp(maxBPM, minBPM, healthPercent);
        float cycle = 60f / bpm;
        timer += Time.deltaTime;
        float t = (timer % cycle) / cycle;

        float scale = 1f;
        if (t < 0.15f) // TU
            scale = Mathf.Lerp(1f, pulseScale, t / 0.15f);
        else if (t < 0.3f) // TUM
            scale = Mathf.Lerp(pulseScale, 1.1f, (t - 0.15f) / 0.15f);
        else // Pausa
            scale = Mathf.Lerp(1.1f, 1f, (t - 0.3f) / 0.7f);

        transform.localScale = initialScale * scale;
    }
}
