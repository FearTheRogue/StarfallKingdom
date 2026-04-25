using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerToolVisuals : MonoBehaviour
{
    [Header("Tool Visuals")]
    [SerializeField] private GameObject pickaxeVisual;
    [SerializeField] private GameObject weaponVisual;

    public void ShowPickaxe()
    {
        if (pickaxeVisual != null)
        {
            pickaxeVisual.SetActive(true);
        }
    }

    public void HidePickaxe()
    {
        if (pickaxeVisual != null)
        {
            pickaxeVisual.SetActive(false);    
        }
    }

    public void SetPickaxeVisible(bool visible)
    {
        if (pickaxeVisual != null)
        {
            pickaxeVisual.SetActive(visible);
        }
    }

    public void ShowWeapon()
    {
        if (weaponVisual != null)
        {
            weaponVisual.SetActive(true);
        }
    }

    public void HideWeapon()
    {
        if (weaponVisual != null)
        {
            weaponVisual.SetActive(false);
        }
    }

    public void HideAll()
    {
        HidePickaxe();
        HideWeapon();
    }
}
