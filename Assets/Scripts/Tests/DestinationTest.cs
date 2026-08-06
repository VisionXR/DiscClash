using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestinationTest : MonoBehaviour
{

    public DeepLinkManager deepLinkManager;
    public SinglePlayerGameManager singlePlayerGameManager;
    public Destination testDestination;

    [Header("Key Bindings (New Input System)")]
    public Key JoinDestinationKey = Key.J;



    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;


        if (kb[JoinDestinationKey].wasPressedThisFrame)
        {
            Debug.Log("Connecting to test destination...");
            deepLinkManager.ConnectToDestination(testDestination, null, null);
            singlePlayerGameManager.StartGame();
        }


    }
}
