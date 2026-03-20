using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWander : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;
    [SerializeField] private float destinationThreshold = 0.2f;
    [SerializeField] private float navMeshSampleDistance = 2f;

    private NavMeshAgent agent;
    private float idleTimer;
    private bool isWaiting;
    private Vector3 startPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }

    private void Start()
    {
        SetNewDestination();
    }

    private void Update()
    {
        Debug.Log($"Waiting: {isWaiting}, Timer: {idleTimer}");

        if (isWaiting)
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                SetNewDestination();
            }

            return;
        }

        if (HasReachedDestination())
        {
            StartWaiting();
        }
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance > destinationThreshold) return false;

        return agent.velocity.sqrMagnitude <= 0.01f;
    }

    private void StartWaiting()
    {
        isWaiting = true;
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
        agent.ResetPath();
    }

    private void SetNewDestination()
    {
        isWaiting = false;

        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;
        randomOffset.y = 0f;

        Vector3 targetPosition = transform.position + randomOffset;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            StartWaiting();
        }
    }
}
