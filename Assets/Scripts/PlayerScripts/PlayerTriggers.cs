using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private GameController gc;
    
    void Start()
    {
        
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
        }
        if (collision.tag == "DarkHole") {
            StartCoroutine(SpawnAtStart());
            // gc.OnLoseLive();
        }
    }

    private IEnumerator SpawnAtStart() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(1f);
        rb.position = gc.GetPlayerSpawn().position;
    }
}
