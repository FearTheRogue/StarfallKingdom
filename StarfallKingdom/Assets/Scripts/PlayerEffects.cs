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

    public Vector3 GetGroundEffectPosition(Ray ray, RaycastHit originalHit, float maxClickDistance)
    {
        if (Physics.Raycast(ray, out RaycastHit groundHit, maxClickDistance, groundLayers))
        {
            return groundHit.point;
        }

        if (NavMesh.SamplePosition(originalHit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
        {
            return navHit.position;
        }

        return originalHit.point;
    }

    public void SpawnClickEffect(Vector3 position)
    {
        SpawnEffect(clickEffect, position + Vector3.up * clickEffectHeightOffset);
    }

    public void SpawnTargetEffect(Transform targetTransform)
    {
        if (targetTransform == null) return;

        SpawnEffect(targetEffect, targetTransform.position + Vector3.up * targetEffectHeightOffset);
    }

    public void SpawnHitEffect(Vector3 position)
    {
        SpawnEffect(hitEffect, position + Vector3.up * hitEffectHeightOffset);
    }

    private void SpawnEffect(ParticleSystem effect, Vector3 position)
    {
        if (effect == null) return;

        Instantiate(effect, position, effect.transform.rotation);
    }
}
