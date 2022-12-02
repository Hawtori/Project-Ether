using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hurtplayer : MonoBehaviour
{


    public GameObject player, pain, dead, win;

    //3 tap
    int hitPoint = 4;

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
            //    print("Dead");
                //pain.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                dead.SetActive(true);
                Time.timeScale = 0; // Pause game
                break;
            case 1:
             //   print("Dead");
                pain.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                break;
            case 2:
             //   print("Low");
                pain.GetComponent<Image>().color = new Color(255, 255, 255, 0.75f);
                break;
            case 3:
               // print("Med");
                pain.GetComponent<Image>().color = new Color(255, 255, 255, 0.50f);
                break;
            case 4:
               // print("High");
                pain.GetComponent<Image>().color = new Color(255, 255, 255, 0);
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
