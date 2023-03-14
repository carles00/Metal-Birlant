using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask colliderMask;
    [SerializeField] private Transform groundCheckLeft, groundCheckRight;
    [SerializeField] private Animator animator;

    [Header("Run")]
    [SerializeField] private float runSpeed = 9f;
    [SerializeField] private float acceleration = 13f;
    [SerializeField] private float deceleration = 16f;
    [SerializeField] private float power = 0.96f;
    [SerializeField] private float frictionAmount = 0.2f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityMultiplier = 2f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] [Range(0,1)] private float jumpCutMultiplier = 1.0f;
    [Space(10)]
    [SerializeField] private float lastGroundedTime = 0;
    [SerializeField] private float lastJumpTime = 0;
    [SerializeField] private bool isJumping = false;
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

    //----------------------------- Movement Calculations --------------------------------//
    private void Movement()
    {

        SetFallGravity();

        SetTimers();

        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            Jump();
        }

        Run();

        ApplyFriction();

        Mathf.Clamp(rigidBody.velocity.y, -maxVerticalSpeed, 10000);

        animator.SetFloat("VerticalSpeed", rigidBody.velocity.y);
    }

    private void SetTimers()
    {
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0;
        lastJumpTime = 0;
        isJumping = true;
    }

    private void SetFallGravity()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rigidBody.gravityScale = gravityScale;
        }
    }

    private void ApplyFriction()
    {
        if (grounded && Mathf.Abs(move) < 0.0f)
        {
            float amount = Mathf.Min(Mathf.Abs(rigidBody.velocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(rigidBody.velocity.x);

            rigidBody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private void Run()
    {
        float targetSpeed = move * runSpeed;

        float speedDiff = targetSpeed - rigidBody.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, power) * Mathf.Sign(speedDiff);

        rigidBody.AddForce(movement * Vector2.right);
    }

    private void CheckGround()
    {
        bool groundedA = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, .1f, colliderMask);
        bool groundedB = Physics2D.Raycast(groundCheckRight.position, Vector2.down, .1f, colliderMask);
        if (groundedA || groundedB)
        {
            grounded = true;
            isJumping = false;
            lastGroundedTime = coyoteTime;
        }
        else
        {
            grounded = false;
        }
        Color rayColor = grounded ? Color.red : Color.green;
        Debug.DrawRay(groundCheckLeft.position, Vector2.down * .1f, rayColor, .2f);
        Debug.DrawRay(groundCheckRight.position, Vector2.down * .1f, rayColor, .2f);

    }

    //----------------------------- Animations --------------------------------//
    private void FlipSprite()
    {
        if (move < 0)
        {
            facing = -1;
            sprite.flipX = true;
        }
        else if (move > 0)
        {
            facing = 1;
            sprite.flipX = false;
        }
        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    //----------------------------- Callbacks --------------------------------//

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            if (context.started)
            {
                lastJumpTime = jumpBufferTime;
            }
            if (context.performed || context.canceled)
            {
                if (rigidBody.velocity.y > 0 && isJumping)
                {
                    rigidBody.AddForce(Vector2.down * rigidBody.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
                }
                lastJumpTime = 0;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            move = context.ReadValue<Single>();
            if (move > 0)
            {
                move = 1;
            }
            else if (move < 0)
            {
                move = -1;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
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