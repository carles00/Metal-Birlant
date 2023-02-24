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
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpMultiplier = 0.1f;
    [SerializeField] private float jumpTime = 1.0f;
    [SerializeField] private float jumpTimeCounter = 0.0f;
    [SerializeField] private float coyoteTime = .2f;
    [SerializeField] private float coyoteTimeCounter = 0.0f;
    [SerializeField] private float jumpBufferTime = .2f;
    [SerializeField] private float jumpBufferCounter = 0.0f;
    [SerializeField] private bool jump = false;
    [SerializeField] private bool playerIsJumping = false;
    [SerializeField] private bool jumpPressedLastFrame = false;

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
    private int facing = 1;
  
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

 
    private void FixedUpdate()
    {
        if (!dashing)
        {
            CheckGround();

            Movement();

            FlipSprite();
        }
    }

    private void Movement(){
        
        float calculatedJump = CalculateJump();

        rigidBody.AddForce(Vector2.up * calculatedJump, ForceMode2D.Force);

        transform.Translate(Vector3.right * move * runSpeed * Time.fixedDeltaTime);
        
    }
    private float CalculateJump()
    {
        float calculatedJump = 0;

        SetJumpTime();
        SetJumpBuffer();
        SetCoyoteTime();

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

    private void SetJumpTime()
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

    private void SetCoyoteTime()
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

    private void SetJumpBuffer()
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

    private void CheckGround(){
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

    private void FlipSprite()
    {
        if(move < 0)
        {
            facing = -1;
            sprite.flipX = true;
        }else if(move > 0)
        {
            facing = 1;
            sprite.flipX = false;
        }
        animator.SetFloat("Speed",Mathf.Abs(move));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            if (context.started)
            {
                jump = true;
            }
            if (context.performed || context.canceled)
            {
                jump = false;
            }
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            move = context.ReadValue<Single>();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Debug.Log("DASH!");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        dashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(dashPower * facing, 0f);

        yield return new WaitForSeconds(dashTime);
        dashing = false;
        rigidBody.gravityScale = originalGravity;
        rigidBody.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
