using UnityEngine;

public class BoatBobbing : MonoBehaviour
{
    [Header("Movimento Vertical (Bobbing)")]
    public float amplitude = 0.1f;
    public float frequency = 0.5f;

    [Header("Movimento de Rotação (Pitch)")]
    public float pitchAmount = 5f;
    public float pitchFrequency = 0.5f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Movimento Vertical
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Movimento de Rotação (CORRIGIDO)
        float pitch = Mathf.Sin(Time.time * pitchFrequency) * pitchAmount;
        // Trocamos a rotação do eixo X para o eixo Z.
        transform.rotation = startRotation * Quaternion.Euler(0, 0, pitch);
    }
}
