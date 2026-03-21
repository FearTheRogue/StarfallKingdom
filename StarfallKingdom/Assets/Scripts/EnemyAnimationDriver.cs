using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterAnimationController))]
public class EnemyAnimationDriver : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterAnimationController animationController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animationController = GetComponent<CharacterAnimationController>();
    }

    private void Update()
    {
        animationController.SetMoveSpeed(agent.velocity.magnitude);
    }
}
