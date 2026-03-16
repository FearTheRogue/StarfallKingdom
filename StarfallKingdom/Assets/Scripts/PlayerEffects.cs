using UnityEngine;
using UnityEngine.AI;

public class PlayerEffects : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private ParticleSystem targetEffect;
    [SerializeField] private ParticleSystem hitEffect;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayers;

    [Header("Offsets")]
    [SerializeField] private float clickEffectHeightOffset = 0.1f;
    [SerializeField] private float targetEffectHeightOffset = 1f;
    [SerializeField] private float hitEffectHeightOffset = 1f;

    [Header("Ground Sampling")]
    [SerializeField] private float navMeshSampleRadius = 2f;

    public void SpawnGroundClickEffect(Ray ray, RaycastHit originalHit, float maxClickDistance)
    {
        Vector3 groundEffectPosition = GetGroundEffectPosition(ray, originalHit, maxClickDistance);
        SpawnClickEffect(groundEffectPosition);
    }

    public void SpawnTargetEffect(Transform targetTransform)
    {
        if (targetTransform == null) return;

        Vector3 spawnPosition = targetTransform.position + Vector3.up * targetEffectHeightOffset;
        SpawnEffect(targetEffect, spawnPosition);
    }

    public void SpawnHitEffect(Vector3 worldPosition)
    {
        Vector3 spawnPosition = worldPosition + Vector3.up * hitEffectHeightOffset;
        SpawnEffect(hitEffect, spawnPosition);
    }

    private Vector3 GetGroundEffectPosition(Ray ray, RaycastHit originalHit, float maxClickDistance)
    {
        if (Physics.Raycast(ray, out RaycastHit groundHit, maxClickDistance, groundLayers))
        {
            return groundHit.point;
        }

        if (NavMesh.SamplePosition(originalHit.point, out NavMeshHit navHit, navMeshSampleRadius, NavMesh.AllAreas))
        {
            return navHit.position;
        }

        return originalHit.point;
    }

    private void SpawnClickEffect(Vector3 worldPosition)
    {
        Vector3 spawnPosition = worldPosition + Vector3.up * clickEffectHeightOffset;
        SpawnEffect(clickEffect, spawnPosition);
    }

    private void SpawnEffect(ParticleSystem effect, Vector3 worldPosition)
    {
        if (effect == null) return;

        Instantiate(effect, worldPosition, effect.transform.rotation);
    }
}
