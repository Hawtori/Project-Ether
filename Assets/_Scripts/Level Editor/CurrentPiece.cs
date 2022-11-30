using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPiece : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collioded");
        SpawnPieces._instance.confirm = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("coliding");
    }
}
