using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Weapon : MonoBehaviour {
  [SerializeField] private GameObject Gun;
  [SerializeField] private GameObject Bullet;
  [SerializeField] private GameObject Target;
  [SerializeField] private Transform ShootPoint;
  [SerializeField] private bool horizontal;
  [SerializeField] private float Range;
  [SerializeField] private float Force;
  [SerializeField] private float FireRate;

  Vector2 Direction;
  bool Detected = false;
  float NextTimeToFire = 0;

  void Update() {
    Target = GameObject.Find("PresentPlayer");
    if (!Target) return;
    Vector2 TargetPos = Target.transform.position;
    Direction = TargetPos - (Vector2) transform.position;
    if(horizontal)
    {
        Direction = new Vector2(Direction.x, 0);
    }
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
        StartCoroutine(ShootLoop());
    }
  }
  
  void Shoot() {
    GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
    // TODO: Rotate bullet with 'Direction'
    if(Direction.x < 0 && horizontal)
        {
            SpriteRenderer sr = BulletIns.GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }
    BulletIns.transform.rotation = Quaternion.Euler(Direction);
    BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * Force);
  }

  // Range debug indicator
  void OnDrawGizmosSelected() {
    Gizmos.DrawWireSphere(transform.position, Range);
  }

    private IEnumerator ShootLoop()
    {
        Shoot();
        yield return new WaitForSeconds(1f/FireRate);
        ShootLoop();
    }

}
