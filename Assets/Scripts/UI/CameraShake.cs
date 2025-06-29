using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    // Posición original de la cámara (se inicializa en Start)
    private Vector3 originalPosition;

    void Awake()
    {
        // Implementación del Singleton para fácil acceso
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
        // Guarda la posición original de la cámara
        originalPosition = transform.localPosition;
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
        Debug.Log($"[CAMERA SHAKE] Iniciando sacudida de cámara. Duración: {duration}s, Magnitud: {magnitude}");
    }
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Genera una posición aleatoria dentro del rango de magnitud
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica la nueva posición a la cámara (relativa a su posición original)
            transform.localPosition = originalPosition + new Vector3(x, y, 0); // Asumiendo 2D, Z es 0

            elapsed += Time.deltaTime; // Incrementa el tiempo transcurrido
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que la cámara vuelva a su posición original al finalizar
        transform.localPosition = originalPosition;
    }

    // Método para actualizar la posición original si la cámara se mueve (ej. si sigue al jugador)
    public void UpdateOriginalPosition()
    {
        originalPosition = transform.localPosition;
    }
}
