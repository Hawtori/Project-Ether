using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

   public GameObject flashLight;

   float onDuration = 0;
   float maxDuration = 60;
   bool canTurnOn = true;
   int flickerAmount = 1000;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.F) && canTurnOn)
        {    
            flashLight.GetComponent<Light>().enabled = !flashLight.GetComponent<Light>().enabled;
        }

        if (flashLight.GetComponent<Light>().enabled)
        {
            onDuration += Time.deltaTime;
           // print("On duration: " + onDuration);
        }

        if(onDuration > maxDuration)
        {
          

            for (int i = 0; i < flickerAmount; i++)
            {
                flashLight.GetComponent<Light>().enabled = !flashLight.GetComponent<Light>().enabled;
                
            }

            flashLight.GetComponent<Light>().enabled = false;
            canTurnOn = false;
        }

    }
}
