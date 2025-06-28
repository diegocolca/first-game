using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    private Animator animator;
    private EnemyDespawn despawnScript;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        despawnScript = GetComponent<EnemyDespawn>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} murió.");
        animator?.SetTrigger("Dead_Velociraptor");

        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.OnDeath();
        }

        despawnScript?.spawner?.NotifyEnemyKilled();

        Destroy(gameObject, 5f);
    }
}
