using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int WalkHash = Animator.StringToHash("Walk");

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float maxClickDistance = 100f;
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementTreshold = 0.01f;

    private CustomActions input;
    private NavMeshAgent agent;
    private Animator animator;
    private Camera cam;

    private int currentAnimationState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cam = Camera.main;

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
    }

    private void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        if (cam == null || Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxClickDistance, clickableLayers)) 
        {
            agent.SetDestination(hit.point);
            SpawnClickEffect(hit.point);
        }
    }

    private void SpawnClickEffect(Vector3 position)
    {
        if (clickEffect == null) return;

        Vector3 spawnPosition = position + new Vector3(0f, 0.1f, 0f);
        Instantiate(clickEffect, spawnPosition, clickEffect.transform.rotation);
    }

    private void Update()
    {
        FaceMovementDirection();
        UpdateAnimationState();
    }

    private void FaceMovementDirection()
    {
        if (agent.velocity.sqrMagnitude <= movementTreshold) return;

        Vector3 direction = agent.velocity.normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
    }

    private void UpdateAnimationState()
    {
        if (agent.velocity.sqrMagnitude <= movementTreshold)
        {
            ChangeAnimationState(IdleHash);
        }
        else
        {
            ChangeAnimationState(WalkHash);
        }
    }

    private void ChangeAnimationState(int newState)
    {
        if (currentAnimationState == newState) return;

        animator.Play(newState);
        currentAnimationState = newState;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        ClickToMove();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnDestroy()
    {
        input.Main.Move.performed -= OnMovePerformed;
    }
}
