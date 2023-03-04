using JetBrains.Annotations;
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

    [SerializeField] private LayerMask colliderMask;
    [SerializeField] private Transform groundCheckLeft,groundCheckRight;
    [SerializeField] private Animator animator;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityMultiplier = 2f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpCutMultiplier = 1.0f;
    [Space(10)]
    [SerializeField] private float lastGroundedTime = 0;
    [SerializeField] private float lastJumpTime = 0;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool jumpImputReleased = false;
    [SerializeField] private float maxVerticalSpeed = 20f;
    
    [Header("Dash")]
    [SerializeField] private TrailRenderer TR;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool dashing = false;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;

    private bool grounded;
    private float move;
    
  
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

    //----------------------------- Movement Calculations --------------------------------//
    private void Movement(){
        
        SetFallGravity();

        SetTimers();

        if(lastGroundedTime> 0 && lastJumpTime > 0 && !isJumping) 
        {           
            Jump();
        }

        Run();

        ApplyFriction();
        
    }

    private float calculateJump()
    {
        float calculatedJump = 0;

        setJumpTime();
        setJumpBuffer();
        setCoyoteTime();

        if(jumpBufferCounter > 0.0f && !playerIsJumping && coyoteTimeCounter > 0.0f)
        {
            calculatedJump = jumpForce;
            playerIsJumping = true;
            jumpBufferCounter = 0.0f;
            coyoteTimeCounter = 0.0f;
        }
        else if(jump && playerIsJumping && !grounded && jumpTimeCounter > 0.0f)
        {
            calculatedJump = jumpForce * jumpMultiplier;
        }
        else if(playerIsJumping && grounded)
        {
            playerIsJumping = false;
        }

        return calculatedJump;
    }

    private void setJumpTime()
    {
        if(playerIsJumping && !grounded)
        {
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            jumpTimeCounter = jumpTime;
        }
    }

    private void setCoyoteTime()
    {
        if(grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void setJumpBuffer()
    {
        if (!jumpPressedLastFrame && jump)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else if(jumpBufferCounter > 0.0f)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }
        jumpPressedLastFrame = jump;
    }

    private void checkGround(){
        bool groundedA = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, .1f, colliderMask);
        bool groundedB = Physics2D.Raycast(groundCheckRight.position, Vector2.down, .1f, colliderMask);
        if(groundedA || groundedB)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        Color rayColor = grounded ? Color.red : Color.green;
        Debug.DrawRay(groundCheckLeft.position, Vector2.down * .1f, rayColor,.2f);
        Debug.DrawRay(groundCheckRight.position, Vector2.down * .1f, rayColor, .2f);

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
        if (!dashing)
        {
            if (context.started)
            {
                lastJumpTime = jumpBufferTime;
            }
            if (context.performed || context.canceled)
            {
                if(rigidBody.velocity.y > 0 && isJumping) {
                    rigidBody.AddForce(Vector2.down* rigidBody.velocity.y * (1-jumpCutMultiplier),ForceMode2D.Impulse);
                }
                jumpImputReleased = true;
                lastJumpTime = 0;
            }

            
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            move = context.ReadValue<Single>();
            if(move > 0)
            {
                move = 1;
            }
            else if(move < 0)
            {
                move = - 1;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            jump = false;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Single>();
    }
}
