using UnityEngine;

public class InventoryPickup : MonoBehaviour
{
    [Header("Inventory Pickup")]
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int amount = 1;

    public bool Collect(PlayerInventory inventory)
    {
        if (inventory == null || itemData == null) return false;

        bool added = inventory.AddItem(itemData, amount);

        if (added) Destroy(gameObject);

        return added;
    }
}
