using UnityEngine;

public class GroundItemVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visualPivot;

    [Header("Rotation")]
    [SerializeField] private bool shouldRotate = true;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Oscillation")]
    [SerializeField] private bool shouldOscillate = true;
    [SerializeField] private float oscillationHeight = 0.25f;
    [SerializeField] private float oscillationSpeed = 2f;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        if (visualPivot == null && transform.childCount > 0)
            visualPivot = transform.GetChild(0);

        if (visualPivot != null)
            startLocalPosition = visualPivot.localPosition;
    }

    private void Update()
    {
        if (visualPivot == null) return;

        HandleRotation();
        HandleOscillation();
    }

    private void HandleRotation()
    {
        if (!shouldRotate) return;

        visualPivot.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleOscillation()
    {
        if (!shouldOscillate)
        {
            visualPivot.localPosition = startLocalPosition;
            return;
        }

        float yOffset = (Mathf.Sin(Time.time * oscillationSpeed) + 1f) * 0.5f * oscillationHeight;

        visualPivot.localPosition = new Vector3(startLocalPosition.x, startLocalPosition.y + yOffset, startLocalPosition.z);
    }
}