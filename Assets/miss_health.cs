using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miss_health : MonoBehaviour
{
    public GameController gameController;
    public GameObject parent;
    private int playerlive;

    void Update()
    {
        playerlive = gameController.getLives();

        if(playerlive == 2)
        {
            parent.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
        }

        if (playerlive == 1)
        {
            parent.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
            parent.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        }

        if (playerlive == 0)
        {
            parent.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
            parent.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
            parent.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
