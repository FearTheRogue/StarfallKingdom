using UnityEngine;

public class PlayerEquipmentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private GameObject pickaxeSlotRoot;
    [SerializeField] private EquipmentSlotUI pickaxeSlotUI;
    [SerializeField] private GameObject weaponSlotRoot;
    [SerializeField] private EquipmentSlotUI weaponSlotUI;

    private void Update()
    {
        if (playerEquipment == null) return;

        bool hasPickaxe = playerEquipment.EquippedPickaxe != null;

        if (pickaxeSlotRoot != null)
        {
            pickaxeSlotRoot.SetActive(hasPickaxe);
        }

        if (pickaxeSlotUI != null && hasPickaxe)
        {
            pickaxeSlotUI.SetSlot(playerEquipment.EquippedPickaxe, "No Pickaxe");
        }

        if (weaponSlotUI != null)
        {
            weaponSlotUI.SetSlot(null, "No Weapon");
        }

        if (weaponSlotRoot != null)
        {
            weaponSlotRoot.SetActive(true);
        }

    }
}