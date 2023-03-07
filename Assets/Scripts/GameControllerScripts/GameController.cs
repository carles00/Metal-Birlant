using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject presentPlayer;
    [SerializeField] private GameObject futurePlayer;

    
    [SerializeField] private int playerLive = 3;
    [SerializeField] private int money = 0;

    [Header("Money")]
    [SerializeField] private int baseMoney = 200;
    [SerializeField] private float roundMultiplier = 1.5f;

    [Header("Rounds")]
    [SerializeField] private int round = 0;
    [SerializeField] private int currentTurn = 0;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private CameraController cameraController;

    [Header("Future")]
    [SerializeField] private GameObject futureInterface;

    private PlayerInput input;
    

    void Start()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
        input = GetComponent<PlayerInput>();
        presentPlayer.transform.position = playerSpawn.position;

        futureInterface.SetActive(false);
        futurePlayer.SetActive(false);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void TreasureReached()
    {
        ChangeToFuture();
        //Give money to player
        //Change to future
    }

    private void ChangeToFuture()
    {
        presentPlayer.SetActive(false);
        futurePlayer.SetActive(true);
        futureInterface.SetActive(true);
        Cursor.visible = true;

        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("UI");
    }

    private void ChangeToPresent()
    {
        presentPlayer.transform.position = playerSpawn.position;
        
        presentPlayer.SetActive(true);
        futurePlayer.SetActive(false);
        futureInterface.SetActive(false);
        Cursor.visible = false;
        
        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("Player");

    }
}
