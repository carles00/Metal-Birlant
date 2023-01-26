using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Controller2D : MonoBehaviour
{
    public LayerMask colliderMask;

    const float inset = .02f;

    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;

    RayOrigins rayOrigins;

    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    public void Move(Vector3 velocity)
    {
        setBounds();
        groundCollision();
    }

    void groundCollision()
    {
        float rayLength = .2f;
        
        RaycastHit2D hitA = Physics2D.Raycast(rayOrigins.bottomLeft, Vector2.down, rayLength, colliderMask);
        RaycastHit2D hitB = Physics2D.Raycast(rayOrigins.bottomLeft, Vector2.up, rayLength, colliderMask);

        grounded = false;
        if (hitA || hitB)
        {
            grounded = true;
        }

        Color color = Color.green;
        if (grounded)
        {
            color = Color.red;
        }
   
        Debug.DrawRay(rayOrigins.bottomLeft, Vector2.down * rayLength, color);
        Debug.DrawRay(rayOrigins.bottomRight, Vector2.down * rayLength, color);

              
    }

    void setBounds()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(inset * -2);
        
        rayOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
        rayOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        rayOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        rayOrigins.xSize = bounds.max.x - bounds.min.x;
        rayOrigins.ySize = bounds.max.y - bounds.min.y;

       
    }

    

    struct RayOrigins{
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public float xSize, ySize;

    }
}
