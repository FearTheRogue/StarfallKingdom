using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -6f);

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 120f;

    private float yaw;

    private void Start()
    {
        yaw = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();
        FollowTarget();
    }

    private void HandleRotation()
    {
        if (Mouse.current == null || !Mouse.current.middleButton.isPressed) return;

        float mouseX = Mouse.current.delta.ReadValue().x;
        yaw += mouseX * rotationSpeed * Time.deltaTime;
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 rotatedOffset = rotation * offset;

        Vector3 desiredPosition = target.position + rotatedOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
        transform.LookAt(target.position);
    }
}
