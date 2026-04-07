using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerEffects))]
[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerCombat : MonoBehaviour
{
    private Interactable currentTarget;

    public void HandleClickResult(Interactable interactable)
    {
        if (interactable != null)
        {
            currentTarget = interactable;
            //effects.ShowTargetIndicator(interactable.transform);
            return;
        }

        ClearTarget();
    }

    private void TryCollectSpecialItem(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        if (targetObject.TryGetComponent(out PickaxePickup pickaxePickup))
        {
           // pickaxePickup.Collect(this);
        }
    }

    private bool HasValidTarget()
    {
        return currentTarget != null && currentTarget.gameObject != null;
    }

    private void ClearTarget()
    {
        currentTarget = null;
        //effects.HideTargetIndicator();
    }
}
