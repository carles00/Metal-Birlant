using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private bool horizontal;
    [SerializeField] private float range;
    [SerializeField] private float force;
    [SerializeField] private float fireRate;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Vector2 direction;
    [SerializeField] private bool detected = false;
    [SerializeField] private bool firstDetection = false;

    private void Update() {
        target = GameObject.Find("PresentPlayer");
        if (!target) return;
        Vector2 TargetPos = target.transform.position;
        direction = TargetPos - (Vector2) transform.position;
        if(horizontal)
        {
            direction = new Vector2(direction.x, 0);
        }
        RaycastHit2D RayInfo = Physics2D.Raycast(transform.position, direction, range, playerLayer);

        // Player detection
        if (RayInfo) 
        {
            if (RayInfo.collider.gameObject.tag == "Player")
            {
                if (!detected) detected = true;
            }
            else if (detected) detected = false;
        }

        if (detected && !firstDetection) 
        {
            firstDetection = true;
            StartCoroutine(ShootLoop());
        }
        else if(!detected && firstDetection)
        {
            firstDetection = false;
            StopCoroutine(ShootLoop());
        }
        
    }
    
    private void Shoot() {
        GameObject BulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        // TODO: Rotate bullet with 'Direction'
        if(direction.x < 0 && horizontal)
            {
                SpriteRenderer sr = BulletIns.GetComponent<SpriteRenderer>();
                sr.flipX = true;
            }
        BulletIns.transform.rotation = Quaternion.Euler(direction);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force);
    }

    // Range debug indicator
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private IEnumerator ShootLoop()
    {
        if(detected)
        {
            Shoot();
        }
        yield return new WaitForSeconds(1f / fireRate);
        StartCoroutine(ShootLoop());
    }

}
