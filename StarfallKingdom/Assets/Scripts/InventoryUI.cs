using System.Text;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Display")]
    [SerializeField] private bool startOpen = false;

    private bool isOpen;

    private void Start()
    {
        isOpen = startOpen;
        RefreshVisibility();
    }

    private void Update()
    {
        if (playerInventory == null || inventoryText == null) return;

        if (!isOpen) return;

        inventoryText.text = BuildInventoryText();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        RefreshVisibility();
    }

    public void SetInventoryOpen(bool open)
    {
        isOpen = open;
        RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isOpen);
        }
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
