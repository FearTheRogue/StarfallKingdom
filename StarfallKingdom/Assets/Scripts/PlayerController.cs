using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;


[RequireComponent (typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerController : MonoBehaviour
{
    private CustomActions input;
    private CharacterAnimationController animationController;
    private PlayerMovement movement;
    private PlayerCombat combat;

    private void Awake()
    {
        animationController = GetComponent<CharacterAnimationController>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
    }

    private void Update()
    {
        combat.HandleTargetMovement();
        movement.FaceMovementDirection(combat.IsBusy);
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
        if (!movement.TryHandleClick(out Interactable interactable))
        {
            return;
        }

        if (interactable != null)
        {
            combat.HandleClickResult(interactable);
            return;
        }

        combat.CancelCurrentAction();
    }

    private void UpdateAnimations()
    {
        animationController.SetMoveSpeed(movement.CurrentSpeed);
    }
}