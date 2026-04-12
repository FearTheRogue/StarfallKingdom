using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private GameObject emptyStateObject;

    public void SetSlot(InventoryItemData itemData, string emptyLabel = "Empty")
    {
        bool hasItem = itemData != null;

        if (iconImage != null)
        {
            iconImage.enabled = hasItem && itemData.Icon != null;
            iconImage.sprite = hasItem ? itemData.Icon : null;
        }

        if (labelText != null)
        {
            labelText.text = hasItem ? itemData.ItemName : emptyLabel;
        }

        if (emptyStateObject != null)
        {
            emptyStateObject.SetActive(!hasItem);
        }
    }
}
