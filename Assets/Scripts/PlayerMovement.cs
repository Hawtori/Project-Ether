using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //assignables
    [SerializeField]
    private Transform playerCam;
    private Rigidbody rb;

    //look around
    public float xSens = 40f, ySens = 40f;

    private float xRotation, yRotation;
    private float sensMultiplier = 1.3726f;

    //move around
    public float maxSpeed = 10f;
    public float speedMultiplier = 1f;

    private bool isGrounded;
    private float moveSpeed = 200f;

    //jump
    private float jumpForce = 400f;
    private float jumpForceIncrease = 400f;
    private float jumpTime = 0.2f;
    private static float jumpCD = 0.5f;
    private bool canJump = true;
    private bool jumping = false;

    //inputs
    private float x, y;
    private bool jump;

    //physics stuff
    [SerializeField]
    private float friction = 15f;
    [SerializeField]
    private float frictionThreshold = 0.35f;
    private float gravityAmount = 35f; //23
    private float gravityChange = 3f;
    private float gravityChangeDelta = 4f;

    //public float yVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInputs();
        Look();
    }

    private void FixedUpdate()
    {
        Gravity();
        Move();
        PerformJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            //reset variables
            jumping = false;
            jumpForceIncrease = 400f;
            jumpForce = 400;
            jumpTime = 0.2f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void GetInputs()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime * sensMultiplier;

        Vector3 rotataion = playerCam.transform.localRotation.eulerAngles;
        yRotation = rotataion.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        rb.MoveRotation(Quaternion.Euler(0, yRotation, 0));
        //transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Move()
    {
        if (canJump && jump) Jump();

        //counter movement
        float speed = rb.velocity.magnitude;
        if (speed > 0.001f)
        {
            float dropLim = Mathf.Max(speed, frictionThreshold);
            float dropAmount = Mathf.Max(0, speed - (dropLim * friction * Time.deltaTime));
           
            rb.velocity *= dropAmount / speed;
        }

        x = (x/1.41423f) * moveSpeed * speedMultiplier;
        y = (y/1.41423f) * moveSpeed * speedMultiplier;

        Vector3 forces = transform.forward * y + transform.right * x;

        rb.AddForce(forces);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumping = true;
            canJump = false;

            Invoke("ResetJump", jumpCD);
        }
    }

    private void PerformJump()
    {
        if (jumping)
        {
            if(jumpTime > 0f)
            {
                gravityChange = 3f;
                jumpForce += jumpForceIncrease * Time.deltaTime;
                jumpForceIncrease -= 25f; //*/ 400 * Time.deltaTime;
                jumpForce = Mathf.Clamp(jumpForce, 300, 400);

                jumpTime -= Time.deltaTime;
                rb.AddForce(Vector3.up * jumpForce);
                gravityChange = 0.65f;
            }
            else
            {
                gravityChange -= gravityChangeDelta * Time.deltaTime;
                gravityChangeDelta -= 0.01f;
                gravityChange = Mathf.Clamp(gravityChange, 0.075f, 0.65f);
            }
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Gravity()
    {
        rb.AddForce(Vector3.down * (gravityAmount / gravityChange));
    }
}
