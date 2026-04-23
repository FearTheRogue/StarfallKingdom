using UnityEngine;

public class PlayerToolVisuals : MonoBehaviour
{
    [Header("Tool Visuals")]
    [SerializeField] private GameObject pickaxeVisuals;

    public void ShowPickaxe()
    {
        if (pickaxeVisuals != null)
        {
            pickaxeVisuals.SetActive(true);
        }
    }

    public void HidePickaxe()
    {
        if (pickaxeVisuals != null)
        {
            pickaxeVisuals.SetActive(false);    
        }
    }

    public void SetPickaxeVisible(bool visible)
    {
        if (pickaxeVisuals != null)
        {
            pickaxeVisuals.SetActive(visible);
        }
    }
}
