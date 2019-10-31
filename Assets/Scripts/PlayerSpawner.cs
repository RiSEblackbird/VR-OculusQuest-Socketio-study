using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public int numberOfPoint = 15;
    [HideInInspector]
    public List<SpawnPoint> SpawnPoints;
    public SpawnPoint playerSpawnPoint;

    /* スポーン位置ランダム：今後余裕があれば。
    public void GenerateSpownPoint()
    {
        playerSpawnPoint = SpawnPoints[Random.Range(1, SpawnPoints.Count)];
        for (int i = 0; i < numberOfPoint; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint PlayerSpawnPoint = (Instantiate(player, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            SpawnPoints.Add(PlayerSpawnPoint);
        }
    }
    */
}
