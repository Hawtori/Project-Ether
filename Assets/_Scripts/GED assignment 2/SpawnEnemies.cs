using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public int enemiesToSpawn;

    [SerializeField]
    private Vector3[] locationToSpawnEnemies;

    private Vector3 bound1, bound2;

    private void Start()
    {
        //locationToSpawnEnemies = new Vector3[enemiesToSpawn];

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject e = EnemyPool._instance.GetEnemy();
            e.transform.position = locationToSpawnEnemies[i];
        }

    }

    private void Update()
    {
        Destroy(gameObject);
    }


}
