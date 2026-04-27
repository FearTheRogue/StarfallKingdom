using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(CharacterAnimationController))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerEquipment))]
[RequireComponent(typeof(PlayerToolVisuals))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private int punchDamage = 1;

    [Header("Tools")]
    [SerializeField] private bool debugBypassPickaxeRequirement = false;

    private PlayerMovement movement;
    private PlayerEffects effects;
    private CharacterAnimationController animationController;
    private PlayerTargeting targeting;
    private PlayerInventory inventory;
    private PlayerEquipment equipment;
    private PlayerToolVisuals toolVisuals;

    private Coroutine currentActionRoutine;
    private bool isBusy;

    public PlayerInventory Inventory => inventory;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        effects = GetComponent<PlayerEffects>();
        animationController = GetComponent<CharacterAnimationController>();
        targeting = GetComponent<PlayerTargeting>();
        inventory = GetComponent<PlayerInventory>();
        equipment = GetComponent<PlayerEquipment>();
        toolVisuals = GetComponent<PlayerToolVisuals>();
    }

    public bool IsBusy => isBusy;
    public float CurrentMoveSpeed => isBusy ? 0f : movement.CurrentSpeed;

    public void HandleTargetMovement()
    {
        if (!targeting.HasValidTarget()) return;

        Interactable currentTarget = targeting.CurrentTarget;
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
        if (toolVisuals != null)
        {
            toolVisuals.HideAll();
        }

        movement.SetStopped(false);
        isBusy = false;
        currentActionRoutine = null;
    }

    public void CancelCurrentAction()
    {
        if (currentActionRoutine != null)
        {
            StopCoroutine(currentActionRoutine);
            currentActionRoutine = null;
        }

        if (toolVisuals != null)
        {
            toolVisuals.HideAll();
        }

        isBusy = false;
        movement.SetStopped(false);
        targeting.ClearTarget();
    }

    private void TryInteractWithTarget()
    {
        if (isBusy || !targeting.HasValidTarget()) return;

        Interactable currentTarget = targeting.CurrentTarget;

        movement.Stop();
        movement.FaceTarget(currentTarget.transform.position);

        switch (currentTarget.InteractionType)
        {
            case InteractionTypes.Enemy:
                StartActionRoutine(AttackRoutine());
                break;

            case InteractionTypes.Item:
                StartActionRoutine(PickupRoutine());
                break;
            case InteractionTypes.Resource:
                if (!CanMineResources())
                {
                    Debug.Log("Cannot mine resources: player doesn't have a pickaxe.");
                    targeting.ClearTarget();
                    return;
                }

                StartActionRoutine(ResourceRoutine());
                break;
        }
    }

    private bool CanMineResources()
    {
        return (equipment != null && equipment.HasPickaxe) || debugBypassPickaxeRequirement;
    }

    private IEnumerator AttackRoutine()
    {
        isBusy = true;
        movement.SetStopped(true);

        if (toolVisuals != null && equipment != null && equipment.HasWeapon)
        {
            toolVisuals.ShowWeapon();
        }

        animationController.TriggerAttack();

        yield return new WaitForSeconds(attackDelay);

        ApplyAttack();

        yield return new WaitForSeconds(Mathf.Max(0f, attackSpeed - attackDelay));

        if (toolVisuals != null)
        {
            toolVisuals.HideWeapon();
        }

        movement.SetStopped(false);
        isBusy = false;
        currentActionRoutine = null;
    }

    private IEnumerator PickupRoutine()
    {
        isBusy = true;
        movement.SetStopped(true);
        animationController.TriggerPickup();

        if (targeting.HasValidTarget())
        {
            targeting.CurrentTarget.InteractWithItem(this);
            targeting.ClearTarget();
        }

        yield break;
    }

    private IEnumerator ResourceRoutine()
    {
        isBusy = true;
        movement.SetStopped(true);

        if (toolVisuals != null && equipment != null && equipment.HasPickaxe)
        {
            toolVisuals.ShowPickaxe();
        }

        animationController.TriggerAttack();

        yield return new WaitForSeconds(attackDelay);

        ApplyResourceHit();

        yield return new WaitForSeconds(Mathf.Max(0f, attackSpeed - attackDelay));

        if (toolVisuals != null)
        {
            toolVisuals.HideAll();
        }

        isBusy = false;
        movement.SetStopped(false);
        currentActionRoutine = null;
    }

    private void ApplyAttack()
    {
        if (!targeting.HasValidTarget()) return;

        Interactable currentTarget = targeting.CurrentTarget;
        Actor targetActor = currentTarget.MyActor;

        if (targetActor == null || targetActor.currentHealth <= 0)
        {
            targeting.ClearTarget();
            return;
        }

        effects.SpawnHitEffect(currentTarget.transform.position);
        targetActor.TakeDamage(GetCurrentAttackDamage());

        if (targetActor.currentHealth <= 0)
        {
           targeting.ClearTarget();
        }
    }

    private int GetCurrentAttackDamage()
    {
        if (equipment != null && equipment.EquippedWeapon != null)
        {
            return equipment.EquippedWeapon.WeaponDamage;
        }

        return punchDamage;
    }

    private void ApplyResourceHit()
    {
        if (!targeting.HasValidTarget()) return;

        if (!targeting.CurrentTarget.TryGetComponent(out OreNode oreNode))
        {
            targeting.ClearTarget();
            return;
        }

        oreNode.MineHit();

        if (oreNode.IsDepleted)
        {
            targeting.ClearTarget();
        }
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
