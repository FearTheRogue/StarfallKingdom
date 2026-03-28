using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
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
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementThreshold = 0.01f;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float maxSprintTime = 5f;
    [SerializeField] private float sprintDrainRate = 1f;
    [SerializeField] private float sprintRecoveryRate = 0.75f;

    private NavMeshAgent agent;
    private PlayerEffects effects;
    private Camera mainCamera;

    private bool isSprinting;
    private float currentSprintTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        effects = GetComponent<PlayerEffects>();
        mainCamera = Camera.main;

        agent.speed = walkSpeed;
        currentSprintTime = maxSprintTime;
    }

    private void Update()
    {
        HandleSprintStamina();
    }

    public bool TryHandleClick(out Interactable interactable)
    {
        interactable = null;

        if (mainCamera == null || Mouse.current == null) return false;

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

    public void ToggleSprint()
    {
        if (isSprinting)
        {
            SetSprint(false);
            return;
        }
        
        if (currentSprintTime > 0f)
        {
            SetSprint(true);
        }
    }

    public void SetSprint(bool sprinting)
    {
        if (sprinting && currentSprintTime <= 0f)
        {
            sprinting = false;
        }

        isSprinting = !isSprinting;
        agent.speed = isSprinting ? sprintSpeed : walkSpeed;
    }

    private void HandleSprintStamina()
    {
        bool isMoving = agent.velocity.sqrMagnitude > movementThreshold;

        if (isSprinting && isMoving)
        {
            currentSprintTime -= sprintDrainRate * Time.deltaTime;

            if (currentSprintTime <= 0f)
            {
                currentSprintTime = 0f;
                SetSprint(false);
            }

            return;
        }

        if (currentSprintTime < maxSprintTime)
        {
            currentSprintTime += sprintRecoveryRate * Time.deltaTime;
            currentSprintTime = Mathf.Min(currentSprintTime, maxSprintTime);
        }
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
    public bool IsSprinting => isSprinting;
    public float CurrentSprintTime => currentSprintTime;
    public float MaxSprintTime => maxSprintTime;
    public float SprintNormalised => maxSprintTime > 0f ? currentSprintTime / maxSprintTime : 0f;
}
