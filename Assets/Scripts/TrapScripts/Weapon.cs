using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
  public GameObject Gun;
  public GameObject Bullet;
  public GameObject AlarmLight;
  public Transform Target;
  public Transform ShootPoint;
  public float Range;
  public float Force;
  public float FireRate;

  Vector2 Direction;
  bool Detected = false;
  float NextTimeToFire = 0;
  
  void Update() {
    Vector2 TargetPos = Target.position;
    Direction = TargetPos - (Vector2) transform.position;

    RaycastHit2D RayInfo = Physics2D.Raycast(transform.position, Direction, Range);

    if (RayInfo) {
      if (RayInfo.collider.gameObject.tag == "Player") {
        if (Detected == false) {
          Detected = true;
          AlarmLight.GetComponent<SpriteRenderer>().color = Color.red;
        }
      }
      else {
        if (Detected == true) {
          Detected = false;
          AlarmLight.GetComponent<SpriteRenderer>().color = Color.green;
        }
      }
    }

    if (Detected) {
      Gun.transform.up = Direction;
      if (Time.time > NextTimeToFire) {
        NextTimeToFire = Time.time + 1 / FireRate;
        Shoot();
      }
    }
  }
  
  void Shoot() {
    GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
    BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
  }

  void OnDrawGizmosSelected() {
    Gizmos.DrawWireSphere(transform.position, Range);
  }
}

