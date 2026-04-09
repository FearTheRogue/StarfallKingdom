using System.Text;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI inventoryText;

    private void Update()
    {
        if (playerInventory == null || inventoryText == null) return;

        inventoryText.text = BuildInventoryText();
    }

    private string BuildInventoryText()
    {
        if (playerInventory.Items.Count == 0)
            return "Inventory\n- Empty -";

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Inventory");

        foreach (InventorySlot slot in playerInventory.Items)
        {
            if (slot.itemData == null) continue;

            builder.AppendLine($"{slot.itemData.ItemName} x{slot.quantity}");
        }

        return builder.ToString();
    }
}
