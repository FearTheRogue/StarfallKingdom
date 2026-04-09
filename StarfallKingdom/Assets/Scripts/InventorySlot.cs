using System.Diagnostics.Contracts;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public InventoryItemData itemData;
    public int quantity;

    public InventorySlot(InventoryItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
}
