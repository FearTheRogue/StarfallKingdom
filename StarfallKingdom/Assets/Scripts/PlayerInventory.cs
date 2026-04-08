using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private int maxSlots = 12;

    [SerializeField] private List<InventorySlot> items = new List<InventorySlot>();

    public IReadOnlyList<InventorySlot> Items => items;
    public int MaxSlots => maxSlots;

    public bool AddItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0) return false;

        InventorySlot existingSlot = items.Find(slot => slot.itemData == itemData);

        if (existingSlot != null)
        {
            existingSlot.quantity += amount;
            return true;
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory full.");
            return false;
        }

        items.Add(new InventorySlot(itemData, amount));
        return true;
    }

    public bool HasItem(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId)) return false;

        return items.Exists(slot => slot.itemData != null && slot.itemData.ItemId == itemId);
    }

    public bool RemoveItem(string itemId, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0) return false;

        InventorySlot slot = items.Find(x => x.itemData != null && x.itemData.ItemId == itemId);

        if (slot == null || slot.quantity < amount) return false;

        slot.quantity -= amount;

        if (slot.quantity <= 0)
        {
            items.Remove(slot);
        }

        return true;
    }
}
