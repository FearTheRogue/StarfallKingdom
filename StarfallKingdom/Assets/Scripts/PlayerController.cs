using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;

[RequireComponent (typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Click Detection")]
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float maxClickDistance = 100f;

    [Header("Movement")]
    [SerializeField] private float lookRotationSpeed = 8f;
    [SerializeField] private float movementThreshold = 0.01f;

    [Header("Combat")]
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private int attackDamage = 1;

    private CustomActions input;
    private NavMeshAgent agent;
    private CharacterAnimationController animationController;
    private Camera mainCam;

    private Interactable currentTarget;
    private Coroutine currentActionRoutine;
    private bool isBusy;

    private PlayerEffects effects;

    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();

        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<CharacterAnimationController>();
        mainCam = Camera.main;

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
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

    private void Update()
    {
        HandleTargetMovement();
        HandleFacing();
        UpdateAnimations();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (isBusy || mainCam == null || Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxClickDistance, clickableLayers))
        {
            return;
        }

        Vector3 groundEffectPosition = effects.GetGroundEffectPosition(ray, hit, maxClickDistance);
        effects.SpawnClickEffect(groundEffectPosition);

        if (TrySetInteractableTarget(hit))
        {
            return;
        }


        ClearTarget();
        agent.SetDestination(hit.point);
    }

    private bool TrySetInteractableTarget(RaycastHit hit)
    {
        if (!hit.transform.CompareTag("Interactable"))
        {
            return false;
        }

        if (!hit.transform.TryGetComponent(out Interactable interactable))
        {
            return false;
        }

        currentTarget = interactable;

        if (interactable.interactionType == InteractionTypes.Enemy)
        {
            effects.SpawnTargetEffect(interactable.transform);
        }

        return true;
    }

    private void HandleTargetMovement()
    {
        if (!HasValidTarget()) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget <= interactionDistance)
        {
            TryInteractWithTarget();
            return;
        }
        
        if (!isBusy)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }
    
    private void TryInteractWithTarget()
    {
        if (isBusy || !HasValidTarget()) return;

        agent.SetDestination(transform.position);
        FaceTarget(currentTarget.transform.position);

        switch (currentTarget.interactionType)
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
        animationController.TriggerAttack();

        yield return new WaitForSeconds(attackDelay);
        ApplyAttack();

        yield return new WaitForSeconds(Mathf.Max(0f, attackSpeed - attackDelay));
        isBusy = false;
        currentActionRoutine = null;
    }

    private IEnumerator PickupRoutine()
    {
        isBusy = true;
        agent.isStopped = true;
        animationController.TriggerPickup();

        if (HasValidTarget())
        {
            currentTarget.InteractWithItem();
            ClearTarget();
        }

        yield break;
    }

    public void FinishPickupAction()
    {
        Debug.Log("Animation just finished");
        agent.isStopped = false;
        isBusy = false;
        currentActionRoutine = null;
    }

    private void ApplyAttack()
    {
        if (!HasValidTarget()) return;

        Actor targetActor = currentTarget.myActor;

        if (targetActor == null || targetActor.currentHealth <= 0)
        {
            ClearTarget();
            return;
        }

        effects.SpawnHitEffect(currentTarget.transform.position);
        targetActor.TakeDamage(attackDamage);

        if (targetActor.currentHealth <= 0)
        {
            ClearTarget();
        }
    }

    private void HandleFacing()
    {
        if (isBusy || agent.velocity.sqrMagnitude <= movementThreshold) return;

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

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void UpdateAnimations()
    {
        if (isBusy)
        {
            animationController.SetMoveSpeed(0f);
            return;
        }

        animationController.SetMoveSpeed(agent.velocity.magnitude);
    }

    private bool HasValidTarget()
    {
        return currentTarget != null && currentTarget.gameObject != null;
    }

    private void ClearTarget()
    {
        currentTarget = null;
    }

    private void StartActionRoutine(IEnumerator routine)
    {
        if (currentActionRoutine != null)
        {
            StopCoroutine(currentActionRoutine);
        }

        currentActionRoutine = StartCoroutine(routine);
    }
}