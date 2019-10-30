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

    public void GenerateSpownPoints()
    {
        for (int i = 0; i < numberOfwakizashi; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-3f, 3f), 3f, Random.Range(-3f, 3f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint wakizashiSpawnPoint = (Instantiate(wakizashi, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            wakizashiSpawnPoints.Add(wakizashiSpawnPoint);
            
        }
    }

    public void SpawnWakizashis(NetworkManager.WakizashiJSON wakizashisJSON)
    {
        foreach (NetworkManager.WakizashiJSON wakizashiJSON in wakizashisJSON.wakizashis)
        {
            if (wakizashiJSON.health <= 0)
            {
                continue;
            }
            Vector3 position = new Vector3(wakizashiJSON.position[0], wakizashiJSON.position[1], wakizashiJSON.position[2]);
            Quaternion rotation = Quaternion.Euler(wakizashiJSON.rotation[0], wakizashiJSON.rotation[1], wakizashiJSON.rotation[2]);
            GameObject newWakizashi = Instantiate(wakizashi, position, rotation) as GameObject;
            newWakizashi.name = wakizashiJSON.name;
            MultiPlayerController pc = newWakizashi.GetComponent<MultiPlayerController>();
            pc.isLocalPlayer = false;
            Health h = newWakizashi.GetComponent<Health>();
            h.currentHealth = 100;
            h.OnChangeHealth();
            h.destroyOnDeath = true;
            h.isWeapon = true;
        }
    }

}
