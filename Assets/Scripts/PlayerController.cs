using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /***********************************************************************************
                                THINGS TO DO
    - improve overall game feel
    
                                MABIES
    - add a slow effect for when landing (unless bunny hopping so falling makes player slow on impact)
    - add crouching
    ***********************************************************************************/

    //assign from unity
    public Transform playerCam;
    public Transform orientation;

    public Rigidbody rb;

    //look around
    //will put editing this in a separate function later (TO DO)
    public float sensX = 50f, sensY = 50f; //so we can edit horizontal and vertical sens independently

    private float xRotation, desiredY;
    private float sensMultiplier = 1f;

    //move around
    public float moveSpeed = 10000f; //put high number so acceleration isn't slow
    public float maxSpeed = 2;
    public bool grounded;
    //public LayerMask _isGround; //this can be used later on depending on what direction the game goes
    private float speedMultiplier = 1; //1 for main weapon, 1.5 for pistol, 2.125 for knife or something like that

    //jump
    [SerializeField]
    private float jumpForce = 1000f;
    private static float jumpCD = 0.25f; //set time
    private bool canJump = true;

    //inputs
    private float x, y;
    private bool jump;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        //hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //transform.localRotation = orientation.localRotation;
        GetInputs();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

    /// <summary>
    /// Get user inputs for movement
    /// </summary>
    private void GetInputs()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");

        //normalize movement so diagnal movement isn't faster
        Vector2 normalizedMovement = new Vector2(x, y);
        normalizedMovement.Normalize();

        //x = normalizedMovement.x; y = normalizedMovement.y;

        Debug.Log("X: " + x + " Y: " + y);  
    }

    /// <summary>
    /// Get mouse inputs to look around 
    /// </summary>
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rotation = playerCam.transform.localRotation.eulerAngles;
        desiredY = rotation.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //lock rotation

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredY, 0);
        //orientation.transform.localRotation = Quaternion.Euler(0, desiredY, 0);
        transform.localRotation = Quaternion.Euler(0, desiredY, 0);
    }

    /// <summary>
    /// Execute the movement
    /// </summary>
    private void Move()
    {
        if (canJump && jump) Jump();

        Vector3 mag = playerCam.transform.rotation * playerCam.transform.TransformDirection(rb.velocity);
        float xMag = mag.x, yMag = mag.z;
        float speed = rb.velocity.magnitude;

        /**********counter movement**********/

        //slow down if key released
        //if (grounded && x == 0 && y == 0 && Mathf.Abs(rb.velocity.magnitude) > 0) { rb.velocity = rb.velocity / divFactor; divFactor = Mathf.Clamp(divFactor - 0.5f, 1f, 5f); }
        //if (Mathf.Abs(xMag) > 0.2 && Mathf.Abs(x) < 0.05f || (xMag < -0.2 && x > 0) || (xMag > 0.2 && x < 0)) rb.AddForce(moveSpeed * orientation.transform.right * -xMag * 0.2f);
        //if (Mathf.Abs(yMag) > 0.2 && Mathf.Abs(y) < 0.05f || (yMag < -0.2 && y > 0) || (yMag > 0.2 && y < 0)) rb.AddForce(moveSpeed * orientation.transform.forward * -yMag * 0.2f);

        //if(speed  > 0.001f)
        //{
        //    float dropLim = Mathf.Max(speed, 0.5f);
        //    float dropAmount = Mathf.Max(0, speed - (dropLim * 15f * Time.deltaTime));

        //    rb.velocity *= dropAmount / speed;
        //}

        //https://github.com/atil/fpscontroller/blob/master/Assets/Scripts/FpsController.cs
        
        //if (x == 0 && y == 0)
        //{
        //    //rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //    if(y != 0 && yMag != 0)
        //    rb.AddForce(transform.forward * -yMag * moveSpeed * speedMultiplier);
        //    if(x != 0 && xMag != 0)
        //    rb.AddForce(transform.right * -xMag * moveSpeed * speedMultiplier);
        //}

        //don't take input if over max speed;
        if (x > 0 && xMag > maxSpeed)  x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed)  y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        rb.AddForce(transform.forward * y * moveSpeed * speedMultiplier * Time.deltaTime);
        rb.AddForce(transform.right * x * moveSpeed * speedMultiplier * Time.deltaTime);

    }

        private void Jump()
        {
            if (grounded && canJump)
            {
                canJump = false;

                rb.AddForce(Vector2.up * jumpForce * 2f);

                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                else if (rb.velocity.y > 0)
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

                Invoke("ResetJump", jumpCD);
            }
        }

        private void ResetJump()
        {
            canJump = true;
        }
}

