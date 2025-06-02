using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float turnSpeed = 2f;
    public float drag = 0.1f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float inputForward = 0f;
    private float inputTurn = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
    }

    void Update()
    {
        // Entrada do jogador
        inputForward = Input.GetAxis("Vertical");   // W/S ou ?/?
        inputTurn = Input.GetAxis("Horizontal");    // A/D ou ?/?
    }

    void FixedUpdate()
    {
        // Aplicar acelera��o
        currentSpeed += inputForward * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed); // r� mais lenta

        // Mover o barco para frente
        Vector3 moveDirection = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // Girar o barco
        if (Mathf.Abs(currentSpeed) > 0.1f) // S� rotaciona se estiver em movimento
        {
            float turnAmount = inputTurn * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
            Quaternion turnOffset = Quaternion.Euler(0, turnAmount * 100f, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}