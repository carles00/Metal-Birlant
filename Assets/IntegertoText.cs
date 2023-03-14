using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntegertoText : MonoBehaviour
{
    private int money;
    public GameController gameController;
    public TextMeshProUGUI t;

    // Update is called once per frame
    void Update()
    {
        money = gameController.getMoney();
        t.text = money.ToString();
    }
}
