using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //this is so camera movement is smooth and nice
    //putting camera as child of player makes it jittery and not nice

    public Transform head;

    private Transform initHeadPos;

    //private float moveTime = 0;
    //private bool goUp = true;

    //private Vector3 vel = Vector3.zero;
    private Vector3 LerpV3(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }

    private void Awake()
    {
        initHeadPos = head;
    }

    
    private void Update()
    {
        transform.position = head.position;

    }
}
