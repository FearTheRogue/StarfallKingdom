using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Tool Slots")]
    [SerializeField] private InventoryItemData equippedPickaxe;

    [Header("Weapon Slots")]
    [SerializeField] private InventoryItemData equippedWeapon;

    public InventoryItemData EquippedPickaxe => equippedPickaxe;
    public InventoryItemData EquippedWeapon => equippedWeapon;

    public bool HasPickaxe => equippedPickaxe != null;
    public bool HasWeapon => equippedWeapon != null;

    public bool EquipPickaxe(InventoryItemData itemData)
    {
        if (itemData == null) return false;

        if (itemData.Category != ItemCategory.Tool || itemData.ToolType != ToolType.Pickaxe) return false;

        equippedPickaxe = itemData;
        return true;
    }

    public bool EquipWeapon(InventoryItemData itemData)
    {
        if(itemData == null) return false;

        if(itemData.Category != ItemCategory.Weapon || itemData.WeaponType == WeaponType.Sword) return false;

        equippedWeapon = itemData;
        return true;
    }

    public void UpequipPickaxe()
    {
        equippedPickaxe = null;
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
    }
}
