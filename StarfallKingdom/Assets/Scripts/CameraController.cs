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
    [SerializeField] private float rotationSpeed = 1f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 0.02f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private float minHeight = 3f;
    [SerializeField] private float maxHeight = 12f;

    private float yaw;

    private void Start()
    {
        yaw = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();
        HandleZoom();
        FollowTarget();
    }

    private void HandleRotation()
    {
        if (Mouse.current == null || !Mouse.current.middleButton.isPressed) return;

        float mouseX = Mouse.current.delta.ReadValue().x;
        yaw += mouseX * rotationSpeed;
    }

    private void HandleZoom()
    {
        float scrollWheel = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Approximately(scrollWheel, 0f)) return;

        distance -= scrollWheel * zoomSpeed;
        height -= scrollWheel * zoomSpeed;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        height = Mathf.Clamp(height, minHeight, maxHeight);
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