using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    // Posici�n original de la c�mara (se inicializa en Start)
    private Vector3 originalPosition;

    void Awake()
    {
        // Implementaci�n del Singleton para f�cil acceso
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Guarda la posici�n original de la c�mara
        originalPosition = transform.localPosition;
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
        Debug.Log($"[CAMERA SHAKE] Iniciando sacudida de c�mara. Duraci�n: {duration}s, Magnitud: {magnitude}");
    }
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Genera una posici�n aleatoria dentro del rango de magnitud
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica la nueva posici�n a la c�mara (relativa a su posici�n original)
            transform.localPosition = originalPosition + new Vector3(x, y, 0); // Asumiendo 2D, Z es 0

            elapsed += Time.deltaTime; // Incrementa el tiempo transcurrido
            yield return null; // Espera al siguiente frame
        }

        // Aseg�rate de que la c�mara vuelva a su posici�n original al finalizar
        transform.localPosition = originalPosition;
    }

    // M�todo para actualizar la posici�n original si la c�mara se mueve (ej. si sigue al jugador)
    public void UpdateOriginalPosition()
    {
        originalPosition = transform.localPosition;
    }
}
