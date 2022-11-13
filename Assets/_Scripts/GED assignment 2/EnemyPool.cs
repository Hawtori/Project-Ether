using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool _instance { get; set; }

    [SerializeField]
    private GameObject enemy;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private int poolSize = 24;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject e = Instantiate(enemy);
            enemyPool.Enqueue(e);
            e.SetActive(false);
            e.transform.parent = transform;
        }
    }

    public GameObject GetEnemy()
    {
        if(enemyPool.Count <= 0)
        {
            GameObject _e = Instantiate(enemy);
            return _e;
        }
        GameObject e = enemyPool.Dequeue();
        e.SetActive(true);
        e.transform.parent = null;
        return e;
    }

    public void ReturnEnemy(GameObject e)
    {
        enemyPool.Enqueue(e);
        e.transform.parent = transform;
        e.SetActive(false);
    }
}
