using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(PlayerTargeting))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerController : MonoBehaviour
{
    private CustomActions input;
    private CharacterAnimationController animationController;
    private PlayerMovement movement;
    private PlayerTargeting targeting;
    private PlayerInteraction interaction;

    private void Awake()
    {
        animationController = GetComponent<CharacterAnimationController>();
        movement = GetComponent<PlayerMovement>();
        targeting = GetComponent<PlayerTargeting>();
        interaction = GetComponent<PlayerInteraction>();

        input = new CustomActions();
        input.Main.Move.performed += OnMovePerformed;
    }

    private void Update()
    {
        interaction.HandleTargetMovement();
        movement.FaceMovementDirection(interaction.IsBusy);
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
        if (!movement.TryHandleClick(out Interactable interactable)) return;

        if (interactable != null)
        {
            targeting.HandleClickResult(interactable);
            return;
        }

        interaction.CancelCurrentAction();
    }

    private void UpdateAnimations()
    {
        animationController.SetMoveSpeed(movement.CurrentSpeed);
    }

    public void FinishPickupAction()
    {
        interaction.FinishPickupAction();
    }
}