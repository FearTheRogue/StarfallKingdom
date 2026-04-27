using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyWander))]
[RequireComponent(typeof(CharacterAnimationController))]
public class EnemyCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private float attackRange = 1.75f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackHitDelay = 0.4f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float lookRotationSpeed = 8f;

    private NavMeshAgent agent;
    private EnemyWander wander;
    private CharacterAnimationController animationController;

    private Transform player;
    private Actor playerActor;
    private bool isAggro;
    private bool isAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        wander = GetComponent<EnemyWander>();
        animationController = GetComponent<CharacterAnimationController>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerActor = playerObject.GetComponent<Actor>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAggro && distanceToPlayer <= chaseRange) SetAggro(true);

        if (!isAggro) return;

        if (distanceToPlayer > chaseRange * 1.5f)
        {
            SetAggro(false);
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            FaceTarget(player.position);

            if (!isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }

            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public void SetAggro(bool value)
    {
        isAggro = value;
        isAttacking = false;
        agent.isStopped = false;
        wander.SetWandering(!value);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;
        animationController.TriggerWeaponAttack();

        yield return new WaitForSeconds(attackHitDelay);

        if (playerActor != null) playerActor.TakeDamage(attackDamage);

        yield return new WaitForSeconds(Mathf.Max(0f, attackCooldown - attackHitDelay));
        isAttacking = false;
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotationSpeed * Time.deltaTime);
    }
}
