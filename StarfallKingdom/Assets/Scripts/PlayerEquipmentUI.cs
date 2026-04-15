using UnityEngine;

public class PlayerEquipmentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private EquipmentSlotUI pickaxeSlotUI;
    [SerializeField] private EquipmentSlotUI weaponSlotUI;

    private void Update()
    {
        if (playerEquipment == null) return;

        if (pickaxeSlotUI != null)
        {
            pickaxeSlotUI.SetSlot(playerEquipment.EquippedPickaxe, "No Pickaxe");
        }

        if (weaponSlotUI != null)
        {
            weaponSlotUI.SetSlot(null, "No Weapon");
        }
    }
}