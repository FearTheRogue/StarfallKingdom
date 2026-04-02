using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private int attackDamage = 1;

    private PlayerMovement movement;
    private PlayerEffects effects;
    private CharacterAnimationController animationController;

    private Interactable currentTarget;
    private Coroutine currentActionRoutine;
    private bool isBusy;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        effects = GetComponent<PlayerEffects>();
        animationController = GetComponent<CharacterAnimationController>();
    }

    public bool IsBusy => isBusy;
    public float CurrentMoveSpeed => isBusy ? 0f : movement.CurrentSpeed;

    public void HandleClickResult(Interactable interactable)
    {
        if (interactable != null)
        {
            currentTarget = interactable;
            effects.ShowTargetIndicator(interactable.transform);
            return;
        }

        ClearTarget();
    }

    public void HandleTargetMovement()
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
            movement.MoveTo(currentTarget.transform.position);
        }
    }

    public void FinishPickupAction()
    {
        movement.SetStopped(false);
        isBusy = false;
        currentActionRoutine = null;
    }

    private void TryInteractWithTarget()
    {
        if (isBusy || !HasValidTarget()) return;

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
            case InteractionTypes.Resource:
                StartActionRoutine(ResourceRoutine());
                break;
        }
    }

    public void CancelCurrentAction()
    {
        if (currentActionRoutine != null)
        {
            StopCoroutine(currentActionRoutine);
            currentActionRoutine = null;
        }

        isBusy = false;
        movement.SetStopped(false);
        ClearTarget();
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

    private IEnumerator ResourceRoutine()
    {
        isBusy = true;
        movement.SetStopped(true);
        animationController.TriggerAttack();

        yield return new WaitForSeconds(attackDelay);

        ApplyResourceHit();

        yield return new WaitForSeconds(Mathf.Max(0f, attackSpeed - attackDelay));

        isBusy = false;
        movement.SetStopped(false);

        if (!HasValidTarget())
        {
            currentActionRoutine = null;
        }
    }

    private void ApplyResourceHit()
    {
        if (!HasValidTarget()) return;

        if (!currentTarget.TryGetComponent(out OreNode oreNode))
        {
            ClearTarget();
            return;
        }

        oreNode.MineHit();

        if (oreNode.IsDepleted)
        {
            ClearTarget();
        }
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

    private bool HasValidTarget()
    {
        return currentTarget != null && currentTarget.gameObject != null;
    }

    private void ClearTarget()
    {
        currentTarget = null;
        effects.HideTargetIndicator();
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
