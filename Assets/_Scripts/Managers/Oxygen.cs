using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    public float maxAirCapacity;
    public float depleteAmount;
    public float depleteSpeed;

    public float recoverySpeed;

    private float currentAir;
    private float multiplier = 1;


    private void Start()
    {
        currentAir = maxAirCapacity;

        InvokeRepeating("DecreaseAir", 15f, depleteSpeed);
    }

    public void SetRunning(bool f)
    {
        if (f)
        {
            multiplier = 1.3f;
            CancelInvoke("ResetAir");
        }
        else Invoke("ResetAir", recoverySpeed);
    }

    private void ResetAir()
    {
        multiplier = 1;
    }

    private void DecreaseAir()
    {
        currentAir -= depleteAmount * multiplier;
        if(currentAir < 0)
        {
            GetComponent<hurtplayer>().TakeDamage(1);
            currentAir = 0;
        }
    }

    public void IncreaseAir(float amount)
    {
        currentAir += amount;
        currentAir = Mathf.Clamp(currentAir, 0, maxAirCapacity);
    }
}
