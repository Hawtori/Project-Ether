using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPort : MonoBehaviour
{

    public BoxCollider batteryTriggerBox;
    public GameObject itemToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Temporary
        if (other.tag == "Battery")
        {
            Debug.Log("BATTERY INSTALLED");
            itemToActivate.GetComponent<Renderer>().material.color = Color.green;
            itemToActivate.transform.position = itemToActivate.transform.position + new Vector3(0, 8, 0);
        }

   
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }
}
