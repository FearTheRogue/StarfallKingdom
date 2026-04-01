using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount = 1;

    public ResourceType ResourceType => resourceType;
    public int Amount => amount;
}
