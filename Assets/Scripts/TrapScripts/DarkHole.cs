using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkHole : MonoBehaviour {
    public Transform player;
    public float influenceRange;
    public float intensity;
    public float distanceToPlayer;

    Rigidbody2D playerBody;
    Vector2 pullForce;

    void Start() {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= influenceRange) {
            pullForce = (transform.position - player.position).normalized / distanceToPlayer * intensity;
            playerBody.AddForce(pullForce, ForceMode2D.Force);
        }
    }
}