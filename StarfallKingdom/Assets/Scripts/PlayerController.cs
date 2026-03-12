using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using Mono.Cecil;
using System.Diagnostics.SymbolStore;

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
    [SerializeField] private float movementThreshold = 0.01f;

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

    private Interactable target;
    private Coroutine actionRoutine;
    private int currentAnimationState;
    private bool isBusy = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cam = Camera.main;

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
    }

    private void Update()
    {
        UpdateTargetMovement();
        FaceCurrentDirection();
        UpdateAnimationState();
    }

    private void HandleClick()
    {
        if (cam == null || Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxClickDistance, clickableLayers))
        {
            return;
        }

        SpawnClickEffect(hit.point);

        if (hit.transform.CompareTag("Interactable"))
        {
            target = hit.transform.GetComponent<Interactable>();
            return;
        }

        ClearTarget();
        agent.SetDestination(hit.point);
    }

    private void SpawnClickEffect(Vector3 position)
    {
        if (clickEffect == null) return;

        Vector3 spawnPosition = position + new Vector3(0f, 0.1f, 0f);
        Instantiate(clickEffect, spawnPosition, clickEffect.transform.rotation);
    }

    private bool HasValidTarget()
    {
        return target != null && target.gameObject != null;
    }

    private void ClearTarget()
    {
        target = null;

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        isBusy = false;
    }

    private void StartActionRoutine(IEnumerator routine)
    {
        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }

        actionRoutine = StartCoroutine(routine);
    }

    private void UpdateTargetMovement()
    {
        if (!HasValidTarget()) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= attackDistance)
        {
            TryInteractWithTarget();
        }
        else if (!isBusy)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void TryInteractWithTarget()
    {
        if (isBusy || !HasValidTarget()) return;

        agent.SetDestination(transform.position);
        FaceTarget(target.transform.position);

        switch (target.interactionType)
        {
            case InteractionTypes.Enemy:
                StartActionRoutine(AttackRoutine());
                break;
            case InteractionTypes.Item:
                StartActionRoutine(PickupRoutine());
                break;
        }
    }

    private IEnumerator AttackRoutine()
    {
        isBusy = true;
        ChangeAnimationState(AttackHash);

        yield return new WaitForSeconds(attackDelay);
        ApplyAttack();

        yield return new WaitForSeconds(Mathf.Max(0f, attackSpeed - attackDelay));
        isBusy = false;
    }

    private IEnumerator PickupRoutine()
    {
        isBusy = true;
        ChangeAnimationState(PickupHash);

        if (HasValidTarget())
        {
            target.InteractWithItem();
            ClearTarget();
        }

        yield return new WaitForSeconds(0.5f);
        isBusy = false;
    }

    private void ApplyAttack()
    {
        if (!HasValidTarget()) return;

        Actor targetActor = target.myActor;

        if (targetActor == null || targetActor.currentHealth <= 0)
        {
            ClearTarget();
            return;
        }

        if (hitEffect != null)
        {
            Instantiate(hitEffect, target.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }

        targetActor.TakeDamage(attackDamage);

        if (targetActor.currentHealth <= 0)
        {
            ClearTarget();
        }
    }

    private void FaceCurrentDirection()
    {
        if (isBusy) return;

        if (agent.velocity.sqrMagnitude <= movementThreshold) return;

        Vector3 direction = agent.velocity.normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    //private void SendAttack()
    //{
    //    if (target == null) return;

    //    if (target.myActor.currentHealth <= 0)
    //    {
    //        target = null;
    //        return;
    //    }

    //    Instantiate(hitEffect, target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    //    target.GetComponent<Actor>().TakeDamage(attackDamage);
    //}

    //private void ResetBusyState()
    //{
    //    isBusy = false;
    //    UpdateAnimationState();
    //}

    //private void FaceMovementDirection()
    //{
    //    if (agent.velocity.sqrMagnitude <= movementThreshold) return;

    //    Vector3 direction = agent.velocity.normalized;
    //    direction.y = 0f;

    //    if (direction.sqrMagnitude <= 0f) return;

    //    Quaternion targetRotation = Quaternion.LookRotation(direction);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
    //}

    private void UpdateAnimationState()
    {
        if (isBusy) return;

        if (agent.velocity.sqrMagnitude <= movementThreshold)
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
        HandleClick();
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
