using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerEffects))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Click Detection")]
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float maxClickDistance = 100f;

    [Header("Movement")]
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementThreshold = 0.01f;

    private NavMeshAgent agent;
    private PlayerEffects effects;
    private Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        effects = GetComponent<PlayerEffects>();
        mainCamera = Camera.main;
    }

    public bool TryHandleClick(bool isBusy, out Interactable interactable)
    {
        interactable = null;

        if (isBusy || mainCamera == null || Mouse.current == null) return false;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, maxClickDistance, clickableLayers)) return false;

        effects.SpawnGroundClickEffect(ray, hit, maxClickDistance);

        if (hit.transform.CompareTag("Interactable") && hit.transform.TryGetComponent(out Interactable foundInteractable))
        {
            interactable = foundInteractable;

            if (foundInteractable.interactionType == InteractionTypes.Enemy)
            {
                effects.SpawnTargetEffect(foundInteractable.transform);
            }

            return true;
        }

        agent.SetDestination(hit.point);
        return true;
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
    }

    public void SetStopped(bool stopped)
    {
        agent.isStopped = stopped;
    }

    public void FaceMovementDirection(bool isBusy)
    {
        if (isBusy || agent.velocity.sqrMagnitude <= movementThreshold) return;

        Vector3 direction = agent.velocity.normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
    }

    public void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public float CurrentSpeed => agent.velocity.magnitude;
}
