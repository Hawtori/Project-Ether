using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapscript : MonoBehaviour
{

    public GameObject minimap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            minimap.GetComponent<Canvas>().enabled = !minimap.GetComponent<Canvas>().enabled;
        }


    }
}
