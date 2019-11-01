using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Dammypoint;
    public GameObject spawnPoint;
    public int numberOfEnemyi;
    public int health;
    [HideInInspector]
    public List<SpawnPoint> enemySpawnPoints;

    public void GenerateSpownPoints ()
    {
        for (int i = 0; i < numberOfEnemyi; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(2f, 5f), Random.Range(5f, 15f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint EnemySpawnPoint = (Instantiate(Dammypoint, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            enemySpawnPoints.Add(EnemySpawnPoint);
        }
    }

    /*
    public void SpawnEnemies(NetworkManager.EnemyJSON enemiesJSON)
    {
        foreach (NetworkManager.EnemyJSON enemyJSON in enemiesJSON.enemies)
        {
            if (enemyJSON.health <= 0)
            {
                continue;
            }
            Vector3 position = new Vector3(enemyJSON.enemyPosition[0], enemyJSON.enemyPosition[1], enemyJSON.enemyPosition[2]);
            Quaternion rotation = Quaternion.Euler(enemyJSON.enemyRotation[0], enemyJSON.enemyRotation[1], enemyJSON.enemyRotation[2]);
            GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
            newEnemy.name = enemyJSON.name;
            MultiPlayerController pc = newEnemy.GetComponent<MultiPlayerController>();
            pc.isLocalPlayer = false;
            Health h = newEnemy.GetComponent<Health>();
            h.currentHealth = health;
            h.OnChangeHealth();
            h.destroyOnDeath = true;
            h.isEnemy = true;
        }
    }
    */
}
