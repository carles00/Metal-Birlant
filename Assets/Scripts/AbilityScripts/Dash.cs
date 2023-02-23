using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dash : MonoBehaviour {
  [SerializeField] TrailRenderer TR;
  
  bool CanDash = true;
  bool IsDashing = false;
  float DashingPower = 24f;
  float DashingTime = 0.2f;
  float DashingCooldown = 1f;
  
  void Update() {
    if (IsDashing) {
      return;
    }

    if (Input.GetKeyDown(KeyCode.Q) && CanDash) {
      StartCoroutine(Dash());
    }
  }

  void FixedUpdate() {
    if (IsDashing) {
      return;
    }
  }

  IEnumerator Dash(Rigidbody2D RB) {
    CanDash = false;
    IsDashing = true;
    float OriginalGravity = RB.gravityScale;
    RB.gravityScale = 0f;
    RB.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);
    TR.emitting = true;
    yield return new WaitForSeconds(DashingTime);
    TR.emitting = false;
    RB.gravityScale = OriginalGravity;
    IsDashing = false;
    yield return new WaitForSeconds(DashingCooldown);
    CanDash = true;
  }
}
