using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtplayer : MonoBehaviour
{


    GameObject player;

    //3 tap
    int hitPoint = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        switch (hitPoint)
        {
            case 0:
                print("Dead");
                break;
            case 1:
                print("Low");
                break;
            case 2:
                print("Med");
                break;
            case 3:
                print("High");
                break;

        }
            

        
        


    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Enemy")
        {
            Debug.Log("I've been injured");
            hitPoint--;

        }

    }

}
