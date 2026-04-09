using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIToggleInput : MonoBehaviour
{
    [SerializeField] private InventoryGridUI inventoryGridUI;
    [SerializeField] private Key toggleKey = Key.Tab;

    private void Update()
    {
        if (inventoryGridUI == null || Keyboard.current == null) return;

        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            inventoryGridUI.ToggleInventory();
        }
    }
}
