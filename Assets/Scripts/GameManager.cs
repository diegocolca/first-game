using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsPlayerDead { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // evita duplicados
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas(si es necesario :v)
        }
    }

    public void SetPlayerDead()
    {
        IsPlayerDead = true;
    }
}
