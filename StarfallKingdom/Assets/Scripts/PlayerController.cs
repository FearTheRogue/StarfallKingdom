using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;


[RequireComponent (typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private int attackDamage = 1;

    private CustomActions input;
    private CharacterAnimationController animationController;
    private PlayerEffects effects;
    private PlayerMovement movement;

    private Interactable currentTarget;
    private Coroutine currentActionRoutine;
    private bool isBusy;

    private void Awake()
    {
        animationController = GetComponent<CharacterAnimationController>();
        effects = GetComponent<PlayerEffects>();
        movement = GetComponent<PlayerMovement>();

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
    }

    private void Update()
    {
        HandleTargetMovement();
        movement.FaceMovementDirection(isBusy);
        UpdateAnimations();
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

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (!movement.TryHandleClick(isBusy, out Interactable interactable))
        {
            return;
        }

        if (interactable != null)
        {
            currentTarget = interactable;
            return;
        }

        ClearTarget();
    }

    private void HandleTargetMovement()
    {
        if (!HasValidTarget())
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget <= interactionDistance)
        {
            TryInteractWithTarget();
            return;
        }

        if (!isBusy)
        {
            movement.MoveTo(currentTarget.transform.position);
        }
    }

    private void TryInteractWithTarget()
    {
        if (isBusy || !HasValidTarget())
        {
            return;
        }

        movement.Stop();
        movement.FaceTarget(currentTarget.transform.position);

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
        movement.SetStopped(true);
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
        movement.SetStopped(false);
        isBusy = false;
        currentActionRoutine = null;
    }

    private void ApplyAttack()
    {
        if (!HasValidTarget())
        {
            return;
        }

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

    private void UpdateAnimations()
    {
        if (isBusy)
        {
            animationController.SetMoveSpeed(0f);
            return;
        }

        animationController.SetMoveSpeed(movement.CurrentSpeed);
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