using UnityEngine;

public class OreNode : MonoBehaviour
{
    [Header("Ore")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int resourceAmount = 3;
    [SerializeField] private int hitPoints = 3;

    [Header("Drops")]
    [SerializeField] private GameObject dropppedResourcePrefab;

    public ResourceType ResourceType => resourceType;
    public int ResourceAmount => resourceAmount;

    public void Mine(int damage)
    {
        hitPoints = damage;

        if (hitPoints <= 0)
        {
            Deplete();
        }
    }

    private void Deplete()
    {
        if (dropppedResourcePrefab != null)
        {
            for (int i = 0; i < resourceAmount; i++)
            {
                Instantiate(dropppedResourcePrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
