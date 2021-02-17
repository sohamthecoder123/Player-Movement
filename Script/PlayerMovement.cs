using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    public float speed;

    float x, z;

    public Transform groundCheck;
    bool isGrounded;

    Vector3 currentVelocity;

    public float jumpForce;
    int jumps;

    bool isWallNear, isWallRunning;
    bool isWallLeft, isWallRight;
    public LayerMask whatIsWall;
    Vector3 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        Jumping();
    }

    void FixedUpdate()
    {
        Movement();
    }


    void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        z = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        isGrounded = Physics.Raycast(groundCheck.position, -transform.up, 0.4f);

        WallRunInput();
    }

    void Movement()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.forward * z + transform.right * x, ForceMode.Force);
            currentVelocity = rb.velocity;

            rb.drag = 1f;
        }

        WallRunning();
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(currentVelocity.x, jumpForce, currentVelocity.z, ForceMode.Impulse);
                jumps = 1;
            }

            else if (!isGrounded && jumps == 1)
            {
                rb.AddForce(currentVelocity.x, jumpForce, currentVelocity.z, ForceMode.Impulse);
                jumps = 2;
            }

            else if (isWallRunning)
            {
                rb.AddForce(currentVelocity.x, jumpForce, currentVelocity.z, ForceMode.Impulse);

                if (isWallLeft)
                {
                    Vector3 wallRunMove = transform.right * 20;
                    rb.AddForce(wallRunMove, ForceMode.Impulse);
                }

                else if (isWallRight)
                {
                    Vector3 wallRunMove = transform.right * -20;
                    rb.AddForce(wallRunMove, ForceMode.Impulse);
                }
            }
        }
    }

    void WallRunInput()
    {
        isWallNear = Physics.CheckSphere(transform.position, 1f, whatIsWall);

        if (!isGrounded)
            isWallRunning = isWallNear;

        if (isWallRunning)
        {
            isWallLeft = Physics.Raycast(transform.position, -transform.right, 0.6f, whatIsWall);
            isWallRight = Physics.Raycast(transform.position, transform.right, 0.6f, whatIsWall);
        }
    }

    void WallRunning()
    {
        if (!isWallRunning)
        {
            currentDirection = transform.forward;
            StopWallRun();
        }

        else
        {
            StartWallRun();
        }
           
    }

    void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(currentDirection/4, ForceMode.Impulse);
    }

    void StopWallRun()
    {
        rb.useGravity = true;
    }
}
