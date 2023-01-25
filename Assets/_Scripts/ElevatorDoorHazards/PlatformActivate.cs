using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivate : MonoBehaviour
{

    public PlatformMove platform;

    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponent<PlatformMove>();
    }

    public void triggerPlatform(){
        platform.canMove = !platform.canMove;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
