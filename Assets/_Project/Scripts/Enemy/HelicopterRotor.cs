using UnityEngine;

public class HelicopterRotor : MonoBehaviour
{
    public float rotationSpeed = 720f; // graus por segundo

    void Update()
    {
        // Gira a hélice no eixo Y local (vertical)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }
}