using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFacing : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float movementThreshold = 0.01f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.velocity.sqrMagnitude <= movementThreshold) return;

        Vector3 direction = agent.velocity.normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
