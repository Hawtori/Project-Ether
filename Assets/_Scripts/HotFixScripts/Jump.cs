using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

float timeCollector,desiredTime = 1;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        timeCollector += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timeCollector - Time.deltaTime > desiredTime ){
            GetComponent<Rigidbody>().AddForce(new Vector3(0,10 * 8 ,0),ForceMode.Impulse);

            timeCollector = 0;
        }

    }
}
