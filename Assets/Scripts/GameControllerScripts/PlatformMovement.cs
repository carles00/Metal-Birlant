using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    /*Vector3 tempPos;
    public float tenSec = 10;
    public bool timerDown = true;
    public bool timerUp = false;
    int i;*/
     private Vector3 start;
     private Vector3 end;

    [SerializeField] private float fract;
    [SerializeField] private float speed;
    [SerializeField] private int distance;
    [SerializeField] private bool goUp;
    [SerializeField] private bool goDown;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        goDown = true;
        goUp = false;
        end = start + new Vector3(0, -distance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (goDown)
        {
            fract += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, fract);
            if (fract >= 1)
            {
                goDown = false;
                fract = 0;
                goUp = true;
            }
        }
        if (goUp)
        {
            fract += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(end, start, fract);
            if (fract >= 1)
            {
                goUp = false;
                fract = 0;
                goDown = true;
            }
        }
    }

    public bool IsGoingUp()
    {
        return goUp;
    }
}
