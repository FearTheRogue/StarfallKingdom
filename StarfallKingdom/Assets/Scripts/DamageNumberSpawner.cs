using Unity.VisualScripting;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    [Header("Damage Number")]
    [SerializeField] private FloatingDamageNumber damageNumberPrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private Vector3 randomOffsetRange = new Vector3(0.4f, 0.2f, 0.4f);

    public void SpawnDamageNumber(int amount)
    {
        if (damageNumberPrefab == null)
            return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-randomOffsetRange.x, randomOffsetRange.x), 
            Random.Range(-randomOffsetRange.y, randomOffsetRange.y), 
            Random.Range(-randomOffsetRange.z, randomOffsetRange.z));

        FloatingDamageNumber damageNumber = Instantiate(damageNumberPrefab, transform.position + spawnOffset + randomOffset, Quaternion.identity);

        damageNumber.SetDamage(amount);
    }
}
