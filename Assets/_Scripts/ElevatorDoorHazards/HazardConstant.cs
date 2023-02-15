using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardConstant : MonoBehaviour
{
   public PlatformMove platform;
   public hurtplayer hurtplayer;

    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponent<PlatformMove>();
    }

    private void OnCollisionEnter(Collision other) {
    if(other.transform.tag == "Player"){
        Debug.Log("Lasered: " + other.transform.name + " Tag: " + other.transform.tag);
        hurtplayer.hitPoint = 0;
    }

    }

    // Update is called once per frame
    void Update()
    {
        if (platform.canMove == false){
            platform.canMove = !platform.canMove;
        }
    }
}
