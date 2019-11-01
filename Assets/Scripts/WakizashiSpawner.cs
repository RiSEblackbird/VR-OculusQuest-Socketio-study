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
            var spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(1f, 10f), Random.Range(5f, 20f));
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
            Vector3 position = new Vector3(wakizashiJSON.wakizashiPosition[0], wakizashiJSON.wakizashiPosition[1], wakizashiJSON.wakizashiPosition[2]);
            Quaternion rotation = Quaternion.Euler(wakizashiJSON.wakizashiRotation[0], wakizashiJSON.wakizashiRotation[1], wakizashiJSON.wakizashiRotation[2]);
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
