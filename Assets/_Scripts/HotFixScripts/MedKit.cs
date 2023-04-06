using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MedKit : MonoBehaviour
{
    private int healthValue;

    private void Start()
    {
        healthValue = Random.Range(1, 4);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<hurtplayer>().TakeDamage(-healthValue);
            other.GetComponent<Oxygen>().IncreaseAir(healthValue * 25f);
            
            Destroy(gameObject);   
            
        }
    }
}
