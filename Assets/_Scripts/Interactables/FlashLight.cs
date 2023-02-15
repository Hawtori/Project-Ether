using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{

   public GameObject flashLight, uiFlashIcon;

   public float onDuration = 1;
   public float maxDuration = 90; //seconds
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


        uiFlashIcon.GetComponent<RawImage>().color = new Color(0,1 - (onDuration/maxDuration),0,1);
        flashLight.GetComponent<Light>().color = new Color(1 - (onDuration/maxDuration),1 - (onDuration/maxDuration),1 - (onDuration/maxDuration),1);

    }
}
