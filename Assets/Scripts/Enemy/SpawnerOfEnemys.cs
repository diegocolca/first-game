using System.Linq;
using UnityEngine;

public class SpawnerOfEnemys : MonoBehaviour
{
    [Header("Prefab del enemigo")]
    public GameObject enemyPrefab;

    [Header("Configuración del spawn")]
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public bool loopSpawn = true;

    [Header("Cantidad inicial")]
    public int initialEnemyCount = 1;

    [Header("Límite de enemigos")]
    public int maxEnemies = 3;

    private float timer;
    private int currentEnemies = 0;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = GetComponentsInChildren<Transform>()
                .Where(t => t != this.transform).ToArray();
        }

        for (int i = 0; i < initialEnemyCount && i < maxEnemies; i++)
        {
            SpawnEnemy();
        }

        timer = 0f; // Primera espera es inmediata
    }

    void Update()
    {
        if (!loopSpawn || enemyPrefab == null || spawnPoints.Length == 0) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        currentEnemies++;

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                controller.player = player.transform;
            }
        }

        EnemyDespawn despawnScript = enemy.GetComponent<EnemyDespawn>();
        if (despawnScript == null)
        {
            despawnScript = enemy.AddComponent<EnemyDespawn>();
        }
        despawnScript.spawner = this;
    }

    public void NotifyEnemyKilled()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }
}

