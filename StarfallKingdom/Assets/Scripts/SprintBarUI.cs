using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup barCanvasGroup;
    [SerializeField] private bool hideWhenFull = true;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogWarning("SprintBarUI: Fill Image is not assigned!");
        }

        if (barCanvasGroup == null)
        {
            Debug.LogWarning("SprintBarUI: Bar CanvasGroup is not assigned!");
        }
    }

    private void Update()
    {
        if (movement == null || fillImage == null || barCanvasGroup == null) return;

        Debug.Log($"Sprint: {movement.IsSprinting}, Value: {movement.SprintNormalised}");

        fillImage.fillAmount = movement.SprintNormalised;

        if (!hideWhenFull)
        {
            barCanvasGroup.alpha = 1.0f;
            return;
        }

        bool shouldShow = movement.SprintNormalised < 1f;
        barCanvasGroup.alpha = shouldShow ? 1.0f : 0.0f;
        barCanvasGroup.interactable = false;
        barCanvasGroup.blocksRaycasts = false;
    }
}
