using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMove : MonoBehaviour
{
    public float speed;
    public bool enable = true;

    private bool changingDir = false;

    private Rigidbody rb;

    private int dir = 1;

    private float lerp(float a, float b, float t) => a + (b - a) * t;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (NetInfo.Instance != null) Random.InitState(NetInfo.Instance.GetSeed());
    }

    private void Update()
    {
        //Vector3 randomSideways = new Vector3(Random.Range(-1f, 1f), 0, 0);

        //transform.position += transform.forward * 0.25f * Time.deltaTime + (Vector3.right * Random.Range(-0.001f, 0.001f));
        //controller.SimpleMove(transform.forward * speed);

        // transform.rotation.x has to be between -40 and 40

        rb.velocity = transform.forward * speed * dir;

        if (!enable) return;

        if (NetInfo.Instance != null) Random.InitState(NetInfo.Instance.GetSeed());

        float x = Random.value;

        float noise = (Mathf.PerlinNoise(x, 0f) - 0.5f);
        rb.velocity += (new Vector3(1f, 0, 0) * noise * 2 * speed);
        transform.Rotate(0f, noise / 8f, 0f);
        //controller.Move(transform.forward * speed);

        //float bob = lerp(-0.025f, -0.015f, noise * 2 + 0.5f);    
        //
        //// up down bob
        //transform.localPosition = new Vector3(transform.localPosition.x, bob, transform.localPosition.z);

        //GetComponent<Rigidbody>().velocity = Vector3.forward * 0.025f;
    }

    public void ChangeDir()
    {
        if (changingDir) return;
        //Debug.Log("Changing direction");
        changingDir = true;
        dir = -1;
        if (enable) StartCoroutine(GetComponent<SpidyAnims>().Rotate(Random.Range(90f, 200.1f)));
        else Invoke("ResetDir", 2.5f);
    }
    public void ResetDir()
    {
        dir = 1;
        changingDir = false;
    }
}
