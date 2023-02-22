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
    [Range(1f, 50f)][SerializeField] private float jumpForce;
    [Range(0, .3f)] [SerializeField] private float acceleration;
    [SerializeField] private LayerMask colliderMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;

    bool grounded;
    float move;
    bool jump = false;
    bool endJump = false;
    Vector3 refVel = Vector3.zero;
  
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

 
    private void FixedUpdate()
    {   
        checkGround();

        movement();

        flipSprite();
    }

    private void movement(){
        if(jump && grounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
        if(endJump)
        {
            var opposite = -rigidBody.velocity;
            rigidBody.AddForce(opposite * Time.fixedDeltaTime, ForceMode2D.Impulse);
            endJump = false;
        }

        transform.Translate(Vector3.right * move * runSpeed * Time.fixedDeltaTime);
    }

    private void checkGround(){

        
        grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, .2f, colliderMask);
        Color rayColor = grounded ? Color.red : Color.green;
        Debug.DrawRay(groundCheck.position, Vector2.down * .2f, rayColor,.2f);

    }

    private void flipSprite()
    {
        if(move < 0)
        {
            sprite.flipX = true;
        }else if(move > 0)
        {
            sprite.flipX = false;
        }
        animator.SetFloat("Speed",Mathf.Abs(move));
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
        }
        if (context.canceled || context.performed)
        {
            endJump = true;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Single>();
    }
}
