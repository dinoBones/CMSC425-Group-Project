using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code based on tutorial from Dave / GameDevelopment on YouTube
// https://www.youtube.com/watch?v=f473C43s8nE&t=128s
public class WizardMovement : MonoBehaviour
{

    public float sensX;
    public float sensY;

    float rotationX;
    float rotationY;

    // Movement Variables
    public float moveSpeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    // Drag + Grounded Variables
    public float playerHeight;
    public LayerMask ground;
    bool onGround;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    public KeyCode jumpKey = KeyCode.Space;


    Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
        player.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f , ground);

        PlayerInput();
        SpeedControl();

        if(onGround) {
            player.drag = groundDrag;
        } else {
            player.drag = 0;
        }

        float moveX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float moveY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        rotationY += moveX; // rotation around y axis vs movement on x axis

        rotationX -= moveY; // rotation around x axis vs movement on y axis
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // mimics head movement limits

        // move camera
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    void FixedUpdate() 
    {
        MovePlayer();
    }

    private void PlayerInput() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && onGround) 
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); // resets jump after jumpCooldown delay
        }
    }

    private void MovePlayer() 
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(onGround) {
            player.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else if (!onGround) {
            player.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl() 
    {
        Vector3 velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);

        if(velocity.magnitude > moveSpeed) {
            Vector3 velocityMax = velocity.normalized * moveSpeed;
            player.velocity = new Vector3(velocityMax.x, velocityMax.y, velocityMax.z);
        }
    }

    private void Jump() 
    {
        player.velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);

        player.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    // INVENTORY SYSTEM - MADE BY CONNOR

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the specific object you want
        if (collision.gameObject.CompareTag("Loot"))
        {
            Debug.Log("loot function");
        }
    }

    //
}
