using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiSpawner : MonoBehaviour
{
    public GameObject wakizashi;
    public GameObject spawnPoint;
    public int numberOfwakizashi;
    [HideInInspector]
    public List<SpawnPoint> wakizashiSpawnPoints;
    
    void Start()
    {
        for (int i = 0; i < numberOfwakizashi; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-3f, 3f), 3f, Random.Range(-3f, 3f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint wakizashiSpawnPoint = (Instantiate(wakizashi, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            wakizashiSpawnPoints.Add(wakizashiSpawnPoint);
            
        }
    }

    public void SpawnEnemies()
    {
        int i = 0;
        foreach (SpawnPoint sp in wakizashiSpawnPoints)
        {
            Vector3 position = sp.transform.position;
            Quaternion rotation = sp.transform.rotation;
            GameObject newWakizashi = Instantiate(wakizashi, position, rotation) as GameObject;
            newWakizashi.name = i+"";
            MultiPlayerController pc = newWakizashi.GetComponent<MultiPlayerController>();
            pc.isLocalPlayer = false;
            // Damage????
            i++;
        }
    }
    
}
