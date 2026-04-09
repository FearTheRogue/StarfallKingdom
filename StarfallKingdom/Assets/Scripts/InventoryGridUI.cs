using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class InventoryGridUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Display")]
    [SerializeField] private bool startOpen = false;

    private readonly List<InventorySlotUI> spawnedSlots = new List<InventorySlotUI>();
    private bool isOpen;

    private void Start()
    {
        isOpen = startOpen;
        RefreshVisibility();
        RebuildGrid();
    }

    private void Update()
    {
        if (!isOpen) return;

        RefreshGrid();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        RefreshVisibility();
        
        if (isOpen)
        {
            RefreshGrid();
        }
    }

    public void SetInventoryOpen(bool open)
    {
        isOpen = open;
        RefreshVisibility();

        if (isOpen)
        {
            RefreshGrid();
        }
    }

    private void RefreshVisibility()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isOpen);
        }
    }

    private void RebuildGrid()
    {
        if (inventory == null || slotContainer == null || slotPrefab == null) return;

        ClearSpawnedSlots();

        for (int i = 0; i < inventory.MaxSlots; i++)
        {
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
            spawnedSlots.Add(slotUI);
        }

        RefreshGrid();
    }

    private void RefreshGrid()
    {
        if (inventory == null) return;

        IReadOnlyList<InventorySlot> items = inventory.Items;

        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            if (i < items.Count)
            {
                spawnedSlots[i].SetSlot(items[i]);
            }
            else
            {
                spawnedSlots[i].ClearSlot();
            }
        }
    }

    private void ClearSpawnedSlots()
    {
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            if (spawnedSlots[i] != null)
            {
                Destroy(spawnedSlots[i].gameObject);
            }
        }

        spawnedSlots.Clear();
    }
}
