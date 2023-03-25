using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public PlatformMove platform;

    public GameObject itemToMove;

    private void Start() {
        platform = GetComponent<PlatformMove>();
    }

private void OnCollisionEnter(Collision other) {
    if(other.transform.tag == "Player"){
        other.transform.SetParent(transform);

       itemToMove = other.transform.gameObject;

    }
           
}

private void OnCollisionExit(Collision other) {
    if(other.transform.tag == "Player"){
        other.transform.SetParent(GameObject.Find("===Player===").transform);
          
        
    }
}

private void Update() {
    if(platform.canMove){
        itemToMove.transform.position = transform.position + new Vector3(0,3.5f,0);
    }
}

}
