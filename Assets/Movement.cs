using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /*Vector3 tempPos;
    public float tenSec = 10;
    public bool timerDown = true;
    public bool timerUp = false;
    int i;*/
    Vector3 start;
    Vector3 end1;
    Vector3 end2;
    public float fract;
    public float speed;
    public int distance;
    public bool goUp;
    public bool goDown;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        goDown = true;
        goUp = false;
        end1 = start + new Vector3(0, -distance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (goDown)
        {
            fract += speed;
            transform.position = Vector3.Lerp(start, end1, fract);
            if (fract >= 1)
            {
                goDown = false;
                fract = 0;
                goUp = true;
            }
        }
        if (goUp)
        {
            fract += speed;
            transform.position = Vector3.Lerp(end1, start, fract);
            if (fract >= 1)
            {
                goUp = false;
                fract = 0;
                goDown = true;
            }
        }



    }
}
