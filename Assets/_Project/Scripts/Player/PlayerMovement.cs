using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float turnSpeed = 100f;
    public float drag = 0.5f;

    private Rigidbody rb;
    private float inputForward = 0f;
    private float inputTurn = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        rb.angularDamping = 5f;

        // ✅ Travar rotação física em X e Z no código também, por segurança
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        inputForward = Input.GetAxis("Vertical");
        inputTurn = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // ✅ Aceleração baseada na frente do barco
        if (Mathf.Abs(inputForward) > 0.01f)
        {
            Vector3 force = transform.forward * inputForward * acceleration;
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // ✅ Rotação controlada apenas por input
        if (rb.linearVelocity.magnitude > 0.5f && Mathf.Abs(inputTurn) > 0.01f)
        {
            float turn = inputTurn * turnSpeed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.Euler(0, turn, 0);
            rb.MoveRotation(rb.rotation * rotation);
        }

        // ✅ Limitar velocidade máxima manualmente
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            flatVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(flatVel.x, rb.linearVelocity.y, flatVel.z);
        }
    }
}