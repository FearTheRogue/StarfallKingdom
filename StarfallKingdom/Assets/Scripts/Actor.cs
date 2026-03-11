using UnityEngine;
using UnityEngine.Rendering;

public class Actor : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int currentHealth {  get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
