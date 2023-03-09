using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpider : MonoBehaviour
{
    bool turning = false;

    public void TurnSpider(float degrees)
    {
        if(turning) return;
        turning = true;
        StartCoroutine(Rotate(degrees));
    }

    IEnumerator Rotate(float degrees)
    {
        for(int i = 0; i < 10; i++)
        {
            transform.Rotate(0f, degrees / 10f, 0f);
            yield return new WaitForFixedUpdate();
        }
        turning = false;
    }
}
