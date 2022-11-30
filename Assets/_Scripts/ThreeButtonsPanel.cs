using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Moved to raycastinginteractions
 */

public class ThreeButtonsPanel : MonoBehaviour
{

    public GameObject ButtonOne, ButtonTwo, ButtonThree;

    public int[] sequenceArr = { 1, 2, 3, 1, 2, 3 };
    private int[] guessArr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

        //Future sight possible issues. To many update frames adding to many index per 1 interact. Solution delta time timer?

        //Use raycast + E to interact.

        //Button 1
        if (true)
        {
            //Add an index to guessArr with number 1
        }

        //Button 3
        if (true)
        {
            //Add an index to guessArr with number 2
        }
        //Button 3
        if (true)
        {
            //Add an index to guessArr with number 3
        }



    }
}
