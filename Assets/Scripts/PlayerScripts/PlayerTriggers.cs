using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private GameController gc;
    [SerializeField] private Transform voidSpawn;
    [SerializeField] private PlayerMovement pm;
    
    void Start()
    {
        pm = GetComponent<PlayerMovement>(); 
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "treasure")
        {
            gc.TreasureReached();
        }
        if(collision.tag == "bullet")
        {
            gc.OnLoseLive();
        }
        if(collision.tag == "void")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.position = voidSpawn.position;
            
            gc.OnLoseLive();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            gameObject.transform.SetParent(collision.gameObject.transform);
            pm.JumpOnPlatform(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            gameObject.transform.SetParent(null);
            pm.ResetPlatform();
        }
    }


}
