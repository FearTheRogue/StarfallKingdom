using Mono.Cecil;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class OreNode : MonoBehaviour
{
    [Header("Ore")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int hitsRemaining = 3;

    [Header("Drop")]
    [SerializeField] private GameObject droppedResourcePrefab;
    [SerializeField] private Transform dropSpawnPoint;
    [SerializeField] private Vector3 randomDropOffset = new Vector3(0.5f, 0f, 0.5f);

    [Header("Launch")]
    [SerializeField] private float minLaunchForce = 2.5f;
    [SerializeField] private float maxLaunchForce = 4f;
    [SerializeField] private float upwardsLaunchBias = 1.2f;

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

        GameObject droppedObject = Instantiate(droppedResourcePrefab, spawnOrigin + randomOffset, Quaternion.identity);

        if (droppedObject.TryGetComponent(out Rigidbody rd))
        {
            Vector3 launchDirection = new Vector3(Random.Range(-1f, 1f), upwardsLaunchBias, Random.Range(-1f, 1f)).normalized;

            float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
            rd.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }

    private void Deplete()
    {
        Destroy(gameObject);
    }
}
