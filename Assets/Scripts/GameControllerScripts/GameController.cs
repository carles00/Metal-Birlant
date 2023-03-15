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

    
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int money = 0;

    [Header("Money")]
    [SerializeField] private int baseMoney = 2000;
    [SerializeField] private int roundMultiplier = 3;
    [SerializeField] private int taxMoney = 1000;

    [Header("Rounds")]
    [SerializeField] private int round = 1;
    [SerializeField] private int currentTurn = 0; //0 = present | 1 = future

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

    [Header("Present Interface")]
    [SerializeField] private GameObject presentInterface;
    [SerializeField] private List<GameObject> fullHearts;
    [SerializeField] private List<GameObject> emptyHearts;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Future Interface")]
    [SerializeField] private GameObject futureInterface;
    [SerializeField] private TextMeshProUGUI availableTraps;
    [SerializeField] private TextMeshProUGUI moneyFuture;

    [Header("Inital Trap Pos")]
    [SerializeField] private List<Transform> initialCrosbowPositions;
    [SerializeField] private List<Transform> initialTeslaPositions;
    [SerializeField] private List<Transform> initialBHPositions;

    private PlayerInput input;
    private SceneControl sceneControl;
    

    void Start()
    {
        currentTurn = 0;
        cameraController = mainCamera.GetComponent<CameraController>();
        sceneControl = GetComponent<SceneControl>();
        input = GetComponent<PlayerInput>();
        presentPlayer.transform.position = playerSpawn.position;

        presentInterface.SetActive(true);
        futureInterface.SetActive(false);
        Cursor.visible = false;

        moneyText.text  = money.ToString();
        availableTraps.text = maxTraps.ToString();

        CreateInitialTraps();
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
        if(playerLives == 0)
        {
            Cursor.visible = true;
            sceneControl.OnPlayerLost();
        }
    }

    private void CreateInitialTraps()
    {
        foreach(Transform t in initialCrosbowPositions)
        {
            GameObject cb = Instantiate(crossbow,t.position, Quaternion.identity);
            placedTraps.Add(cb);
            placedMoney += crossbowValue;
        }
        foreach (Transform t in initialTeslaPositions)
        {
            GameObject tt = Instantiate(teslaTower, t.position, Quaternion.identity);
            placedTraps.Add(tt);
            placedMoney += teslaValue;
        }
        foreach (Transform t in initialBHPositions)
        {
            GameObject bh = Instantiate(blackHole, t.position, Quaternion.identity);
            placedTraps.Add(bh);
            placedMoney += blackHoleValue;
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
        money -= taxMoney * (roundMultiplier * round);
    }

    private void ChangeToFuture()
    {
        currentTurn = 1;
        currentNumberOfTraps = 0;
        presentPlayer.SetActive(false);
        futureInterface.SetActive(true);
        presentInterface.SetActive(false);
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
        playerLives = 3;
        currentTurn = 0;
        round++;
        maxTraps += 10;

        presentPlayer.transform.position = playerSpawn.position;
        
        presentPlayer.SetActive(true);
        futureInterface.SetActive(false);
        presentInterface.SetActive(true);

        for (int i = 0; i < fullHearts.Count; i++)
        {
            fullHearts[i].SetActive(true);
            emptyHearts[i].SetActive(false);
        }

        moneyText.text = money.ToString();

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

    public Transform GetPlayerSpawn() {
        return playerSpawn;
    }
    
    public int GetCurrentTurn()
    {
        return currentTurn;
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

    public void OnLoseLive()
    {
        playerLives--;
        for (int i = 0; i < fullHearts.Count; i++)
        {
            if (i < playerLives)
            {
                fullHearts[i].SetActive(true);
                emptyHearts[i].SetActive(false);
            }
            else
            {
                fullHearts[i].SetActive(false);
                emptyHearts[i].SetActive(true);
            }
        }
    }
}   
