using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [Range(-5f, 5f)] [SerializeField] float offset = 0;
    [SerializeField] private bool folowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        folowPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(folowPlayer) {
            transform.position = new Vector3(0, playerPosition.position.y + offset, -10);
        }
    }

    public void SwitchPlayerFollowing()
    {
        folowPlayer = !folowPlayer;
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }
}
