using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : MonoBehaviour
{
    public float speed;
    public float amplitudeOfDrift;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) StartCoroutine(GetComponent<SpiderAnims>().Rotate(90f));

        //Vector3 randomSideways = new Vector3(Random.Range(-1f, 1f), 0, 0);

        //transform.position += transform.forward * 0.25f * Time.deltaTime + (Vector3.right * Random.Range(-0.001f, 0.001f));
        //controller.SimpleMove(transform.forward * speed);
        rb.velocity = transform.forward * speed;
        rb.velocity += (transform.right * (Mathf.PerlinNoise(Time.time, 0f) - 0.5f) * amplitudeOfDrift);
        //controller.Move(transform.forward * speed);

        //GetComponent<Rigidbody>().velocity = Vector3.forward * 0.025f;
    }
}
