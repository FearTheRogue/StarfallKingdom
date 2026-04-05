using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject barVisualRoot;
    [SerializeField] private bool hideWhenFull = false;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogWarning("SprintBarUI: Fill Image is not assigned!");
        }

        if (barVisualRoot == null)
        {
            barVisualRoot = fillImage != null ? fillImage.transform.parent.gameObject : null;
        }
    }

    private void Update()
    {
        if (movement == null || fillImage == null) return;

        fillImage.fillAmount = movement.SprintNormalised;

        if (!hideWhenFull)
        {
            return;
        }

        bool shouldShow = movement.IsSprinting || movement.SprintNormalised < 1f;
        barVisualRoot.SetActive(shouldShow);
    }
}
