using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum State
    {
        goingA,
        goingB,
        goingC
    }

    private Rigidbody rb;

    //x: 1 , 3
    //y = 3
    //z = 4, 1
    private Vector3 bound1 = new Vector3(242, 1, -238), bound2 = new Vector3(-231, 1, 233), bound3 = new Vector3(-237, 1, -237), bound4 = new Vector3(235, 1, 235);

    //go between three positions
    private Vector3 posA, posB, posC;

    private int currentState; // 0 = going to A, 1 = going to B, 2 = going to C
    private float speed = 5f;

    private Vector3 movementVector;
    
    private float desiredSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        posA = new Vector3(
            Random.Range(bound1.x, bound3.x),
            1f,
            Random.Range(bound4.z, bound1.z)
            );

        posB = new Vector3(
            Random.Range(bound1.x, bound3.x),
            1f,
            Random.Range(bound4.z, bound1.z)
            );
        
        posC = new Vector3(
            Random.Range(bound1.x, bound3.x),
            1f,
            Random.Range(bound4.z, bound1.z)
            );

        currentState = (int)State.goingA;
    }

    private void Update()
    {
        ChangeState();

        switch (currentState)
        {
            case 0:
                movementVector = (transform.position - posA).normalized;
                desiredSpeed = Mathf.Clamp(Vector3.Distance(transform.position, posA), 0f, speed);
                break;
            case 1:
                movementVector = (transform.position - posB).normalized;
                desiredSpeed = Mathf.Clamp(Vector3.Distance(transform.position, posB), 0f, speed);
                break;
            case 2:
                movementVector = (transform.position - posC).normalized;
                desiredSpeed = Mathf.Clamp(Vector3.Distance(transform.position, posC), 0f, speed);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        movementVector *= desiredSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movementVector);
    }

    private void ChangeState()
    {
        switch (currentState)
        {
            case 0:
                if(Vector3.Distance(transform.position, posA) < 3f)
                {
                    if (Random.Range(1, 3) == 1) currentState = (int)State.goingB;
                    else currentState = (int)State.goingC;
                }
                break;
            case 1:
                if (Vector3.Distance(transform.position, posB) < 3f)
                {
                    if (Random.Range(1, 3) == 1) currentState = (int)State.goingA;
                    else currentState = (int)State.goingC;
                }
                break;
            case 2:
                if (Vector3.Distance(transform.position, posC) < 3f)
                {
                    if (Random.Range(1, 3) == 1) currentState = (int)State.goingB;
                    else currentState = (int)State.goingA;
                }
                break;
            default:
                break;
        }
    }

}
