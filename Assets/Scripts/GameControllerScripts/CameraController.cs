using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [Range(-5f, 5f)] [SerializeField] float offset = 0;
    [SerializeField] private bool folowPlayer;
    [SerializeField][Range(0,0.5f)] private float lerpStep = 0.05f;
    [SerializeField] private Vector3 cameraTarget;
    [SerializeField] private float scrollDistance = 5;
    [SerializeField] private float maxHeight = 0;
    [SerializeField] private float minHeight = -140;

    // Start is called before the first frame update
    void Start()
    {
        folowPlayer = true;
        cameraTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (folowPlayer)
        {
            cameraTarget = playerPosition.position;
        }
        
        float lerpedPosition = Mathf.Lerp(transform.position.y, cameraTarget.y, lerpStep);
        float clampedPosition = Mathf.Clamp(lerpedPosition, minHeight, maxHeight);
        transform.position = new Vector3(0, clampedPosition, transform.position.z);

    }

    public void SwitchPlayerFollowing()
    {
        folowPlayer = !folowPlayer;
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            float value = ctx.ReadValue<Vector2>().y;
            if(Mathf.Abs(value) > 0.1f) {
                float scrollDirection = Mathf.Sign(value);
                Debug.Log(scrollDirection);
                cameraTarget = new Vector3(0,transform.position.y + (scrollDirection * scrollDistance),transform.position.z);
            }
            
        }
        
    }
}
