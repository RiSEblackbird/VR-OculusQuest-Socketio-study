using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject spawnPoint;
    public int numberOfEnemyi;
    [HideInInspector]
    public List<SpawnPoint> EnemySpawnPoints;

    void Start()
    {
        for (int i = 0; i < numberOfEnemyi; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-30f, 30f), Random.Range(10f, 30f), Random.Range(-30f, 30f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint EnemySpawnPoint = (Instantiate(Enemy, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            EnemySpawnPoints.Add(EnemySpawnPoint);

        }
    }

    public void SpawnEnemies()
    {
        int i = 0;
        foreach (SpawnPoint sp in EnemySpawnPoints)
        {
            Vector3 position = sp.transform.position;
            Quaternion rotation = sp.transform.rotation;
            GameObject newEnemy = Instantiate(Enemy, position, rotation) as GameObject;
            newEnemy.name = i + "";
            MultiPlayerController pc = newEnemy.GetComponent<MultiPlayerController>();
            pc.isLocalPlayer = false;
            // Damage????
            i++;
        }
    }
}
