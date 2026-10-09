using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BasicPlayerController : MonoBehaviour {
    public float moveSpeed = 3f;
    public float jumpForce = 100f;

    private Rigidbody2D rb;

    public bool isGrounded = true;
    public bool isPlayer2;

    InputAction moveAction;
    InputAction jumpAction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(!isPlayer2){
            moveAction = InputSystem.actions.FindAction("MoveP1");
            jumpAction = InputSystem.actions.FindAction("JumpP1");
        } else {
            moveAction = InputSystem.actions.FindAction("MoveP2");
            jumpAction = InputSystem.actions.FindAction("JumpP2");
        }
        
    }
    private void FixedUpdate()
    {
        
        //Move left or right
        float x = moveAction.ReadValue<Vector2>().x;
        //Construct the moving direction vector from keyboard input
        Vector3 direction = new Vector3(x, 0, 0);

        //Move the transform (i.e., the gameObject) in the given direction and distance
        //The distance is determined by how long you've pressed the keys, and the moveSpeed
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        //If Spacebar is pressed: jump
        if (jumpAction.IsPressed() && isGrounded) { 
            rb.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
            Debug.Log(rb.linearVelocityY);
            isGrounded = false;
        }         
    }
    public void setGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}

