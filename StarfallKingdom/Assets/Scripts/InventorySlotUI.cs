using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image iconImage;

    public void SetSlot(InventorySlot slot)
    {
        if (slot == null || slot.itemData == null)
        {
            ClearSlot();
            return;
        }

        if (itemNameText != null)
        {
            itemNameText.text = slot.itemData.ItemName;
        }

        if (quantityText != null)
        {
            quantityText.text = slot.quantity > 1 ? $"x{slot.quantity}" : string.Empty;
        }

        if (iconImage != null)
        {
            iconImage.enabled = slot.itemData.Icon != null;
            iconImage.sprite = slot.itemData.Icon;
        }
    }

    public void ClearSlot()
    {
        if (itemNameText != null)
        {
            itemNameText.text = string.Empty;
        }

        if (quantityText != null)
        {
            quantityText.text = string.Empty;
        }

        if (iconImage != null)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
        }
    }
}
