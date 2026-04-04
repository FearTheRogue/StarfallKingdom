using UnityEngine;
using UnityEngine.UIElements;

public class PickaxePickup : MonoBehaviour
{
    public void Collect(PlayerCombat playerCombat)
    {
        if (playerCombat == null)
            return;

        playerCombat.SetHasPickaxe(true);
        Destroy(gameObject);
    }
}
