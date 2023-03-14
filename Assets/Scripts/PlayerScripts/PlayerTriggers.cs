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
        if(collision.tag == "bullet")
        {
            gc.OnLoseLive();
        }
    }
}
