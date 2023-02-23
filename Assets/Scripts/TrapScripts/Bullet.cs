using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
  void OnCollisionEnter2D(Collision2D Col) {
    Destroy(this.gameObject);
  }
}

