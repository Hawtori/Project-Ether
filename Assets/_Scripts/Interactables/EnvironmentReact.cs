using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentReact : MonoBehaviour
{

    public GameObject ThingToInvoke;


    public static Vector3 openDoorInstant(Vector3 direction, int distance)
    {
        Vector3 woahNewPosition = new Vector3(0, 0, 0);

  
       // woahNewPosition = new Vector3(0, 0, 0);
        

        return woahNewPosition + direction * distance;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
