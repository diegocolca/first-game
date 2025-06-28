using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    private Move moveScript;

    private void Start()
    {
        currentHealth = maxHealth;
        moveScript = GetComponent<Move>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"[PLAYER] Daño recibido: {damage}. Vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        Debug.Log("[PLAYER] murió.");
        onDeath?.Invoke();

        if (moveScript != null)
        {
            moveScript.OnDeath();
        }

        GameManager.Instance?.SetPlayerDead();
    }
}
