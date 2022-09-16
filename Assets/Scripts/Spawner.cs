using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Paramters")]
    public int maxCountEnemies;
    int curCountEnemies;

    //Components
    BoxCollider bc;

    [Header("Enemy Prefab")]
    public GameObject enemyPref;

    List<GameObject> enemies;


    [Header("Player")]
    public Player player;


    void Start()
    {
        bc = GetComponent<BoxCollider>();

        enemies = new List<GameObject>();

        for (int i = 0; i < maxCountEnemies; i++)
        {
            SpawnEnemy();
        }
    }



    public void Restart()
    {
        foreach (var enemy in enemies)
            Destroy(enemy);

        enemies.Clear();

        curCountEnemies = 0;

        for (int i = 0; i < maxCountEnemies; i++)
        {
            SpawnEnemy();
        }
    }


    void SpawnEnemy()
    {

        float randX = Random.Range(-bc.size.x / 2 + transform.position.x, bc.size.x / 2 + transform.position.x);
        float randZ = Random.Range(-bc.size.z / 2 + transform.position.z, bc.size.z / 2 + transform.position.z);


        GameObject enemy = Instantiate(enemyPref, new Vector3(randX, 0, randZ), Quaternion.identity);
        enemy.GetComponent<Enemy>().Initialize(this, player);

        enemies.Add(enemy);

        curCountEnemies++;
    }


    public void PlayerOnBase()
    {
        int countOfEnemies = maxCountEnemies - curCountEnemies;


        for (int i = 0; i < countOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    public void EnemyDead(GameObject enemy)
    {
        curCountEnemies--;

        enemies.Remove(enemy);
    }


    public GameObject ClosestEnemy()
    {
        if (enemies.Count > 0)
        {
            int indexEnemy = 0;
            float shortestDistance = Vector3.Distance(player.transform.position, enemies[indexEnemy].transform.position);

            for (int i = 1; i < enemies.Count; i++)
            {
                float dist = Vector3.Distance(player.transform.position, enemies[i].transform.position);

                if (dist < shortestDistance)
                {
                    indexEnemy = i;
                    shortestDistance = dist;
                }
            }

            return enemies[indexEnemy];
        }
        else
            return null;
    }
}