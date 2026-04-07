using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

[RequireComponent(typeof(PlayerEffects))]
public class PlayerTargeting : MonoBehaviour
{
    private PlayerEffects effects;
    private Interactable currentTarget;

    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();
    }

    public Interactable CurrentTarget => currentTarget;

    public void HandleClickResult(Interactable interactable)
    {
        if (interactable != null)
        {
            SetTarget(interactable);
            return;
        }

        ClearTarget();
    }

    public void SetTarget(Interactable interactable)
    {
        currentTarget = interactable;

        if (currentTarget != null)
        {
            effects.ShowTargetIndicator(currentTarget.transform);
        }
    }

    public void ClearTarget()
    {
        currentTarget = null;
        effects.HideTargetIndicator();
    }

    public bool HasValidTarget()
    {
        return currentTarget != null && currentTarget.gameObject != null;
    }
}
