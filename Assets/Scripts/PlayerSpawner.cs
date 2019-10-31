using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public SpawnPoint playerSpawnPoint;
    public int numberOfPoint;
    [HideInInspector]
    public List<SpawnPoint> SpawnPoints;

    public void GenerateSpownPoint()
    {
        playerSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
        for (int i = 0; i < numberOfPoint; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-30f, 30f), Random.Range(10f, 30f), Random.Range(-30f, 30f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint PlayerSpawnPoint = (Instantiate(player, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            SpawnPoints.Add(PlayerSpawnPoint);
        }
    }
}
