using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Image fillImage;
    [SerializeField] private bool hideWhenFull = false;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogWarning("SprintBarUI: Fill Image is not assigned!");
        }
    }

    private void Update()
    {
        if (movement == null || fillImage == null) return;

        fillImage.fillAmount = movement.SprintNormalised;

        if (hideWhenFull)
        {
            bool shouldShow = movement.IsSprinting || movement.SprintNormalised < 1f;
            gameObject.SetActive(shouldShow); 
        }
    }
}
