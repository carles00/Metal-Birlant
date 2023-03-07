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

    
    [SerializeField] private int playerLive = 3;
    [SerializeField] private int money = 0;

    [Header("Money")]
    [SerializeField] private int baseMoney = 200;
    [SerializeField] private float roundMultiplier = 1.5f;

    [Header("Rounds")]
    [SerializeField] private int round = 0;
    [SerializeField] private int currentTurn = 0; //0 = pesent | 1 = future

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private CameraController cameraController;

    [Header("Future")]
    [SerializeField] private GameObject futureInterface;
    [SerializeField] private Transform leftMargin;
    [SerializeField] private Transform rightMargin;

    [Header("Traps")]
    [SerializeField] private int maxTraps = 20;
    [SerializeField] private int currentNumberOfTraps = 0;
    [SerializeField] private GameObject selectedTrap = null;
    [SerializeField] private GameObject followingTrap = null;

    [Header("Traps Prefabs")]
    [SerializeField] private GameObject teslaTower;
    [SerializeField] List<GameObject> placedTraps;

    private PlayerInput input;
    

    void Start()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
        input = GetComponent<PlayerInput>();
        presentPlayer.transform.position = playerSpawn.position;

        futureInterface.SetActive(false);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (followingTrap)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.transform.position.z));
            followingTrap.transform.position = new Vector3(-mousePos.x, -mousePos.y + 2 * mainCamera.transform.position.y, 0);
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
        currentTurn = 1;
        presentPlayer.SetActive(false);
        futureInterface.SetActive(true);
        Cursor.visible = true;

        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("UI");

        //Delete Existing Traps
        DestroyTraps();
    }

    private void ChangeToPresent()
    {
        currentTurn = 0;
        presentPlayer.transform.position = playerSpawn.position;
        
        presentPlayer.SetActive(true);
        futureInterface.SetActive(false);
        Cursor.visible = false;
        
        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("Player");

        followingTrap = null;
        selectedTrap = null;
    }

    private void CreateTrap(Vector3 pos)
    {
        if (selectedTrap)
        {
            if (currentNumberOfTraps < maxTraps)
            {
                placedTraps.Add(Instantiate(selectedTrap, pos, Quaternion.identity));
                currentNumberOfTraps++;
            }
        }
    }

    public void DestroyTraps()
    {
        placedTraps.ForEach(trap =>
        {
            Destroy(trap);
        });
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 screenPos = ctx.ReadValue<Vector2>();
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.transform.position.z));
            if (mousePos.x > leftMargin.position.x && mousePos.x < rightMargin.position.x)
            {
                Vector3 trapPos = new Vector3(-mousePos.x, -mousePos.y + 2 * mainCamera.transform.position.y, 0);
                CreateTrap(trapPos);
            }
        }
    }

    public void OnEndTurnClick()
    {
        ChangeToPresent();
    }

    public void OnSelectTesla()
    {
        selectedTrap = teslaTower;
        followingTrap = Instantiate(selectedTrap, Mouse.current.position.ReadValue(), Quaternion.identity);
    }
}
