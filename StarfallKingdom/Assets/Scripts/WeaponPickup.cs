using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private InventoryItemData weaponItemData;

    public void Collect(PlayerInteraction playerInteraction)
    {
        if (playerInteraction == null || weaponItemData == null) return;

        PlayerEquipment equipment = playerInteraction.GetComponent<PlayerEquipment>();

        if (equipment == null) return;

        bool equipped = equipment.EquipWeapon(weaponItemData);

        if (equipped)
        {
            Destroy(gameObject);
        }
    }
}
