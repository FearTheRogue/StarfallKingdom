using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;

    public int currentHealth {  get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (TryGetComponent(out TrainingDummy trainingDummy))
        {
            trainingDummy.OnDamaged();
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }


    public void RestoreHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
