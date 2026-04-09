using UnityEngine;

public class PickaxePickup : MonoBehaviour
{
    public void Collect(PlayerInteraction playerInteraction)
    {
        if (playerInteraction == null)
            return;

        playerInteraction.SetHasPickaxe(true);
        Destroy(gameObject);
    }
}
