using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkHole : MonoBehaviour {
    public GameObject player;
    public float influenceRange;
    public float intensity;
    public float distanceToPlayer;

    Rigidbody2D playerBody;
    Vector2 pullForce;

    void Update() {
        player = GameObject.Find("PresentPlayer");
        if (!player) return;
        playerBody = player.transform.GetComponent<Rigidbody2D>();
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= influenceRange) {
            pullForce = (transform.position - player.transform.position).normalized / distanceToPlayer * intensity;
            playerBody.AddForce(pullForce, ForceMode2D.Force);
        }
    }

    // Range debug indicator
    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, influenceRange);
    }
}