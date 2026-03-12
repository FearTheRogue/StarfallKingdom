using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");
    private static readonly int PickupHash = Animator.StringToHash("Pickup");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoveSpeed(float speed)
    {
        animator.SetFloat(SpeedHash, speed);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger(AttackHash);
    }

    public void TriggerHit()
    {
        animator.SetTrigger(HitHash);
    }

    public void SetDead(bool isDead)
    {
        animator.SetBool(DeadHash, isDead);
    }

    public void TriggerPickup()
    {
        animator.SetTrigger(PickupHash);
    }
}
