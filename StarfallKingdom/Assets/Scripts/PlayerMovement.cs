using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementThreshold = 0.01f;

    [Header("Click Detection")]
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float maxClickDistance = 100f;
}
