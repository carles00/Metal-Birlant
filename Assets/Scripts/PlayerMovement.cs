using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onJump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Single>();
        Debug.Log(context);
        
    }
}
