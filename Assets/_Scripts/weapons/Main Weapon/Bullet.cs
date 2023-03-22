using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        gameObject.tag = "Bullet";
        Invoke("DestroySelf", 2f);           
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, 1.5f, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Head"))
            {
                hit.transform.GetComponentInParent<Health>().TakeDamage(damage * 2.5f);
                return;
            }
            if(hit.transform.GetComponent<Health>() != null)
            {
                hit.transform.GetComponent<Health>().TakeDamage(damage);
            } 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if collision gameobject is not in target layer
        if(collision.gameObject.layer != 7) { DestroySelf(); return; } 
        collision.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        ShootingRange._instance.IncreaseHit();
        DestroySelf();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    //    ShootingRange._instance.IncreaseHit();
    //    DestroySelf();
    //}

    private void DestroySelf()
    {
        Destroy(gameObject)
;   }
}
