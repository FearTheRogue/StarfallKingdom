using UnityEngine;

public enum ItemCategory
{
    Material,
    Tool,
    Weapon,
    Quest,
    Valuable
}

public enum ToolType
{
    None,
    Pickaxe
}

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    [Header("Item Into")]
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemCategory category;

    [Header("Tool Settings")]
    [SerializeField] private ToolType toolType = ToolType.None;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponType weaponType = WeaponType.None;
    [SerializeField] private int weaponDamage = 1;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public ItemCategory Category => category;
    public ToolType ToolType => toolType;
    public WeaponType WeaponType => weaponType;
    public int WeaponDamage => weaponDamage;
}
