using Unity.VisualScripting;
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

    public void InteractWithItem(PlayerCombat playerCombat)
    {
        if (TryGetComponent(out PickaxePickup pickaxePickup))
        {
            pickaxePickup.Collect(playerCombat);
            return;
        }

        Destroy(gameObject);
    }
}
