using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    //assignables
    [SerializeField]
    private Transform playerCam;
    private Rigidbody rb;

    //look around
    public float xSens = 90f, ySens = 90f;
    public float xRotationRecoil = 0;

    public float xRotation, yRotation;
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
        if(Instance == null) Instance = this;
        else Destroy(this);
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!PlayerPrefs.HasKey("xSens")) return;
        xSens = PlayerPrefs.GetFloat("xSens");
        ySens = PlayerPrefs.GetFloat("ySens");
    }

    private void Update()
    {
        GetInputs();
        Look();
        CheckGround();
    }

    private void FixedUpdate()
    {
        Gravity();
        Move();
        PerformJump();
    }

    //previous check for if grounded
    /*private void OnCollisionEnter(Collision collision)
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
    }*/
    
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    //}

    private void GetInputs()
    {
        x = y = 0;

        if (Input.GetKey(KeyCode.W)) y = 1;
        if (Input.GetKey(KeyCode.S)) y = -1;
        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;
                
       // jump = Input.GetButton("Jump");

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMultiplier = 1.35f;
            GetComponent<Oxygen>().SetRunning(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMultiplier = 1f;
            GetComponent<Oxygen>().SetRunning(false);
        }
    }

    private void CheckGround()
    {
        int layer = 6; //ground
        int layerMask = 1;
        layerMask = layerMask << layer;

        if(canJump && Physics.Raycast(transform.position, Vector3.down, 3.1f, layerMask))
        {
            isGrounded = true;

            jumping = false;
            jumpForceIncrease = 400f;
            jumpForce = 400;
            jumpTime = 0.2f;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime * sensMultiplier;

        Vector3 rotataion = playerCam.transform.localRotation.eulerAngles;
        yRotation = rotataion.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        float totalRotationX = xRotation - xRotationRecoil;
        totalRotationX = Mathf.Clamp(totalRotationX, -89f, 89f);
        playerCam.transform.localRotation = Quaternion.Euler(totalRotationX, yRotation, 0);
        rb.MoveRotation(Quaternion.Euler(0, yRotation, 0));
        //transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Move()
    {
        if (canJump && jump) Jump();

        //counter movement friction stuff
        float speed = rb.velocity.magnitude;
        if (speed > 0.001f)
        {
            float dropLim = Mathf.Max(speed, frictionThreshold);
            float dropAmount = Mathf.Max(0, speed - (dropLim * friction * Time.deltaTime));
           
            rb.velocity *= dropAmount / speed;
        }

        x = (y == 1 ? x/1.41423f : x) * moveSpeed * speedMultiplier;
        y = (x == 1 ? y/1.41423f : y) * moveSpeed * speedMultiplier;

        Vector3 forces = new Vector3(0, 0, 0);
        forces = transform.forward * y + transform.right * x;


        forces = new Vector3(forces.x == float.NaN ? 0 : Mathf.Clamp(forces.x, -200 * speedMultiplier, 200 * speedMultiplier), 
                             forces.y == float.NaN ? 0 : Mathf.Clamp(forces.y, -200 * speedMultiplier, 200 * speedMultiplier),
                             forces.z == float.NaN ? 0 : Mathf.Clamp(forces.z, -200 * speedMultiplier, 200 * speedMultiplier));
        
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

    public Vector2 GetMovement()
    {
        return new Vector2(x, y);
    }
}
