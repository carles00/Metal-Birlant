using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
  public GameObject Gun;
  public GameObject Bullet;
  public GameObject Target;
  public Transform ShootPoint;
  public float Range;
  public float Force;
  public float FireRate;

  Vector2 Direction;
  bool Detected = false;
  float NextTimeToFire = 0;

  void Update() {
    Target = GameObject.Find("PresentPlayer");
    if (!Target) return;
    Vector2 TargetPos = Target.transform.position;
    Direction = TargetPos - (Vector2) transform.position;
    RaycastHit2D RayInfo = Physics2D.Raycast(transform.position, Direction, Range);

    // Player detection
    if (RayInfo) {
      if (RayInfo.collider.gameObject.tag == "Player") {
        if (!Detected) Detected = true;
      }
      else if (Detected) Detected = false;
    }

    // Shoot if detected
    if (Detected) {
      if (Time.time > NextTimeToFire) {
        NextTimeToFire = Time.time + 1 / FireRate;
        Shoot();
      }
    }
  }
  
  void Shoot() {
    GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
    BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * Force);
  }

  // Range debug indicator
  void OnDrawGizmosSelected() {
    Gizmos.DrawWireSphere(transform.position, Range);
  }
}
