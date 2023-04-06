using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverStuff : MonoBehaviour
{


    public GameObject woahEnemy;

    public bool pulled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        

    }

    private void OnMouseDown() {
        
        if(pulled == false){
            transform.Rotate(new Vector3(transform.localRotation.x,transform.localRotation.y,90));
            woahEnemy.SetActive(true);
        }
    

    }

}
