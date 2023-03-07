using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Transform playerPosition;
    [Range(-5f, 5f)] [SerializeField] float offset = 0;
    [SerializeField][Range(0,0.5f)] private float lerpPositionStep = 0.05f;
    
    [Header("Scrolling")]
    [SerializeField] private float scrollDistance = 5;
    [SerializeField] private float maxHeight = 0;
    [SerializeField] private float minHeight = -140;
    
    [Header("FOV")]
    [SerializeField][Range(0, 0.5f)] private float lerpFovStep = 0.05f;
    [SerializeField] private int presentFov = 60;
    [SerializeField] private int futureFov = 90;
    private bool folowPlayer;
    private Vector3 cameraTarget;
    private float targetFov;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        folowPlayer = true;
        cameraTarget = transform.position;
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (folowPlayer)
        {
            cameraTarget = playerPosition.position;
            targetFov = presentFov;
        }
        else
        {
            targetFov = futureFov;
        }
        //camera position 
        float lerpedPosition = Mathf.Lerp(transform.position.y, cameraTarget.y, lerpPositionStep);
        float clampedPosition = Mathf.Clamp(lerpedPosition, minHeight, maxHeight);
        transform.position = new Vector3(0, clampedPosition, transform.position.z);

        //camera fov
        float lerpedFov = Mathf.Lerp(mainCamera.fieldOfView, targetFov, lerpFovStep);
        mainCamera.fieldOfView= lerpedFov;
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
                cameraTarget = new Vector3(0,transform.position.y + (scrollDirection * scrollDistance),transform.position.z);
            }
        }
    }
}
