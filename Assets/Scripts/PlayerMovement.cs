using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller2D))]

public class PlayerMovement : MonoBehaviour
{
    Controller2D controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(new Vector3(0,0,0));
    }

    public void onJump(InputAction.CallbackContext context)
    {
        
    }

    public void onMove(InputAction.CallbackContext context)
    { 
        
    }
}
