using UnityEngine;

public class PickaxePickup : MonoBehaviour
{
    [SerializeField] private InventoryItemData pickaxeItemData;

    public void Collect(PlayerInteraction playerInteraction)
    {
        if (playerInteraction == null || pickaxeItemData == null)
            return;
        
        PlayerEquipment equipment = playerInteraction.GetComponent<PlayerEquipment>();

        if (equipment == null) return;

        bool equipped = equipment.EquipPickaxe(pickaxeItemData);

        if (equipped) Destroy(gameObject);
    }
}
