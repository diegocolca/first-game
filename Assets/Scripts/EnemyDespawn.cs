using UnityEngine;

public class EnemyDespawn : MonoBehaviour
{
    [HideInInspector]
    public SpawnerOfEnemys spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.NotifyEnemyKilled();
        }
    }
}
