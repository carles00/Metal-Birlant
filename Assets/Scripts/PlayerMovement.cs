using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller2D))]

public class PlayerMovement : MonoBehaviour
{
    Controller2D controller;

    public float runSpeed = 40f;
    float move;
    bool jump = false;

  
    void Start()
    {
        controller = gameObject.GetComponent<Controller2D>();
    }

 
    private void FixedUpdate()
    {
        controller.Move(move * Time.fixedDeltaTime, jump);
        jump = false;
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
