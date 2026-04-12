using UnityEngine;

public class PlayerEquipmentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private EquipmentSlotUI pickaxeSlotUI;

    private void Update()
    {
        if (playerEquipment == null || pickaxeSlotUI == null) return;

        pickaxeSlotUI.SetSlot(playerEquipment.EquippedPickaxe, "No Pickaxe");
    }
}
