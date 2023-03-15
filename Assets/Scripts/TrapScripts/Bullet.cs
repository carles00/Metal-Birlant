using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D Col) {
    // Destroy the bullet when exists the map edges
    if (Col.gameObject.tag == "Destroyer") Destroy(this.gameObject);
  }

  void OnColliderEnter2D(Collider2D Col) {
    Debug.Log("Hola :)");
    // Destroy the bullet when collides with player
    if (Col.gameObject.tag == "Player") Destroy(this.gameObject);
  }
}
