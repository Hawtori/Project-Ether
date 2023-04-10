using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxOxygen, currentOxygen, normalizedOxygen;
    public GameObject oxygenScript, oxygenUIBar;


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        currentOxygen = oxygenScript.GetComponent<Oxygen>().currentAir;
        maxOxygen = oxygenScript.GetComponent<Oxygen>().maxAirCapacity;

        normalizedOxygen = (currentOxygen/maxOxygen);

        //Debug.Log(normalizedOxygen);

        oxygenUIBar.GetComponent<RectTransform>().localScale = new Vector3 (normalizedOxygen,1,1);


    }
}
