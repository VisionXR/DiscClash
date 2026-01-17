using com.VisionXR.HelperClasses;               
using com.VisionXR.ModelClasses;
using UnityEngine;

public class KeyBoardInput : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public MyPlayerSettings myPlayerSettings;

    [Header("Prefabs")]
    [SerializeField] private GameObject OVRCameraRig;
    [SerializeField] private GameObject StandingRig;
    [SerializeField] private GameObject RController;
    [SerializeField] private GameObject LController;

    [Header(" Key Codes")]
    public KeyCode leftSwipe = KeyCode.A;
    public KeyCode rightSwipe = KeyCode.D;

    public KeyCode leftAim = KeyCode.LeftArrow;
    public KeyCode rightAim = KeyCode.RightArrow;

    public KeyCode startStrike = KeyCode.RightShift;
    public KeyCode endStrike = KeyCode.Return;


    private void Start()
    {
        if (!Application.isEditor)
        {
            this.enabled = false;
        }
    }

    /// <summary>
    /// Processes keyboard input data.
    /// </summary>
    private void ProcessKeyboardData()
    {

        HandlePauseInput();
        if (!inputData.isInputActivated) return;
        HandleSwipeInput();
        HandleButtonInput();
        HandleRotationInput();
        HandleStrikerFiring();
        HandlePlayerMovement();
        HandleThumbstickInput();

    }


    /// <summary>
    /// Handles swipe input from the keyboard.
    /// </summary>
    private void HandleSwipeInput()
    {
        if (Input.GetKey(leftSwipe))
        {
            inputData.Swiped(SwipeDirection.LEFT);
         
        }
        else if (Input.GetKey(rightSwipe))
        {
            inputData.Swiped(SwipeDirection.RIGHT);
           
        }
    }

    /// <summary>
    /// Handles button input from the keyboard.
    /// </summary>
    private void HandleButtonInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            inputData.GrabButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            inputData.TriggerButtonClicked();
        }
    }

    /// <summary>
    /// Handles rotation input from the keyboard.
    /// </summary>
    private void HandleRotationInput()
    {
        if (Input.GetKey(rightAim))
        {
            inputData.KeyboardRotated(SwipeDirection.LEFT);
        }
        else if (Input.GetKey(leftAim))
        {
            inputData.KeyboardRotated(SwipeDirection.RIGHT);
        }
    }

    /// <summary>
    /// Handles striker firing input from the keyboard.
    /// </summary>
    private void HandleStrikerFiring()
    {
        if (Input.GetKeyDown(startStrike))
        {
            inputData.FireStrikerWithForce(0.6f);
        }
        if (Input.GetKeyDown(endStrike))
        {
            inputData.FireStrikerWithForce(0);
        }
    }

    /// <summary>
    /// Handles player movement input from the keyboard.
    /// </summary>
    private void HandlePlayerMovement()
    {
        if (Input.GetKey(KeyCode.L))
        {
            inputData.MovePlayerX(1);
        }
        if (Input.GetKey(KeyCode.K))
        {
            inputData.MovePlayerX(-1);
        }
    }

    /// <summary>
    /// Handles thumbstick input from the keyboard.
    /// </summary>
    private void HandleThumbstickInput()
    {
        if (Input.GetKey(KeyCode.T))
        {

            inputData.ThumbStickLeftRightSwiped(1);
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            inputData.ThumbStickLeftRightSwiped(-1);
        }
    }

    /// <summary>
    /// Handles pause input from the keyboard.
    /// </summary>
    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            inputData.PauseButtonClicked();
        }
    }

    private void Update()
    {
        ProcessKeyboardData();
    }
}
    