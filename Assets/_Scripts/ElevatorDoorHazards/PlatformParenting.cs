using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

private void OnCollisionEnter(Collision other) {
    other.transform.SetParent(transform);
}

private void OnCollisionExit(Collision other) {
    if(other.transform.tag == "Player"){
        other.transform.SetParent(GameObject.Find("===Player===").transform);
    }
}




}
