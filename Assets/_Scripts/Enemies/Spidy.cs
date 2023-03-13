using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spidy : MonoBehaviour
{
    Rigidbody rb;
    private int dir = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //StartCoroutine(Switch());
    }

    private void Update()
    {
        rb.velocity = transform.parent.forward * 0.25f * dir;
    }

    IEnumerator Switch()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            for(int i = 0; i < 20; i++)
            {
                transform.Rotate(0f, 0f, 9f);
                yield return new WaitForFixedUpdate();
            }
            dir *= -1;
        }
    }

}
