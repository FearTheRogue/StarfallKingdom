using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class TrainingDummy : MonoBehaviour
{
    [Header("Training Dummy")]
    [SerializeField] private bool invulnerable = true;
    [SerializeField] private bool resetHealthAfterHit = false;
    [SerializeField] private float resetDelay = 0.5f;

    private Actor actor;
    private int maxHealth;
    private Coroutine resetRoutine;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        maxHealth = actor.currentHealth;
    }

    public void OnDamage()
    {
        if (invulnerable)
        {
            actor.currentHealth = maxHealth;
            return;
        }

        if (resetHealthAfterHit)
        {
            if (resetRoutine != null)
            {
                StopCoroutine(resetRoutine);
            }

            resetRoutine = StartCoroutine(ResetHealthRoutine());
        }
    }

    private IEnumerator ResetHealthRoutine()
    {
        yield return new WaitForSeconds(resetDelay);

        if (actor != null)
        {
            actor.currentHealth = maxHealth;
        }

        resetRoutine = null;
    }
}
