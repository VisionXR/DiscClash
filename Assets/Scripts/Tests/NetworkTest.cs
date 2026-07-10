using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkTest : MonoBehaviour
{
    public NetworkOutputSO networkOutputData;
    public NetworkInputSO networkInputData;



    [Header("Key Bindings (New Input System)")]
    public Key CreateRoomKey = Key.C;
    public Key JoinRoomKey = Key.J;

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;


        if (kb[CreateRoomKey].wasPressedThisFrame)
        {
            Debug.Log("Creating Room...");
            networkInputData.CreateRoom(ServerRegion.any, "TestRoom", () => Debug.Log("Room Created Successfully"), (error) => Debug.LogError($"Failed to create room: {error}"));
        }

        if (kb[JoinRoomKey].wasPressedThisFrame)
        {
            Debug.Log("Joining Room...");
            networkInputData.JoinRoom(ServerRegion.any, "TestRoom", () => Debug.Log("Joined Room Successfully"), (error) => Debug.LogError($"Failed to join room: {error}"));
        }
    }
}
