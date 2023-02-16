using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float runSpeed = 40f;
    [Range(1f, 10f)][SerializeField] private float jumpForce;
    [Range(0, .3f)] [SerializeField] private float acceleration;
    [SerializeField] private LayerMask colliderMask;
    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Vector3 velocity = Vector3.zero;

    bool grounded;
    float move;
    bool jump = false;
    bool isJumping = false;
    float jumpAcceleration = 10f;
  
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

 
    private void FixedUpdate()
    {   
        checkGround();

        movement();
        Debug.Log(isJumping);
    }

    private void movement(){
        if(jump && grounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
        else if(isJumping && !jump)
        {
            rigidBody.AddForce(Vector2.up * jumpAcceleration, ForceMode2D.Force);
        }
    }

    private void checkGround(){

        
        grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, .2f, colliderMask);
        Color rayColor = grounded ? Color.red : Color.green;
        Debug.DrawRay(groundCheck.position, Vector2.down * .2f, rayColor);

    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
            isJumping = true;
        }
        if (context.canceled)
        {
            isJumping = false;
        }
        if (context.performed)
        {
            isJumping = false;
        }
        
    }

    public void onMove(InputAction.CallbackContext context)
    {

        move = context.ReadValue<Single>() * runSpeed;
        
    }
}
