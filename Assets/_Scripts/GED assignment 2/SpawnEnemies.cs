using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public int enemiesToSpawn;

    [SerializeField]
    private Vector3 bound1, bound2;
    private Vector3[] locationToSpawnEnemies;

    private void Start()
    {
        locationToSpawnEnemies = new Vector3[enemiesToSpawn];

        Vector3 pos;
        for (int i = 0; i < enemiesToSpawn; i++) {
            pos = new Vector3(
                Random.Range(bound1.x, bound2.x),
                Random.Range(bound1.y, bound2.y),
                Random.Range(bound1.z, bound2.z)
                );
            locationToSpawnEnemies[i] = pos; 
        }

    }


    private void OnTriggerEnter(Collider other) //spawner
    {
        if (other.CompareTag("Player"))
        {
            for(int i = 0; i < enemiesToSpawn; i++)
            {
                GameObject e = EnemyPool._instance.GetEnemy();
                e.transform.position = locationToSpawnEnemies[i];
            }
        }
    }


}
