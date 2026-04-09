using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIToggleInput : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Key toggleKey = Key.Tab;

    private void Update()
    {
        if (inventoryUI == null || Keyboard.current == null) return;

        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            inventoryUI.ToggleInventory();
        }
    }
}
