using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroySelf", 2f);           
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != 7) { DestroySelf(); return; }
        collision.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        ShootingRange._instance.IncreaseHit();
        DestroySelf();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        ShootingRange._instance.IncreaseHit();
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject)
;   }
}
