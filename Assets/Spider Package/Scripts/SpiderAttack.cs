using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    Vector3 pos;
    public float damage;
    public float radius;

    public ParticleSystem vfx_placeholder;

    bool freezePos;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Bullet"))
            Burst();
    }

    private void Update()
    {
        if (freezePos)
        {
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<SpidyAnims>().PlayerDetected(other.transform.position);
            StartCoroutine(AttachToPlayer());
            //GetComponent<SpiderMove>().enable = false;
            //Debug.Log("Disasbled");

            //float x = other.transform.position.x - transform.position.x;
            //float z = other.transform.position.z - transform.position.z;

            //float angle = Mathf.Atan2(x, z);
            //angle *= Mathf.Rad2Deg;

            //Vector3 forward = transform.forward;
            //Vector3 res = other.transform.position - transform.position;

            //angle = Vector3.Angle(forward, res);

            //Debug.Log("Rotating: " + angle);

            //StartCoroutine(GetComponent<ProAnims>().Rotate(angle));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) pos = other.transform.position;
    }


    private void Burst()
    {
        if (vfx_placeholder != null) Instantiate(vfx_placeholder, transform.position, Quaternion.identity);

        Vector3 location = transform.position;
        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);
        foreach (Collider col in objectsInRange)
        {
            Health hp = col.GetComponent<Health>();
            if (hp != null)
            {
                // linear falloff of effect
                float proximity = (location - hp.transform.position).magnitude;
                float effect = 1 - (proximity / radius);

                hp.TakeDamage(damage * effect);
            }
        }
        Invoke("Die", .25f);
    }

    private void Die()
    {
        Destroy(this.gameObject);  
    }

    IEnumerator AttachToPlayer()
    {
        Vector3 startPos = transform.position;
        pos += (transform.position - pos).normalized * 2f;
        for (int i = 0; i < 50f; i++)
        {
            transform.position = Vector3.Lerp(startPos, pos, i / 51f);
            transform.position += transform.up * Mathf.Sin(i / 51f * Mathf.PI) * 0.5f;
            yield return new WaitForFixedUpdate();
        }
        freezePos = true;
    }
}
