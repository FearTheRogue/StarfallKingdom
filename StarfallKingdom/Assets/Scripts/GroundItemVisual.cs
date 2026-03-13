using UnityEngine;

public class GroundItemVisual : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private bool shouldRotate = true;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Oscillation")]
    [SerializeField] private bool shouldOscillate = true;
    [SerializeField] private float oscillationHeight = 0.25f;
    [SerializeField] private float oscillationSpeed = 2f;

    [Header("Item")]
    [SerializeField] private Transform itemVisual;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        if (itemVisual == null)
        {
            itemVisual = transform.GetChild(0);
        }

        startLocalPosition = itemVisual.localPosition;
    }

    private void Update()
    {
        if (itemVisual == null)
        {
            return;
        }

        HandleRotation();
        HandleOscillation();
    }

    private void HandleRotation()
    {
        if (!shouldRotate) return;

        itemVisual.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleOscillation()
    {
        if (!shouldOscillate)
        {
            itemVisual.localPosition = startLocalPosition;
            return;
        }

        float yOffset = (Mathf.Sin(Time.time * oscillationSpeed) + 1f) * 0.5f * oscillationHeight;

        itemVisual.localPosition = new Vector3(
            startLocalPosition.x,
            startLocalPosition.y + yOffset,
            startLocalPosition.z
        );
    }
}