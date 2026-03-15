using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float height = 6f;
    [SerializeField] private Vector3 lookOffset = new Vector3(0f, 1.5f, 0f);

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
        yaw += mouseX * rotationSpeed;
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 orbitOffset = rotation * Vector3.back * distance;
        Vector3 desiredPosition = target.position + orbitOffset + Vector3.up * height;

        transform.position = desiredPosition;
        transform.LookAt(target.position + lookOffset);
    }
}
