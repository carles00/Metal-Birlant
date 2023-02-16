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

    private Rigidbody2D rigidBody;
    private BoxCollider2D collider;
    private Vector3 velocity = Vector3.zero;

    bool grounded;
    float move;
    bool jump = false;
  
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

 
    private void FixedUpdate()
    {   

    }

    private void movement(){

    }

    private bool isGrounded(){

        return false;
    }

    public void onJump(InputAction.CallbackContext context)
    {
        jump = true;
    }

    public void onMove(InputAction.CallbackContext context)
    {

        move = context.ReadValue<Single>() * runSpeed;
        
    }


}
