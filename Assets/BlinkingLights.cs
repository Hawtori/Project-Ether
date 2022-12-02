using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLights : MonoBehaviour
{

    public GameObject[] lightsToBlink;
    private float deltaStart, blinkDuration;
    public Material[] onOffMaterial;


    // Start is called before the first frame update
    void Start()
    {
        blinkDuration = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        deltaStart += Time.deltaTime;

        //print(deltaStart-Time.deltaTime);

        if ((deltaStart - Time.deltaTime) >= blinkDuration)
        {
          

            foreach (GameObject i in lightsToBlink)
            {
               // print(i.name);

                i.GetComponent<Light>().enabled = !i.GetComponent<Light>().enabled;


                if (i.GetComponent<Light>().enabled == true)
                {
                    i.transform.GetChild(0).GetComponent<Renderer>().material = onOffMaterial[1];
                }

                if (i.GetComponent<Light>().enabled == false)
                {
                    i.transform.GetChild(0).GetComponent<Renderer>().material = onOffMaterial[0];
                }

            }

            deltaStart = 0;

        }


    }
}
