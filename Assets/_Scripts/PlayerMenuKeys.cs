using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuKeys : MonoBehaviour
{
    public CanvasGroup canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && canvas.alpha == 1f)
        {
            canvas.alpha = 0f;
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            canvas.alpha = 1f;
        }
    }
}
