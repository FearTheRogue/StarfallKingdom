using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI labelText;

    [Header("Empty State")]
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private string emptyLabel = "No Pickaxe";

    public void SetSlot(InventoryItemData itemData, string emptyLabel = "Empty")
    {
        bool hasItem = itemData != null;

        if (iconImage != null)
        {
            iconImage.enabled = hasItem && itemData.Icon != null;
            iconImage.sprite = hasItem ? itemData.Icon : null;
        }
        else
        {
            iconImage.enabled = emptySprite != null;
            iconImage.sprite = emptySprite;
        }

        if (labelText != null)

        {
            labelText.text = hasItem ? itemData.ItemName : emptyLabel;
        }
    }
}
