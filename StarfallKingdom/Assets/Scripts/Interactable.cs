using UnityEngine;

public enum InteractionTypes { Enemy, Item, Resource }
public class Interactable : MonoBehaviour
{
    public Actor MyActor { get; private set; }

    [SerializeField] private InteractionTypes interactionType;
    public InteractionTypes InteractionType => interactionType;

    private void Awake()
    {
        if (interactionType == InteractionTypes.Enemy)
            MyActor = GetComponent<Actor>(); ;
    }

    public void InteractWithItem(PlayerInteraction playerInteraction)
    {
        if (playerInteraction == null) return;

        if (TryGetComponent(out PickaxePickup pickaxePickup))
        {
            pickaxePickup.Collect(playerInteraction);
            return;
        }

        if (TryGetComponent(out WeaponPickup weaponPickup))
        {
            weaponPickup.Collect(playerInteraction);
            return;
        }

        if (TryGetComponent(out InventoryPickup inventoryPickup))
        {
            inventoryPickup.Collect(playerInteraction.Inventory);
            return;
        }

        Destroy(gameObject);
    }
}
