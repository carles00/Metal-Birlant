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
    [SerializeField] private int baseMoney = 2000;
    [SerializeField] private int roundMultiplier = 3;

    [Header("Rounds")]
    [SerializeField] private int round = 0;
    [SerializeField] private int currentTurn = 0; //0 = pesent | 1 = future

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private CameraController cameraController;

    [Header("Future")]
    
    [SerializeField] private Transform leftMargin;
    [SerializeField] private Transform rightMargin;

    [Header("Traps")]
    [SerializeField] private int maxTraps = 20;
    [SerializeField] private int currentNumberOfTraps = 0;
    [SerializeField] private GameObject selectedTrap = null;
    [SerializeField] private GameObject followingTrap = null;

    [Header("Traps Economy")]
    [SerializeField] private int crossbowValue = 400;
    [SerializeField] private int teslaValue = 800;
    [SerializeField] private int blackHoleValue = 1200;
    [SerializeField] private int placedMoney = 0;

    [Header("Traps Prefabs")]
    [SerializeField] private GameObject crossbow;
    [SerializeField] private GameObject teslaTower;
    [SerializeField] private GameObject blackHole;
    [SerializeField] private string nameOfSelectedTrap = "";

    [SerializeField] List<GameObject> placedTraps;

    [Header("Interface")]
    [SerializeField] private GameObject futureInterface;
    [SerializeField] private TextMeshProUGUI availableTraps;
    [SerializeField] private TextMeshProUGUI moneyFuture;

    private PlayerInput input;
    private SceneControl sceneControl;
    

    void Start()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
        sceneControl = GetComponent<SceneControl>();
        input = GetComponent<PlayerInput>();
        presentPlayer.transform.position = playerSpawn.position;

        futureInterface.SetActive(false);
        Cursor.visible = false;

        availableTraps.text = maxTraps.ToString();
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
        //Give money to player
        CalculateMoney();

        //check if player lost
        if (money <=0)
        {
            sceneControl.OnPlayerLost();
        }

        ChangeToFuture();
    }

    private void CalculateMoney()
    {
        money += baseMoney + placedMoney;
        money -= baseMoney * (roundMultiplier * round);
    }

    private void ChangeToFuture()
    {
        currentTurn = 1;
        currentNumberOfTraps = 0;
        presentPlayer.SetActive(false);
        futureInterface.SetActive(true);
        Cursor.visible = true;

        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("UI");

        availableTraps.text = maxTraps.ToString();
        //Delete Existing Traps
        DestroyTraps();

        placedMoney = 0;
        moneyFuture.text = placedMoney.ToString();

    }

    private void ChangeToPresent()
    {
        currentTurn = 0;
        round++;
        maxTraps += 10;

        presentPlayer.transform.position = playerSpawn.position;
        
        presentPlayer.SetActive(true);
        futureInterface.SetActive(false);
        Cursor.visible = false;
        
        cameraController.SwitchPlayerFollowing();
        input.SwitchCurrentActionMap("Player");
        Destroy(followingTrap);
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
                availableTraps.text = (maxTraps - currentNumberOfTraps).ToString();
                switch (nameOfSelectedTrap)
                {
                    case "tesla":
                        placedMoney += teslaValue;
                        break;
                    case "blackHole":
                        placedMoney += blackHoleValue;
                        break;
                    case "crossbow":
                        placedMoney += crossbowValue;
                        break;
                    default:
                        break;
                }
                moneyFuture.text = placedMoney.ToString();
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
        nameOfSelectedTrap = "tesla";
    }

    public void OnSelectCrossbow()
    {
        selectedTrap = crossbow;
        followingTrap = Instantiate(selectedTrap, Mouse.current.position.ReadValue(), Quaternion.identity);
        nameOfSelectedTrap = "crossbow";
    }
    public void OnSelectBlackHole()
    {
        selectedTrap = blackHole;
        followingTrap = Instantiate(selectedTrap, Mouse.current.position.ReadValue(), Quaternion.identity);
        nameOfSelectedTrap = "blackHole";
    }
}
