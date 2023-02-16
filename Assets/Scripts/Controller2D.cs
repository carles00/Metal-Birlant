using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Controller2D : MonoBehaviour
{
    [Range(1f, 10f)][SerializeField] private float jumpForce;
    [Range(0, .3f)] [SerializeField] private float acceleration;
    [SerializeField] private LayerMask colliderMask;
    [SerializeField] private Transform groundCheck;


    private Rigidbody2D rigidBody;
    private Vector3 velocity = Vector3.zero;
    private float groundCheckRadius = .2f;
    public UnityEvent onLand;
    

    bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, colliderMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                    onLand.Invoke();
            }
        }
    }

    public void Move(float move, bool jump)
    {

        Vector3 targetVelocity = new Vector2(move * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, acceleration);

        if(grounded && jump) {
            rigidBody.AddForce(new Vector2(0f, jumpForce*100));
        }
    }
}
