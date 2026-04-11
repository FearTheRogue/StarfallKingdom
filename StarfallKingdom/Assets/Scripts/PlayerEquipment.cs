using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Tool Slots")]
    [SerializeField] private InventoryItemData equippedPickaxe;

    public InventoryItemData EquippedPickaxe => equippedPickaxe;
    public bool HasPickaxe => equippedPickaxe != null;

    public bool EquipPickaxe(InventoryItemData itemData)
    {
        if (itemData == null) return false;

        if (itemData.Category != ItemCategory.Tool || itemData.ToolType != ToolType.Pickaxe) return false;

        equippedPickaxe = itemData;
        return true;
    }

    public void UpequipPickaxe()
    {
        equippedPickaxe = null;
    }
}
