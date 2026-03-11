using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int WalkHash = Animator.StringToHash("Walk");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int PickupHash = Animator.StringToHash("Pickup");

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float maxClickDistance = 100f;
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementTreshold = 0.01f;

    [Header("Attack")]
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private ParticleSystem hitEffect;

    private CustomActions input;
    private NavMeshAgent agent;
    private Animator animator;
    private Camera cam;

    private int currentAnimationState;
    private bool playerBusy = false;
    Interactable target;

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
            if (hit.transform.CompareTag("Interactable"))
            {
                target = hit.transform.GetComponent<Interactable>();
                SpawnClickEffect(hit.point);
            }
            else
            {
                target = null;
                agent.SetDestination(hit.point);
                SpawnClickEffect(hit.point);
            }
        }
    }

    private void SpawnClickEffect(Vector3 position)
    {
        if (clickEffect == null) return;

        Vector3 spawnPosition = position + new Vector3(0f, 0.1f, 0f);
        Instantiate(clickEffect, spawnPosition, clickEffect.transform.rotation);
        //Destroy(clickEffect, 2f);
    }

    private void Update()
    {
        FollowTarget();
        FaceMovementDirection();
        UpdateAnimationState();
    }

    private void FollowTarget()
    {
        if (target == null) return;

        if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
        {
            ReachDistance();
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void ReachDistance()
    {
        agent.SetDestination(transform.position);

        if (playerBusy) return;
        playerBusy = true;

        switch (target.interactionType)
        {
            case InteractionTypes.Enemy:
                animator.Play(AttackHash);

                Invoke(nameof(SendAttack), attackDelay);
                Invoke(nameof(ResetBusyState), attackSpeed);
                break;
            case InteractionTypes.Item:
                animator.Play(PickupHash);

                target.InteractWithItem();
                target = null;

                Invoke(nameof(ResetBusyState), 0.5f);
                break;
        }
    }

    private void SendAttack()
    {
        if (target == null) return;

        if (target.myActor.currentHealth <= 0)
        {
            target = null;
            return;
        }

        Instantiate(hitEffect, target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        target.GetComponent<Actor>().TakeDamage(attackDamage);
    }

    private void ResetBusyState()
    {
        playerBusy = false;
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
        if (playerBusy) return;

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
