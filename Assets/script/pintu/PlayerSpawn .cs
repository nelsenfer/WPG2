using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        SpawnPoint[] semuaSpawn = FindObjectsOfType<SpawnPoint>();

        foreach (SpawnPoint spawn in semuaSpawn)
        {
            if (spawn.spawnID == GameManager.Instance.lastSpawnPoint)
            {
                transform.position = spawn.transform.position;
                break;
            }
        }
    }
}