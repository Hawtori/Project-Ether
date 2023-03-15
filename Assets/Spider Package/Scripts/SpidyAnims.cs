using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.Playables;

public class SpidyAnims : MonoBehaviour
{
    // point is where the leg is
    // target is where the leg should go

    // can't really go faster than 0.25 but see if we can fix it laterrr (if we need)
    // really just works well on flat ground, can climb and desend slopes as long as it isn't too steep
    // will try for future to make it so it doesn't matter the terrain 

    public Transform[] targets;
    public Transform[] points;

    public Transform[] legTargets;

    public bool[] legMoving;

    public float maxDistance = 0.15f;
    public float smooth = 5f;
    public float stepHeight = 0.1f;

    private int nLegs;
    private Vector3[] raycastPos;
        
    //public Vector3 forward;

    public Transform[] frontTarget;

    private bool enable = true;

    //private int indexToMove = -1;

    private void Start()
    {
        nLegs = targets.Length;
        legMoving = new bool[nLegs];
        raycastPos = new Vector3[nLegs];

        for (int i = 0; i < nLegs; i++) raycastPos[i] = targets[i].position;
    }

    private void Update()
    {
        for (int i = 0; i < nLegs; i++)
        {
            legTargets[i].position = points[i].position;
            raycastPos[i] = targets[i].position;
        }
    }

    private void FixedUpdate()
    {
        if (!enable) return;
        for (int i = 0; i < nLegs; i++)
        {
            // ray cast down to find point of contact with floor
            RaycastHit hit;
            Ray ray = new Ray(raycastPos[i] + transform.up * 0.02f, -transform.up);

            //Debug.DrawRay(raycastPos[i] + transform.up * 0.02f, -transform.up * 1f, Color.yellow, 0.2f);

            if (Physics.Raycast(ray, out hit, 1f))
            {
                targets[i].position = hit.point;
            }

            //Debug.DrawLine(pos, -transform.up * 8f, Color.yellow, 0.75f);
        }

        // check if we can move forward
        for(int i = 0; i < frontTarget.Length; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(frontTarget[i].position + transform.up * 0.02f, -transform.up);

            //Debug.DrawLine(frontTarget.position + transform.up * 0.2f, frontTarget.position * 0.05f * 0.2f, Color.yellow, 0.2f);
            //Debug.DrawRay(frontTarget[i].position + transform.up * 0.02f, -transform.up * 0.2f, Color.yellow, 0.2f);

            if (!Physics.Raycast(ray, out hit, 0.2f)) // can not move
                GetComponent<SpiderMove>().ChangeDir();

        }



        for (int i = 0; i < legMoving.Length; i++) if (legMoving[i]) return;
        for (int i = 0; i < nLegs; i++)
        {
            // check if the distance between the point and the target is greater than max
            if (Vector3.Distance(targets[i].position, points[i].position) > (maxDistance) && legMoving[i] == false/* && indexToMove == -1*/)
            {
                //indexToMove = i;
                StartCoroutine(Step(i, targets[i].position));
                break;
                //points[i].position = targets[i].position;
            }
        }
        //if(indexToMove != -1) StartCoroutine(Step(indexToMove, targets[indexToMove].position));
    }

    public void PlayerDetected(Vector3 pos)
    {
        enable = false;
        for(int i = 0; i < targets.Length; i++)
        {
            targets[i].position = pos;
            points[i].position = pos;
        }
    }

    public IEnumerator Rotate(float angle)
    {
        for(int i = 0; i < 1.6f * Mathf.Abs(angle); i++)
        {
            yield return new WaitForFixedUpdate();
            transform.Rotate(0f, 0.5f, 0f);
        }
        GetComponent<SpiderMove>().ResetDir();
    }
    
    IEnumerator Step(int index, Vector3 target)
    {
        Vector3 startPos = points[index].position;
        legMoving[index] = true;
        for (int i = 0; i <= smooth; i++)
        {
            points[index].position = Vector3.Lerp(startPos, target, i / (smooth + 1f));

            points[index].position += transform.up * Mathf.Sin(i / (smooth + 1f) * Mathf.PI) * stepHeight;

            yield return new WaitForFixedUpdate();
        }

        points[index].position = target;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        legMoving[index] = false;

        //indexToMove = -1;
    }
}
