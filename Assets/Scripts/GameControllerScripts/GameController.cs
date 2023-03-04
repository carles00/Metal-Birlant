using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject player;

    
    [SerializeField] private int playerLive = 3;
    [SerializeField] private int money = 0;

    [Header("Money")]
    [SerializeField] private int baseMoney = 200;
    [SerializeField] private float roundMultiplier = 1.5f;

    [Header("Rounds")]
    [SerializeField] private int round = 0;
    [SerializeField] private int currentTurn = 0;

    void Start()
    {
        player.transform.position = playerSpawn.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TreasureReached()
    {
        Debug.Log("Treasure from game controller");
        //Give money to player
        //Change to future

    }

    public void TreasureReached()
    {
        Debug.Log("Treasure from game controller");
        //Give money to player
        //Change to future

    }
}
