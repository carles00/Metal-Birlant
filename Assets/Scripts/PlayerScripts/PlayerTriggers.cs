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
        if (collision.tag == "Bullet") {
            Destroy(collision.gameObject);
            gc.OnLoseLive();
        }
        if (collision.tag == "DarkHole") {
            StartCoroutine(SpawnAtStart());
            gc.OnLoseLive();
        }
        if(collision.tag == "void")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.position = voidSpawn.position;

            gc.OnLoseLive();
        }
    }

    private IEnumerator SpawnAtStart() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(1f);
        rb.position = gc.GetPlayerSpawn().position;
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
