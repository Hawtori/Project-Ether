using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float bobSpeed = 0.18f;
    public float bobAmount = 0.2f;
    public float midPpoint = 2f;
    public float speedThreshold = 0.23f;

    private float timer = 0f;
    private float yPos;

    private float waveSlice;

    public Rigidbody player;


    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (player.velocity.magnitude < speedThreshold) timer = 0f;
        else timer += bobSpeed;

        if (timer > Mathf.PI * 2) timer = timer - (Mathf.PI * 2);

        waveSlice = Mathf.Sin(timer);

        if (waveSlice != 0)
        {
            float change = waveSlice * bobAmount;
            float t = Mathf.Clamp01(player.velocity.magnitude);
            change *= t;
            yPos = midPpoint + change;
            Vector3 pos = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
            transform.localPosition = pos;
        }
    }
}
