using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMove : MonoBehaviour
{
    
    [SerializeField]
    GameObject canvas, hud;

    [SerializeField]
    Vector2[] hudPositions = {new Vector2(0,0),new Vector2(0,0)};

    [SerializeField]
    float desiredDuration = 3f;

    [SerializeField]
    bool  uiExtended = false;
    private float elapsedTime = 0;
 

    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {

        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime/desiredDuration;

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            uiExtended = !uiExtended;

            elapsedTime = 0;
        }

        if(uiExtended)
        {

            hud.GetComponent<RectTransform>().position = Vector2.Lerp(hudPositions[1],hudPositions[0],percentageComplete);

        }    

        if(!uiExtended ){

          hud.GetComponent<RectTransform>().position = Vector2.Lerp(hudPositions[0],hudPositions[1],percentageComplete);
        }


//        Debug.Log(uiExtended);

    }
}
