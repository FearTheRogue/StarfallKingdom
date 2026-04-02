using Mono.Cecil;
using UnityEngine;

public class OreNode : MonoBehaviour
{
    [Header("Ore")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int hitsRemaining = 3;

    [Header("Drop")]
    [SerializeField] private GameObject droppedResourcePrefab;
    [SerializeField] private Transform dropSpawnPoint;
    [SerializeField] private Vector3 randomDropOffset = new Vector3(0.5f, 0f, 0.5f);

    public ResourceType ResourceType => resourceType;
    public bool IsDepleted => hitsRemaining <= 0f;

    public void MineHit()
    {
        if (IsDepleted) return;

        SpawnDrop();
        hitsRemaining--;

        if (IsDepleted)
        {
            Deplete();
        }
    }

    private void SpawnDrop()
    {
        if (droppedResourcePrefab == null) return;

        Vector3 spawnOrigin = dropSpawnPoint != null ? dropSpawnPoint.position : transform.position;

        Vector3 randomOffset = new Vector3(Random.Range(-randomDropOffset.x, randomDropOffset.x), randomDropOffset.y, Random.Range(-randomDropOffset.z, randomDropOffset.z));

        Instantiate(droppedResourcePrefab, spawnOrigin + randomOffset, Quaternion.identity);
    }

    private void Deplete()
    {
        Destroy(gameObject);
    }
}
