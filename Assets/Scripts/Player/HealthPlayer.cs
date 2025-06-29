using UnityEngine;
using System;
using Unity.Cinemachine;

public class HealthPlayer : MonoBehaviour
{
    public static HealthPlayer Instance { get; private set; }
    public int maxHealth = 50;
    private int currentHealth;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    private Move moveScript;

    public CinemachineImpulseSource impulseSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        moveScript = GetComponent<Move>();
        if (impulseSource == null)
        {
            impulseSource = FindFirstObjectByType<CinemachineImpulseSource>();
            if (impulseSource == null)
            {
                Debug.LogWarning("CinemachineImpulseSource not found in scene. Camera shake for damage will not work.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"[PLAYER] Daño recibido: {damage}. Vida actual: {currentHealth}");

        //if (CameraShake.Instance != null)
        //{
        //    // Ajusta estos valores según la intensidad y duración que desees
        //    CameraShake.Instance.Shake(0.15f, 0.1f); // Duración: 0.2 segundos, Magnitud: 0.1 unidades
        //    Debug.Log("[PLAYER] Cámara sacudida por daño recibido.");
        //}

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
            // También puedes pasar una fuerza específica:
            // impulseSource.GenerateImpulse(transform.forward * 5f); // Ejemplo de fuerza en una dirección
        }
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
        //Debug.Log("[PLAYER] murió.");
        onDeath?.Invoke();

        if (moveScript != null)
        {
            moveScript.OnDeath();
        }

        GameManager.Instance?.SetPlayerDead();
        Destroy(gameObject);
    }
}
