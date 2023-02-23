using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class Dash : MonoBehaviour {
    [SerializeField] private TrailRenderer TR;
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private float DashingPower = 24f;
    [SerializeField] private float DashingTime = 0.2f;
    [SerializeField] private float DashingCooldown = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private bool CanDash = true;

    void Start()
    {
        gameObject.GetComponent<PlayerMovement>();
    }

    IEnumerator DashLoop() {
        CanDash = false;
        float OriginalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        float facing = 1.0f;
        if (spriteRenderer.flipX)
        {
            facing = -1.0f;
        }

        rigidBody.velocity = new Vector2(facing * transform.localScale.x * DashingPower, 0f);
        //TR.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        //TR.emitting = false;
        rigidBody.gravityScale = OriginalGravity;
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && CanDash)
        {
            StartCoroutine(DashLoop());
        }
    }
}
