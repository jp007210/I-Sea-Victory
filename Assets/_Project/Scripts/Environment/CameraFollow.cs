using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -12);
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float tiltAngle = 20f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, targetRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, rotationSpeed * Time.deltaTime);
    }
}