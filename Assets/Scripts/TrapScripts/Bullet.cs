using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
  void OnCollisionEnter2D(Collision2D Col) {
    // Destroy the bullet when collides with player
    Destroy(this.gameObject);
  }
}
