using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;        // The player to follow
    public float distance = 4.0f;   // Distance behind player
    public float height = 2.0f;     // Height above player
    public float rotationSpeed = 3f;
    public float smoothSpeed = 0.15f;

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        if (!target) return;

        // Get mouse input for rotation
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // Limit up/down angle

        // Calculate rotation and position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPos = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
