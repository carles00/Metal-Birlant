using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Controller2D : MonoBehaviour
{
    [Range(1f, 10f)][SerializeField] private float jumpForce;
    [Range(0, .3f)] [SerializeField] private float acceleration;
    [SerializeField] private LayerMask colliderMask;

    private Rigidbody2D rigidBody;
    private Vector3 velocity = Vector3.zero;

    

    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void Move(float move, bool crouch, bool jump)
    {

        Vector3 targetVelocity = new Vector2(move * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, acceleration);

        
        if(grounded && jump) {
            rigidBody.AddForce(new Vector2(0f, jumpForce*100));
        }
    }
}
